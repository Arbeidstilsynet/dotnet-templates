# hexarch v3 migration

To be executed in the given order

## Renamings (code)

* search and replace all usages of ``API.Ports`` to `Domain.Ports.App`
* search and replace all usages of ``\\.API\\.Ports`` to ``\\.Domain\\.Ports\\.App``
* rename ``API.Ports.csproj`` to ``Domain.Ports.App.csproj``
* search and replace all usages of ``Infrastructure.Ports`` to `Domain.Ports.Infrastructure`
* search and replace all usages of ``\\.Infrastructure\\.Ports`` to ``\\.Domain\\.Ports\\.Infrastructure``
* rename ``Infrastructure.Ports.csproj`` to ``Domain.Ports.Infrastructure.csproj``
* search and replace all usages of ``API.Adapters`` to `App`
* search and replace all usages of ``\\.API\\.Adapters`` to ``\\.App``
* rename ``API.Adapters.csproj`` to ``App.csproj``
* rename ``API.Adapters.Test.csproj`` to ``App.Test.csproj``
* search and replace all usages of ``Infrastructure.Adapters`` to `Infrastructure`
* search and replace all usages of ``\\.Infrastructure\\.Adapters`` to ``\\.Infrastructure``
* rename ``Infrastructure.Adapters.csproj`` to ``Infrastructure.csproj``
* rename ``Infrastructure.Adapters.Test.csproj`` to ``Infrastructure.Test.csproj``

## Move files / rename directories

* Rename directory `API` to `App`
* Rename directory `API.Adapters` to `src`
* Rename directory `API.Adapters.Test` to `test`
* Rename directory `Domain.Data` to `Data`
* Under directory  `Domain`, create new sub directory `Logic`
* Rename directory `Domain.Logic` to `src` and make it a subdirectory of `Logic`
* Rename directory `Domain.Logic.Test` to `test` and make it a subdirectory of `Logic`
* Under directory  `Domain`, create new sub directory `Ports`
* Move `API.Ports` to the new directory `Ports`, and rename it afterwards to `App`
* Move `Infrastructure.Ports` to the new directory `Ports`, and rename it afterwards to `Infrastructure`
* Rename directory `Infrastructure.Adapters` to `src`
* Rename directory `Infrastructure.Test` to `test`

## Update project references and solution files

* remove all project references in all .csproj files
* delete the solution file `<yourappname>.sln`
* create a new solution file via `dotnet new sln -n <yourappname>`
* update the solution file to look like the following:

 ```xml
 <Solution>
  <Folder Name="/App/">
    <Project Path="App/src/App.csproj" />
    <Project Path="App/test/App.Test.csproj" />
  </Folder>
  <Folder Name="/Domain/">
    <Project Path="Domain/Data/Domain.Data.csproj" />
    <Project Path="Domain/Logic/src/Domain.Logic.csproj" />
    <Project Path="Domain/Logic/test/Domain.Logic.Test.csproj" />
    <Project Path="Domain/Ports/App/Domain.Ports.App.csproj" />
    <Project Path="Domain/Ports/Infrastructure/Domain.Ports.Infrastructure.csproj" />
  </Folder>
  <Folder Name="/Infrastructure/">
    <Project Path="Infrastructure/src/Infrastructure.csproj" />
    <Project Path="Infrastructure/test/Infrastructure.Test.csproj" />
  </Folder>
  <Project Path="ArchUnit.Tests/ArchUnit.Tests.csproj" />
</Solution>
 ```

* finally, update all .csproj files to include the new, correct project references:

** App.csproj, add

```xml
<ItemGroup>
    <ProjectReference Include="../../Domain/Logic/src/Domain.Logic.csproj" />
    <ProjectReference Include="../../Domain/Ports/App/Domain.Ports.App.csproj" />
    <ProjectReference Include="../../Infrastructure/src/Infrastructure.csproj" />
</ItemGroup>
```

** App.Test.csproj, add

```xml
<ItemGroup>
    <ProjectReference Include="../../Infrastructure/test/Infrastructure.Test.csproj" />
    <ProjectReference Include="../src/App.csproj" />
</ItemGroup>
```

** ArchUnit.Tests.csproj, add

```xml
<ItemGroup>
    <ProjectReference Include="../App/src/App.csproj" />
</ItemGroup>
```

** Domain.Logic.csproj, add

```xml
<ItemGroup>
    <ProjectReference Include="../../Ports/App/Domain.Ports.App.csproj" />
    <ProjectReference Include="../../Ports/Infrastructure/Domain.Ports.Infrastructure.csproj" />
</ItemGroup>
```

** Domain.Logic.Test.csproj, add

```xml
<ItemGroup>
    <ProjectReference Include="../src/Domain.Logic.csproj" />
</ItemGroup>
```

** Domain.Ports.App.csproj, add

```xml
<ItemGroup>
    <ProjectReference Include="../../Data/Domain.Data.csproj" />
</ItemGroup>
```

** Domain.Ports.Infrastructure.csproj, add

```xml
<ItemGroup>
    <ProjectReference Include="../../Data/Domain.Data.csproj" />
</ItemGroup>
```

**Infrastructure.csproj, add

```xml
<ItemGroup>
    <ProjectReference Include="../../Domain/Ports/Infrastructure/Domain.Ports.Infrastructure.csproj" />
</ItemGroup>
```

** Infrastructure.Test.csproj, add

```xml
<ItemGroup>
    <ProjectReference Include="../src/Infrastructure.csproj" />
</ItemGroup>
```

## Build and test

* run `dotnet build` to see if everything compiles
* run `dotnet test` to validate integrity

## Update remaining files

* update the dockerfile with the following

```dockerfile
# syntax=docker/dockerfile:1.22@sha256:4a43a54dd1fedceb30ba47e76cfcf2b47304f4161c0caeac2db1c61804ea3c91
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:9b4b31da5246f575086b1901e9871b189ae2a80eb42fe9234e9d000b51febd4b AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:258b939d6d684ff05ad7ea16782b4bee55260de4acc6f99bec897fd11de7640c
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "App.dll"]
```

* update the following readme sections:

```terminal
dotnet ef migrations add InitDb --startup-project App/src --project Infrastructure/src -o Db/Migrations
```

```terminal
dotnet run --project App/src
```

    ## 🧱 Project Structure

    <!-- prettier-ignore -->
    ```md
    .
    ├── App
    │   ├── src
    │   └── test
    ├── ArchUnit.Tests
    ├── Domain
    │   ├── Data
    │   ├── Logic
    │   │   ├── src
    │   │   └── test
    │   └── Ports
    │       ├── App
    │       └── Infrastructure
    └── Infrastructure
        ├── src
        └── test
    ```

    * ArchUnit.Tests
    * Important tests to gurantee the below structure keeps maintained
    * Domain
    * **Domain.Logic** _implements_ Domain.Ports.App and _uses_ Domain.Ports.Infrastructure
    * **Domain.Logic.Test** contains tests that validate the domain logic
    * **Domain.Data** contains Classes/DTOs which can be shared across layers
    * Infrastructure (Outgoing: infrastructure the application talks with)
    * **Infrastructure** _implements_ Domain.Ports.Infrastructure (i.e. Adapters)
    * **Infrastructure.Test** contains tests that validate the Infrastructure
    * App (Incoming: adapters to make it possible to talk with the application)
    * **App** _uses_ Domain.Ports.App
    * Responsible for injecting the necessary dependencies and exposing API endpoints
    * **App.Test** contains typically integration tests

    > Domain.Logic and Infrastructure implementations are internal, and only exposed through DependencyInjection extensions.

## finally

* run `docker compose up -d --build` . If this works, we are all good :)
