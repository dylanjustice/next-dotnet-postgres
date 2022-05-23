
locals {
  cidr_block   = "10.0.0.0/16"
  cloudfront_origin_id = "djustice-nextapp"
  region = "us-east-2"

  tags = {
    project = "next-dotnet-postgres"
  }
}

data "aws_partition" "current" {}
data "aws_caller_identity" "current" {}

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

data "aws_iam_policy_document" "fargate_assume_role" {
  statement {
    actions = [
      "sts:AssumeRole"
    ]
    effect  = "Allow"
    principals {
      type        = "Service"
      identifiers = ["ecs-tasks.amazonaws.com"]
    }
  }
}
data "aws_iam_policy_document" "logs" {
  statement {
    actions = [
      "logs:CreateLogGroup",
      "logs:CreateLogStream",
      "logs:PutLogEvents"
    ]
    effect = "Allow"
    resources = [
      "arn:${data.aws_partition.current.partition}:logs:${local.region}:${data.aws_caller_identity.current.account_id}:log-group:*",
      "arn:${data.aws_partition.current.partition}:logs:${local.region}:${data.aws_caller_identity.current.account_id}:log-stream:*"
    ]
  }
}

resource "aws_iam_role" "fargate" {
  name                = "fargate"
  assume_role_policy  = data.aws_iam_policy_document.fargate_assume_role.json
  inline_policy {
    name = "AllowCloudwatch"
    policy = data.aws_iam_policy_document.logs.json
  }
  path                = "/"
  managed_policy_arns = ["arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"]
  #TODO: Get AWS Secret value??
}

# Network
## VPC
resource "aws_vpc" "main" {
  cidr_block           = "10.0.0.0/16"
  enable_dns_support   = true
  enable_dns_hostnames = true
  tags = merge(local.tags, {
    Name = "vpc-gb-${var.environment}"
  })
}
resource "aws_vpc" "mockaroo" {
  cidr_block           = "10.1.0.0/16"
  enable_dns_support   = true
  enable_dns_hostnames = true
  tags = merge(local.tags, {
    Name = "vpc-mockaroo-${var.environment}"
  })
}

resource "aws_vpc_peering_connection" "main_mockaroo" {
  peer_owner_id = data.aws_caller_identity.current.account_id
  peer_vpc_id   = aws_vpc.main.id
  vpc_id        = aws_vpc.mockaroo.id
  tags = merge(local.tags, {
    Name = "main => mockaroo"
  })
}
resource "aws_vpc_peering_connection" "mockaroo_main" {
  peer_owner_id = data.aws_caller_identity.current.account_id
  peer_vpc_id   = aws_vpc.mockaroo.id
  vpc_id        = aws_vpc.main.id
  tags = merge(local.tags, {
    Name = "mockaroo => main"
  })
}

resource "aws_subnet" "mockaroo_private_1" {
  vpc_id            = aws_vpc.mockaroo.id
  cidr_block        = "10.1.100.0/24"
  availability_zone = data.aws_availability_zones.available.names[0]
  tags = merge(local.tags, {
    Name = "snet-mockaroo-private-1"
  })
}

resource "aws_subnet" "mockaroo_private_2" {
  vpc_id            = aws_vpc.mockaroo.id
  cidr_block        = "10.1.200.0/24"
  availability_zone = data.aws_availability_zones.available.names[1]
  tags = merge(local.tags, {
    Name = "snet-mockaroo-private-2"
  })
}
resource "aws_subnet" "mockaroo_public_0" {
  vpc_id            = aws_vpc.mockaroo.id
  cidr_block        = "10.1.0.0/24"
  availability_zone = data.aws_availability_zones.available.names[0]
  tags = merge(local.tags, {
    Name = "snet-mockaroo-public-0"
  })
}
resource "aws_subnet" "mockaroo_public_1" {
  vpc_id            = aws_vpc.mockaroo.id
  cidr_block        = "10.1.1.0/24"
  availability_zone = data.aws_availability_zones.available.names[1]
  tags = merge(local.tags, {
    Name = "snet-mockaroo-public-1"
  })
}



resource "aws_subnet" "public_0" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.0.0/24"
  availability_zone = data.aws_availability_zones.available.names[0]
  tags              = local.tags
}
resource "aws_subnet" "public_1" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.1.0/24"
  availability_zone = data.aws_availability_zones.available.names[1]
  tags              = local.tags
}
resource "aws_subnet" "private_00" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.100.0/24"
  availability_zone = data.aws_availability_zones.available.names[0]
  tags              = local.tags
}
resource "aws_subnet" "private_10" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.200.0/24"
  availability_zone = data.aws_availability_zones.available.names[1]
  tags              = local.tags
}
resource "aws_subnet" "private_01" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.101.0/24"
  availability_zone = data.aws_availability_zones.available.names[0]
  tags              = local.tags
}
resource "aws_subnet" "private_11" {
  vpc_id            = aws_vpc.main.id
  cidr_block        = "10.0.201.0/24"
  availability_zone = data.aws_availability_zones.available.names[1]
  tags              = local.tags
}

resource "aws_route_table" "public" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.gw.id
  }
}
resource "aws_route_table" "private_0" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block = "0.0.0.0/0"
    nat_gateway_id = aws_nat_gateway.main_0.id
  }
}
resource "aws_route_table" "private_1" {
  vpc_id = aws_vpc.main.id
  route {
    cidr_block = "0.0.0.0/0"
    nat_gateway_id = aws_nat_gateway.main_1.id
  }
}

resource "aws_route_table_association" "private_00" {
  subnet_id      = aws_subnet.private_00.id
  route_table_id = aws_route_table.private_0.id
}
resource "aws_route_table_association" "private_01" {
  subnet_id      = aws_subnet.private_01.id
  route_table_id = aws_route_table.private_0.id
}
resource "aws_route_table_association" "private_10" {
  subnet_id      = aws_subnet.private_10.id
  route_table_id = aws_route_table.private_1.id
}
resource "aws_route_table_association" "private_11" {
  subnet_id      = aws_subnet.private_11.id
  route_table_id = aws_route_table.private_1.id
}
resource "aws_route_table_association" "public_0" {
  subnet_id      = aws_subnet.public_0.id
  route_table_id = aws_route_table.public.id
}
resource "aws_route_table_association" "public_1" {
  subnet_id      = aws_subnet.public_1.id
  route_table_id = aws_route_table.public.id
}

resource "aws_route_table" "mockaroo_public" {
  vpc_id = aws_vpc.mockaroo.id
  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.mockaroo_gw.id
  }
}
resource "aws_route_table" "mockaroo_private" {
  vpc_id = aws_vpc.mockaroo.id
  route  = []
}

resource "aws_route_table_association" "mockaroo_private_00" {
  subnet_id      = aws_subnet.mockaroo_private_1.id
  route_table_id = aws_route_table.mockaroo_private.id
}
resource "aws_route_table_association" "mockaroo_private_01" {
  subnet_id      = aws_subnet.mockaroo_private_2.id
  route_table_id = aws_route_table.mockaroo_private.id
}

resource "aws_network_acl" "main" {
  vpc_id     = aws_vpc.main.id
  subnet_ids = [aws_subnet.public_0.id, aws_subnet.public_1.id]
  tags = merge(local.tags, {
    Name = "-public-nacl-api"
  })
  egress {
    rule_no    = 100
    protocol   = "tcp"
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 65535
  }
  ingress {
    rule_no    = 100
    protocol   = "tcp"
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 65535
  }
}
resource "aws_network_acl" "mockaroo" {
  vpc_id     = aws_vpc.mockaroo.id
  subnet_ids = [aws_subnet.mockaroo_public_0.id, aws_subnet.mockaroo_public_1.id]
  tags = merge(local.tags, {
    Name = "-public-nacl-mockaroo"
  })
  egress {
    rule_no    = 100
    protocol   = "tcp"
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 65535
  }
  ingress {
    rule_no    = 100
    protocol   = "tcp"
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = 0
    to_port    = 65535
  }
}

resource "aws_internet_gateway" "gw" {
  vpc_id = aws_vpc.main.id
}
resource "aws_internet_gateway" "mockaroo_gw" {
  vpc_id = aws_vpc.mockaroo.id
}

resource "aws_eip" "main_0" {
  vpc = true
}
resource "aws_eip" "main_1" {
  vpc = true
}
resource "aws_eip" "mockaroo_0" {
  vpc = true
}
resource "aws_eip" "mockaroo_1" {
  vpc = true
}

resource "aws_nat_gateway" "main_0" {
  allocation_id = aws_eip.main_0.id
  subnet_id     = aws_subnet.public_0.id
  tags = merge(local.tags, {
    Name = "nat-main0"
  })
}
resource "aws_nat_gateway" "main_1" {
  allocation_id = aws_eip.main_1.id
  subnet_id     = aws_subnet.public_1.id
  tags = merge(local.tags, {
    Name = "nat-main1"
  })
}
resource "aws_nat_gateway" "mockaroo_0" {
  allocation_id = aws_eip.mockaroo_0.id
  subnet_id     = aws_subnet.mockaroo_public_0.id
  tags = merge(local.tags, {
    Name = "nat-mockaroo-0"
  })
}
resource "aws_nat_gateway" "mockaroo_1" {
  allocation_id = aws_eip.mockaroo_1.id
  subnet_id     = aws_subnet.mockaroo_public_1.id
  tags = merge(local.tags, {
    Name = "nat-mockaroo-0"
  })
}

resource "aws_security_group" "db" {
  name        = "secgroup-gravityboots-db-${var.environment}"
  description = "Traffic to the database"
  vpc_id      = aws_vpc.main.id
  ingress {
    protocol    = "tcp"
    description = "App Subnet traffic to database"
    from_port   = 5432
    to_port     = 5432
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
resource "aws_security_group" "alb" {
  name        = "secgroup-gravityboots-alb-${var.environment}"
  description = "Traffic to the application load balancer"
  vpc_id      = aws_vpc.main.id
  ingress {
    protocol    = "tcp"
    description = "Ingress HTTP to ALB"
    from_port   = 80
    to_port     = 80
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    protocol    = "tcp"
    description = "Ingress HTTP to ALB"
    from_port   = 80
    to_port     = 3000
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    protocol         = "tcp"
    description      = "Ingress Ipv6 traffic to ALB"
    from_port        = 80
    to_port          = 80
    ipv6_cidr_blocks = ["::/0"]
  }
  egress {
    from_port   = 0
    to_port     = 65535
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = local.tags
}
resource "aws_security_group" "mockaroo_alb" {
  name        = "secgroup-mockaroo-alb-${var.environment}"
  description = "Traffic to the application load balancer"
  vpc_id      = aws_vpc.mockaroo.id
  ingress {
    protocol    = "tcp"
    description = "Ingress HTTP to ALB"
    from_port   = 80
    to_port     = 80
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    protocol    = "tcp"
    description = "Ingress HTTP to ALB"
    from_port   = 80
    to_port     = 3000
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    protocol         = "tcp"
    description      = "Ingress Ipv6 traffic to ALB"
    from_port        = 80
    to_port          = 80
    ipv6_cidr_blocks = ["::/0"]
  }
  egress {
    from_port   = 0
    to_port     = 65535
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  tags = local.tags
}

resource "aws_security_group" "ecs" {
  name        = "secgroup-gravityboots-ecs-${var.environment}"
  description = "ECS SG"
  vpc_id      = aws_vpc.main.id
  ingress {
    protocol        = "tcp"
    to_port         = 65535
    from_port       = 0
    security_groups = [aws_security_group.alb.id]
  }
  egress {
    protocol    = "tcp"
    from_port   = 0
    to_port     = 65535
    cidr_blocks = ["0.0.0.0/0"]
  }

}
resource "aws_security_group" "ecs_mockaroo" {
  name        = "secgroup-mockaroo-ecs-${var.environment}"
  description = "ECS SG"
  vpc_id      = aws_vpc.mockaroo.id
  ingress {
    protocol        = "tcp"
    to_port         = 65535
    from_port       = 0
    security_groups = [aws_security_group.mockaroo_alb.id]
  }
  egress {
    protocol    = "tcp"
    from_port   = 0
    to_port     = 65535
    cidr_blocks = ["0.0.0.0/0"]
  }

}
resource "aws_lb" "main" {
  name               = "alb-gravityboots"
  internal           = false
  load_balancer_type = "application"
  subnets            = [aws_subnet.public_0.id, aws_subnet.public_1.id]
  security_groups    = [aws_security_group.alb.id]
}
resource "aws_lb" "mockaroo" {
  name               = "alb-mockaroo"
  internal           = false
  load_balancer_type = "application"
  subnets            = [aws_subnet.mockaroo_public_0.id, aws_subnet.mockaroo_public_1.id]
  security_groups    = [aws_security_group.mockaroo_alb.id]
}

resource "aws_lb_target_group" "api_01" {
  name        = "tg-gravityboots-api-01"
  target_type = "ip"
  protocol    = "HTTP"
  port        = 80
  vpc_id      = aws_vpc.main.id
}
resource "aws_lb_target_group" "frontend_01" {
  name        = "tg-gravityboots-frontend-01"
  target_type = "ip"
  protocol    = "HTTP"
  port        = 3000
  vpc_id      = aws_vpc.main.id
}
resource "aws_lb_target_group" "mockaroo" {
  name        = "tg-mockaroo"
  target_type = "ip"
  protocol    = "HTTP"
  port        = 80
  vpc_id      = aws_vpc.mockaroo.id
}
resource "aws_lb_listener" "api" {
  load_balancer_arn = aws_lb.main.arn
  port              = 80
  protocol          = "HTTP"
  default_action {
    target_group_arn = aws_lb_target_group.api_01.arn
    type             = "forward"
  }
}
resource "aws_lb_listener" "frontend" {
  load_balancer_arn = aws_lb.main.arn
  port              = 3000
  protocol          = "HTTP"
  default_action {
    target_group_arn = aws_lb_target_group.frontend_01.arn
    type             = "forward"
  }
}

resource "aws_lb_listener" "mockaroo" {
  load_balancer_arn = aws_lb.mockaroo.arn
  port              = 80
  protocol          = "HTTP"
  default_action {
    target_group_arn = aws_lb_target_group.mockaroo.arn
    type             = "forward"
  }
}


resource "aws_db_subnet_group" "db_subnet_group" {
  name       = "main"
  subnet_ids = [aws_subnet.private_01.id, aws_subnet.private_11.id]
  tags       = local.tags
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
  tags                   = local.tags
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
resource "aws_ecr_repository" "api" {
  name                 = "gravityboots-api"
  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }
  encryption_configuration {
    encryption_type = "KMS"
  }
  tags = local.tags
}

resource "aws_ecr_repository" "mockaroo" {
  name                 = "gravityboots-mockaroo"
  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }
  encryption_configuration {
    encryption_type = "KMS"
  }
  tags = local.tags
}

resource "aws_ecr_repository" "frontend" {
  name                 = "gravityboots-frontend"
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
resource "aws_cloudwatch_log_group" "ecs" {
  name = "/aws/ecs"
}

resource "aws_ecs_cluster" "api" {
  name = "cluster-gb-api"

  configuration {
    execute_command_configuration {
      logging = "OVERRIDE"
      log_configuration {
        cloud_watch_log_group_name = aws_cloudwatch_log_group.ecs.name
      }
    }
  }
  setting {
    name  = "containerInsights"
    value = "enabled"
  }
  tags = local.tags
}
resource "aws_ecs_cluster" "mockaroo" {
  name = "cluster-gb-mockaroo"

  setting {
    name  = "containerInsights"
    value = "enabled"
  }
  configuration {
    execute_command_configuration {
      logging = "OVERRIDE"
      log_configuration {
        cloud_watch_log_group_name = aws_cloudwatch_log_group.ecs.name
      }
    }
  }
  tags = local.tags
}

resource "aws_ecs_cluster" "frontend" {
  name = "cluster-gb-frontend"

  setting {
    name  = "containerInsights"
    value = "enabled"
  }
  configuration {
    execute_command_configuration {
      logging = "OVERRIDE"
      log_configuration {
        cloud_watch_log_group_name = aws_cloudwatch_log_group.ecs.name
      }
    }
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

resource "aws_ecs_task_definition" "api" {
  family                   = "taskdef-gbapi-${var.environment}"
  requires_compatibilities = ["FARGATE"]
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }
  cpu                   = 1024
  memory                = 2048
  network_mode          = "awsvpc"
  execution_role_arn    = aws_iam_role.fargate.arn
  container_definitions = <<TASK_DEFINITION
  [
    {
      "name": "dotnet-api",
      "image": "${aws_ecr_repository.api.repository_url}:latest",
      "cpu": 10,
      "memory": 512,
      "environment": [
        {
          "name": "ConnectionStrings__Api",
          "value": "Host=${aws_db_instance.db.address};Port=${aws_db_instance.db.port};Database=GravityBoots;Username=${var.db_username};Password=${var.db_password}"
        }
      ],
      "portMappings": [
        {
          "containerPort": 80,
          "hostPort": 80
        },
        {
          "containerPort": 443,
          "hostPort": 443
        }
      ],
      "logConfiguration": {
        "logDriver": "awslogs",
        "options": {
          "awslogs-group": "/aws/ecs/dotnet-api-container",
          "awslogs-region": "us-east-2",
          "awslogs-create-group": "true",
          "awslogs-stream-prefix": "dotnet-api"
        }
      }
    }
  ]
  TASK_DEFINITION
}

resource "aws_ecs_service" "api" {
  name            = "service-gb-api-${var.environment}"
  cluster         = aws_ecs_cluster.api.id
  task_definition = aws_ecs_task_definition.api.id
  desired_count   = 2
  capacity_provider_strategy {
    base              = 1
    capacity_provider = "FARGATE"
    weight            = 100
  }
  network_configuration {
    subnets = [aws_subnet.private_00.id, aws_subnet.private_10.id]
    security_groups = [aws_security_group.ecs.id]
  }
  load_balancer {
    target_group_arn = aws_lb_target_group.api_01.arn
    container_name   = "dotnet-api"
    container_port   = 80
  }

  tags = local.tags
}

resource "aws_ecs_cluster_capacity_providers" "mockaroo" {
  cluster_name       = aws_ecs_cluster.mockaroo.name
  capacity_providers = ["FARGATE"]
  default_capacity_provider_strategy {
    base              = 1
    weight            = 100
    capacity_provider = "FARGATE"
  }
}

resource "aws_ecs_task_definition" "mockaroo" {
  family                   = "taskdef-gbmock-${var.environment}"
  requires_compatibilities = ["FARGATE"]
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }
  cpu                   = 1024
  memory                = 2048
  network_mode          = "awsvpc"
  execution_role_arn    = aws_iam_role.fargate.arn
  container_definitions = <<TASK_DEFINITION
   [
    {
      "name": "mockaroo",
      "image": "${aws_ecr_repository.mockaroo.repository_url}:latest",
      "cpu": 10,
      "memory": 512,
      "portMappings": [
        {
          "containerPort": 80,
          "hostPort": 80
        },
        {
          "containerPort": 443,
          "hostPort": 443
        }
      ],
      "environment": [
        {
          "name": "MockarooApi__ApiKey",
          "value": "${var.mockaroo_api_key}"
        }
      ],
      "logConfiguration": {
        "logDriver": "awslogs",
        "options": {
          "awslogs-group": "/aws/ecs/mockaroo-container",
          "awslogs-region": "us-east-2",
          "awslogs-create-group": "true",
          "awslogs-stream-prefix": "mockaroo"
        }
      }
    }
  ]
  TASK_DEFINITION
}

resource "aws_ecs_service" "mockaroo" {
  name            = "service-gb-mockaroo-${var.environment}"
  cluster         = aws_ecs_cluster.mockaroo.id
  task_definition = aws_ecs_task_definition.mockaroo.id
  desired_count   = 2
  capacity_provider_strategy {
    base              = 1
    capacity_provider = "FARGATE"
    weight            = 100
  }
  network_configuration {
    subnets          = [aws_subnet.mockaroo_private_1.id, aws_subnet.mockaroo_private_2.id]
    assign_public_ip = true
  }
  load_balancer {
    target_group_arn = aws_lb_target_group.mockaroo.arn
    container_name = "mockaroo"
    container_port = 80
  }
  tags = local.tags
}

resource "aws_ecs_cluster_capacity_providers" "frontend" {
  cluster_name       = aws_ecs_cluster.frontend.name
  capacity_providers = ["FARGATE"]
  default_capacity_provider_strategy {
    base              = 1
    weight            = 100
    capacity_provider = "FARGATE"
  }
}
resource "aws_ecs_task_definition" "frontend" {
  family                   = "taskdef-gbfrontend-${var.environment}"
  requires_compatibilities = ["FARGATE"]
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }
  cpu                   = 1024
  memory                = 2048
  network_mode          = "awsvpc"
  execution_role_arn    = aws_iam_role.fargate.arn
  container_definitions = <<TASK_DEFINITION
   [
    {
      "name": "frontend",
      "image": "${aws_ecr_repository.frontend.repository_url}:latest",
      "cpu": 10,
      "memory": 512,
      "portMappings": [
        {
          "containerPort": 3000,
          "hostPort": 3000
        }
      ],
      "logConfiguration": {
        "logDriver": "awslogs",
        "options": {
          "awslogs-group": "/aws/ecs/frontend-container",
          "awslogs-region": "us-east-2",
          "awslogs-create-group": "true",
          "awslogs-stream-prefix": "frontend"
        }
      }
    }
  ]
  TASK_DEFINITION
}

resource "aws_ecs_service" "frontend" {
  name            = "service-gb-frontend-${var.environment}"
  cluster         = aws_ecs_cluster.frontend.id
  task_definition = aws_ecs_task_definition.frontend.id
  desired_count   = 2
  capacity_provider_strategy {
    base              = 1
    capacity_provider = "FARGATE"
    weight            = 100
  }
  network_configuration {
    subnets          = [aws_subnet.private_00.id, aws_subnet.private_10.id]
    security_groups = [aws_security_group.ecs.id]
  }
  load_balancer {
    target_group_arn = aws_lb_target_group.frontend_01.arn
    container_name   = "frontend"
    container_port   = 3000
  }
  tags = local.tags
}

# Frontend
resource "aws_cloudfront_origin_access_identity" "oai" {}
resource "aws_cloudfront_distribution" "s3_distribution" {
  origin {
    domain_name = aws_lb.main.dns_name
    origin_id   = local.cloudfront_origin_id
    custom_origin_config {
      http_port = 3000
      https_port = 3001
      origin_protocol_policy = "http-only"
      origin_ssl_protocols = ["TLSv1.2"]
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
    target_origin_id = local.cloudfront_origin_id

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
    target_origin_id = local.cloudfront_origin_id

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
    target_origin_id = local.cloudfront_origin_id

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

