# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine@sha256:4f06e168669408b87e6e671a6fc6b78bf661f08ab8fafddb0e4615a2e5839f06 AS build
WORKDIR /source

COPY HexagonalArchitectureTemplateDocker.sln .
COPY nuget.config .
COPY Domain/. ./Domain/
COPY API/. ./API/
COPY Infrastructure/. ./Infrastructure/
COPY ArchUnit.Tests/. ./ArchUnit.Tests/

RUN dotnet publish -c release

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine@sha256:53e4fe31c30a3aea383291d362c84ceb752d121bfab0e4ca1ba73a65945106e9
WORKDIR /build
COPY --from=build /source/API/API.Adapters/bin/Release/net8.0 ./
ENTRYPOINT ["dotnet", "API.Adapters.dll"]
