FROM mcr.microsoft.com/dotnet/aspnet:6.0.5 as base
WORKDIR /api

FROM mcr.microsoft.com/dotnet/sdk:6.0.300 as build
WORKDIR /api

COPY . .
RUN dotnet restore

FROM build as publish
COPY . .
RUN dotnet publish --configuration release

FROM base as final
WORKDIR /api
COPY --from=publish /api/Presentation/Web/bin/Release/net6.0 .
EXPOSE 80
EXPOSE 443
ENTRYPOINT [ "dotnet", "Web.dll" ]

