

locals {
  cidr_block   = "10.0.0.0/16"
  s3_origin_id = "djustice-nextapp"

  tags = {
    project = "next-dotnet-postgres"
  }
}

data "aws_availability_zones" "available" {
  state = "available"
}

resource "aws_s3_bucket" "logs" {
  bucket = "djustice-gb-logs"
}

resource "aws_s3_bucket_acl" "logs_acl" {
  bucket = aws_s3_bucket.logs.id
  acl    = "private"
}

# IAM


# Network
## VPC
#TODO: Rework VPC from scratch
resource "aws_vpc" "main" {
  cidr_block           = local.cidr_block
  enable_dns_support   = true
  enable_dns_hostnames = true
  tags = local.tags
}

resource "aws_internet_gateway" "gw" {
  vpc_id = aws_vpc.main.id
}

resource "aws_subnet" "public_0" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.0.0/24"
  availability_zone = data.aws_availability_zones.available.names[0]
  tags = local.tags
}
resource "aws_subnet" "public_1" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.1.0/24"
  availability_zone = data.aws_availability_zones.available.names[1]
  tags = local.tags
}
resource "aws_subnet" "private_00" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.100.0/24"
  availability_zone = data.aws_availability_zones.available.names[0]
  tags = local.tags
}
resource "aws_subnet" "private_10" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.200.0/24"
  availability_zone = data.aws_availability_zones.available.names[1]
  tags = local.tags
}
resource "aws_subnet" "private_01" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.101.0/24"
  availability_zone = data.aws_availability_zones.available.names[0]
  tags = local.tags
}
resource "aws_subnet" "private_11" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.201.0/24"
  availability_zone = data.aws_availability_zones.available.names[1]
  tags = local.tags
}

resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.gw.id
  }
}
resource "aws_route_table" "private" {
  vpc_id = aws_vpc.main.id
  route = []
}

resource "aws_route_table_association" "private_00" {
  subnet_id = aws_subnet.private_00.id
  route_table_id = aws_route_table.private.id
}
resource "aws_route_table_association" "private_01" {
  subnet_id = aws_subnet.private_01.id
  route_table_id = aws_route_table.private.id
}
resource "aws_route_table_association" "private_10" {
  subnet_id = aws_subnet.private_10.id
  route_table_id = aws_route_table.private.id
}
resource "aws_route_table_association" "private_11" {
  subnet_id = aws_subnet.private_11.id
  route_table_id = aws_route_table.private.id
}

resource "aws_security_group" "db" {
  name        = "secgroup-gravityboots-db-${var.environment}"
  description = "Traffic to the database"
  vpc_id      = aws_vpc.main.id
  ingress {
    protocol    = "tcp"
    description = "App Subnet traffic to database"
    from_port   = 443
    to_port     = 1433
    cidr_blocks = [local.cidr_block]
  }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = local.tags
}
resource "aws_db_subnet_group" "db_subnet_group" {
  name       = "main"
  subnet_ids = [aws_subnet.private_01.id, aws_subnet.private_11.id]
  tags = local.tags
}

# Database
resource "aws_db_instance" "db" {
  allocated_storage      = 10
  availability_zone      = data.aws_availability_zones.available.names[0]
  db_subnet_group_name   = aws_db_subnet_group.db_subnet_group.name
  engine                 = "postgres"
  engine_version         = 14.2
  instance_class         = "db.t3.medium"
  password               = var.db_password
  publicly_accessible    = true
  skip_final_snapshot    = true
  username               = var.db_username
  vpc_security_group_ids = [aws_security_group.db.id]
  identifier             = "db-gravitybootsapi-db-${var.environment}"
  multi_az               = var.environment == "production" ? true : false
  tags = local.tags
}

# EC2
## Security Group
resource "aws_security_group" "allow_tls" {
  name        = "allow_tls"
  description = "Allow TLS inbound traffic"
  vpc_id      = aws_vpc.main.id

  ingress {
    description = "TLS from VPC"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = [local.cidr_block]
  }

  egress {
    from_port        = 0
    to_port          = 0
    protocol         = "-1"
    cidr_blocks      = ["0.0.0.0/0"]
    ipv6_cidr_blocks = ["::/0"]
  }
  tags = local.tags
}

# API
# Elastic container registry
resource "aws_ecr_repository" "repo" {
  name                 = "ecr-gravityboots"
  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }
  encryption_configuration {
    encryption_type = "KMS"
  }
  tags = local.tags
}

# ECS

resource "aws_ecs_cluster" "api" {
  name = "cluster-gb-api"

  setting {
    name  = "containerInsights"
    value = "enabled"
  }
  tags = local.tags
}

resource "aws_ecs_cluster_capacity_providers" "api" {
  cluster_name       = aws_ecs_cluster.api.name
  capacity_providers = ["FARGATE"]
  default_capacity_provider_strategy {
    base              = 1
    weight            = 100
    capacity_provider = "FARGATE"
  }
}

# resource "aws_ecs_task_definition" "api" {
#   family                   = "taskdef-gbapi-${var.environment}"
#   requires_compatibilities = ["FARGATE"]
#   container_definitions = jsonencode([
#     {
#       name  = "dotnet-api"
#       image = ""
#     }
#   ])
# }

# resource "aws_ecs_service" "api" {
#   name            = "service-gb-api-${var.environment}"
#   cluster         = aws_ecs_cluster.api.id
#   task_definition = aws_ecs_task_definition.api.id
#   tags = local.tags
# }


# Frontend
resource "aws_cloudfront_origin_access_identity" "oai" {}

resource "aws_s3_bucket" "b" {
  bucket = "gb-frontend"
}

resource "aws_s3_bucket_acl" "b_acl" {
  bucket = aws_s3_bucket.b.id
  acl    = "private"
}

data "aws_iam_policy_document" "s3_policy" {
  statement {
    actions   = ["s3:GetObject"]
    resources = ["${aws_s3_bucket.b.arn}/*"]
    principals {
      type        = "AWS"
      identifiers = [aws_cloudfront_origin_access_identity.oai.iam_arn]
    }
  }
}

resource "aws_s3_bucket_policy" "b_policy" {
  bucket = aws_s3_bucket.b.id
  policy = data.aws_iam_policy_document.s3_policy.json
}


resource "aws_cloudfront_distribution" "s3_distribution" {
  origin {
    domain_name = aws_s3_bucket.b.bucket_regional_domain_name
    origin_id   = local.s3_origin_id

    s3_origin_config {
      origin_access_identity = aws_cloudfront_origin_access_identity.oai.cloudfront_access_identity_path
    }
  }

  enabled             = true
  is_ipv6_enabled     = true
  default_root_object = "index.html"

  logging_config {
    include_cookies = false
    bucket          = aws_s3_bucket.logs.bucket_domain_name
    prefix          = "cloudfront"
  }

  aliases = []

  default_cache_behavior {
    allowed_methods  = ["DELETE", "GET", "HEAD", "OPTIONS", "PATCH", "POST", "PUT"]
    cached_methods   = ["GET", "HEAD"]
    target_origin_id = local.s3_origin_id

    forwarded_values {
      query_string = false

      cookies {
        forward = "none"
      }
    }

    viewer_protocol_policy = "allow-all"
    min_ttl                = 0
    default_ttl            = 3600
    max_ttl                = 86400
  }

  # Cache behavior with precedence 0
  ordered_cache_behavior {
    path_pattern     = "/content/immutable/*"
    allowed_methods  = ["GET", "HEAD", "OPTIONS"]
    cached_methods   = ["GET", "HEAD", "OPTIONS"]
    target_origin_id = local.s3_origin_id

    forwarded_values {
      query_string = false
      headers      = ["Origin"]

      cookies {
        forward = "none"
      }
    }

    min_ttl                = 0
    default_ttl            = 86400
    max_ttl                = 31536000
    compress               = true
    viewer_protocol_policy = "redirect-to-https"
  }

  # Cache behavior with precedence 1
  ordered_cache_behavior {
    path_pattern     = "/content/*"
    allowed_methods  = ["GET", "HEAD", "OPTIONS"]
    cached_methods   = ["GET", "HEAD"]
    target_origin_id = local.s3_origin_id

    forwarded_values {
      query_string = false

      cookies {
        forward = "none"
      }
    }

    min_ttl                = 0
    default_ttl            = 3600
    max_ttl                = 86400
    compress               = true
    viewer_protocol_policy = "redirect-to-https"
  }

  price_class = "PriceClass_200"

  restrictions {
    geo_restriction {
      restriction_type = "whitelist"
      locations        = ["US", "CA", "GB", "DE"]
    }
  }

  tags = {
    Environment = "production"
  }

  viewer_certificate {
    cloudfront_default_certificate = true
  }
}

