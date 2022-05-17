FROM mcr.microsoft.com/dotnet/aspnet:5.0.16 as base
WORKDIR /api

FROM mcr.microsoft.com/dotnet/sdk:5.0.407 as build
WORKDIR /api

COPY . .
RUN dotnet restore

FROM build as publish
COPY . .
RUN dotnet publish --configuration release -o release

FROM base as final
WORKDIR /api
COPY --from=publish /api/release .

ENTRYPOINT [ "dotnet", "DylanJustice.Demo.Presentation.Web.dll" ]

