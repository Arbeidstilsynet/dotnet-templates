# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine@sha256:cd6c81064940585fc59a2c7b169268dff2e015565ecde961f7c6e2a18434a210 AS build
WORKDIR /source

COPY HexagonalArchitectureTemplateDocker.sln .
COPY nuget.config .
COPY Domain/. ./Domain/
COPY API/. ./API/
COPY Infrastructure/. ./Infrastructure/
COPY ArchUnit.Tests/. ./ArchUnit.Tests/

RUN dotnet publish -c release

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine@sha256:8f4ced906b6610a1835696f59d80e992008957d38317093258e0879f736d28ed
WORKDIR /build
COPY --from=build /source/API/API.Adapters/bin/Release/net8.0 ./
ENTRYPOINT ["dotnet", "API.Adapters.dll"]
