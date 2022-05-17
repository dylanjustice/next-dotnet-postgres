FROM bitnami/dotnet-sdk:6.0.300 AS build
WORKDIR /api
COPY . .
RUN dotnet restore
RUN dotnet publish --configuration release -o release

FROM bitnami/aspnet-core:6.0.5
WORKDIR /api
COPY --from=build /api/release .
ENTRYPOINT [ "dotnet", "Mockaroo.Presentation.Web.dll" ]

