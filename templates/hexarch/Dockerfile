# syntax=docker/dockerfile:1.22@sha256:4a43a54dd1fedceb30ba47e76cfcf2b47304f4161c0caeac2db1c61804ea3c91
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:a0116e63beedf9197c3d491eb224aea9ae7d1692079eda9eebe2809f06d580e3 AS build
WORKDIR /source

# Improves restore speed by skipping XML docs.
ENV NUGET_XMLDOC_MODE=skip

COPY HexagonalArchitectureTemplateDocker.sln .
COPY nuget.config .

# Copy project files first to maximize Docker layer caching for restore.
COPY API/API.Adapters/API.Adapters.csproj ./API/API.Adapters/
COPY API/API.Ports/API.Ports.csproj ./API/API.Ports/
COPY Domain/Domain.Data/Domain.Data.csproj ./Domain/Domain.Data/
COPY Domain/Domain.Logic/Domain.Logic.csproj ./Domain/Domain.Logic/
COPY Infrastructure/Infrastructure.Adapters/Infrastructure.Adapters.csproj ./Infrastructure/Infrastructure.Adapters/
COPY Infrastructure/Infrastructure.Ports/Infrastructure.Ports.csproj ./Infrastructure/Infrastructure.Ports/

RUN dotnet restore ./API/API.Adapters/API.Adapters.csproj

# Copy the remaining source code.
COPY Domain/ ./Domain/
COPY API/ ./API/
COPY Infrastructure/ ./Infrastructure/

RUN dotnet publish ./API/API.Adapters/API.Adapters.csproj -c Release -f net10.0 -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:258b939d6d684ff05ad7ea16782b4bee55260de4acc6f99bec897fd11de7640c
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "API.Adapters.dll"]
