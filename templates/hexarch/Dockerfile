# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine@sha256:3f0fb43bb36dbce018486be9ba00a88121b838ac8ea47f8e3ddc9579a7391dda AS build
WORKDIR /source

COPY HexagonalArchitectureTemplateDocker.sln .
COPY nuget.config .
COPY Domain/. ./Domain/
COPY API/. ./API/
COPY Infrastructure/. ./Infrastructure/
COPY ArchUnit.Tests/. ./ArchUnit.Tests/

RUN dotnet publish -c release

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine@sha256:cb69be896f82e0d73f513c128ece501c7c1f1809c49415a69dc096e013d5314a
WORKDIR /build
COPY --from=build /source/API/API.Adapters/bin/Release/net8.0 ./
ENTRYPOINT ["dotnet", "API.Adapters.dll"]
