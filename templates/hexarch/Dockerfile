# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:0fba99926e4f12405c78f37941312f00c1aadb178bd63616f5d96fc2af5a26a9 AS build
WORKDIR /source

COPY HexagonalArchitectureTemplateDocker.sln .
COPY nuget.config .
COPY Domain/. ./Domain/
COPY API/. ./API/
COPY Infrastructure/. ./Infrastructure/
COPY ArchUnit.Tests/. ./ArchUnit.Tests/

RUN dotnet publish -c release

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:1be14b20e4ec3dad26aa4932ae16c2193cc990c5499239492e9b697f0cba4e04
WORKDIR /build
COPY --from=build /source/API/API.Adapters/bin/Release/net10.0 ./
ENTRYPOINT ["dotnet", "API.Adapters.dll"]
