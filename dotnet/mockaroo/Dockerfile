FROM mcr.microsoft.com/dotnet/aspnet:6.0.5 as base
WORKDIR /api

FROM mcr.microsoft.com/dotnet/sdk:6.0.300 as build
WORKDIR /api

COPY . .
RUN dotnet restore

FROM build as publish
COPY . .
RUN dotnet publish --configuration release -o release

FROM base as final
WORKDIR /api
COPY --from=publish /api/release .
ENTRYPOINT [ "dotnet", "Mockaroo.Presentation.Web.dll" ]

