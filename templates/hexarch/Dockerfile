# syntax=docker/dockerfile:1.20@sha256:26147acbda4f14c5add9946e2fd2ed543fc402884fd75146bd342a7f6271dc1d
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:0fba99926e4f12405c78f37941312f00c1aadb178bd63616f5d96fc2af5a26a9 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:1be14b20e4ec3dad26aa4932ae16c2193cc990c5499239492e9b697f0cba4e04
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "API.Adapters.dll"]
