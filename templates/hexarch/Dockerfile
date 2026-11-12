# syntax=docker/dockerfile:1.27@sha256:4edf897a3ffa55b89f906fc8cc78afdb3f1834cc9c7083565e611a8a7d5fe99e
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:0294f285d062afacc8b5f6d7febb0cd4ee879320e5585fa15f2f4ebf1feb9ba7 AS build
WORKDIR /source

# Improves restore speed by skipping XML docs.
ENV NUGET_XMLDOC_MODE=skip

COPY HexagonalArchitectureTemplateDocker.slnx .
COPY nuget.config .

# Copy project files first to maximize Docker layer caching for restore.
COPY App/src/App.csproj ./App/src/
COPY Domain/Ports/App/Domain.Ports.App.csproj ./Domain/Ports/App/
COPY Domain/Data/Domain.Data.csproj ./Domain/Data/
COPY Domain/Logic/src/Domain.Logic.csproj ./Domain/Logic/src/
COPY Domain/Ports/Infrastructure/Domain.Ports.Infrastructure.csproj ./Domain/Ports/Infrastructure/
COPY Infrastructure/src/Infrastructure.csproj ./Infrastructure/src/

RUN dotnet restore ./App/src/App.csproj

# Copy the remaining source code.
COPY Domain/ ./Domain/
COPY App/ ./App/
COPY Infrastructure/ ./Infrastructure/

RUN dotnet publish ./App/src/App.csproj -c Release -f net10.0 -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:21c3f9798416cba4c0576a947c286231cb71ea2884896cb968253f5c9668f930
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "App.dll"]
