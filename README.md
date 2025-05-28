<!-- Provide an overview of what your template package does and how to get started.
Consider previewing the README before uploading (https://learn.microsoft.com/en-us/nuget/nuget-org/package-readme-on-nuget-org#preview-your-readme). -->

# 🔧 Prerequisites

Per today, we published all our templates in our Public `AT.Public.NuGet` Nuget feed which is hosted on Azure. Even though it is public, you need to have the feed in your `nuget.config` to be able to execute the provided cli commands.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add
      key="AT.Public.NuGet"
      value="https://pkgs.dev.azure.com/Atil-utvikling/Public/_packaging/AT.Public.NuGet/nuget/v3/index.json"
      protocolVersion="3"
    />
  </packageSources>
</configuration>
```

# 🏃‍♂️ Getting Started

Install our template package by running the following command. If you get an exeption, make sure you updated the `nuget.config` as described in [Prerequisites](#-prerequisites).

```console
dotnet new install Arbeidstilsynet.Templates
```

Once installed, you can create new templates. We currently support `hexarch`, `common-package`, `extension-package`.

## Hexarch Template

Our opinionated template for all applications which are going to expose an API. By adapting to this template, you will get:

- Well-tested project structure, which follows the hexagonal architecture pattern
- Observability
- Structured Logging
- Containerized setup, with a local test setup

To create a new project, run:

```console
dotnet new hexarch -n "YourAppName"
```

## Common Package (felles-pakke)

If you want to create a common package, we are providing a template which makes it easy to maintain the package and release new versions. Typical examples for a common package are:

- Client for an external endpoint (e.g. Altinn/Brreg)
- Outsource an infrastructure adapter from your hexagonal application, which can be useful for several hexagonal applications

Before creating a new package, checkout our package repository [dotnet-common](https://github.com/Arbeidstilsynet/dotnet-common) and see if there is a similar package available.

If not, you can add a new package by running the following command in the `dotnet-common` repository:

```console
dotnet new common-package -n "YourPackageName"
```

## Extension Package

If you want to create a common package, but you do not have to hide any internal logic (this means that everything in this package is publicly available), you can create an extension package. A typical use case for this is to provide `Startup Extensions`, which can be reused by any dotnet project at startup.

Before creating a new package, checkout our package repository [dotnet-common](https://github.com/Arbeidstilsynet/dotnet-common) and see if there is a similar package available.

If not, you can add a new package by running the following command in the `dotnet-common` repository:

```console
dotnet new extension-package -n "YourExtensionName"
```

# 🤝 Contribute

This template is intended to be maintained by all developers and should serve as common base for future (or migrated) dotnet projects.
Simply propose your changes in a PullRequest.

# 🚀 Deploy

To publish a new version of the template, create a PR and remember to increase the version number in `Arbeidstilsynet.Templates`.

To use the latest template version, run:

```console
dotnet new update
```
