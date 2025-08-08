# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine@sha256:4042a687b9c50bfbd4defd6c28c455e5883aaaa5d086cb2d6e8dd39027bde58b AS build
WORKDIR /source

COPY HexagonalArchitectureTemplateDocker.sln .
COPY nuget.config .
COPY Domain/. ./Domain/
COPY API/. ./API/
COPY Infrastructure/. ./Infrastructure/
COPY ArchUnit.Tests/. ./ArchUnit.Tests/

RUN dotnet publish -c release

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine@sha256:d1f21f18d9a720274eef9fe68fe3cbdd2301e42628447a2ef2632d0006881628
WORKDIR /build
COPY --from=build /source/API/API.Adapters/bin/Release/net8.0 ./
ENTRYPOINT ["dotnet", "API.Adapters.dll"]
