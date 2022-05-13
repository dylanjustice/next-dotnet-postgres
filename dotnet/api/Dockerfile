FROM bitnami/dotnet-sdk:5.0.407 AS build
WORKDIR /api
COPY . .
RUN dotnet restore
RUN dotnet publish --configuration release -o release

FROM bitnami/aspnet-core:5.0.16
WORKDIR /api
COPY --from=build /api/release .
ENTRYPOINT [ "dotnet", "DylanJustice.Demo.Presentation.Web.dll" ]

