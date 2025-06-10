# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine@sha256:cec8f5d4537ff29112274379401142fa73d97fcc9f174dc1c623c29dcaef24c1 AS build
WORKDIR /source

COPY HexagonalArchitectureTemplateDocker.sln .
COPY nuget.config .
COPY Domain/. ./Domain/
COPY API/. ./API/
COPY Infrastructure/. ./Infrastructure/
COPY ArchUnit.Tests/. ./ArchUnit.Tests/

RUN dotnet publish -c release

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine@sha256:ea72850bd81ba5c95ba88641a4fa315471bef9e3d1cd7e26c2594faff56e3a36
WORKDIR /build
COPY --from=build /source/API/API.Adapters/bin/Release/net8.0 ./
ENTRYPOINT ["dotnet", "API.Adapters.dll"]
