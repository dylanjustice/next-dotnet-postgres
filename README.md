# Cloud Native Three Layer App

## Application Stack

- .Net 6
- NextJS 13
- PostgresSQL

## AWS Architectural Plan

- .NET 5 API deployed to ECS with Fargate
- .NET 6 Peer (Mock 3rd party) API deployed to ECS with Fargate
- NextJS frontend deployed to ECS with Fargate delivered through Cloudfront CDN
- PostgreSQL deployed to AWS RDS
