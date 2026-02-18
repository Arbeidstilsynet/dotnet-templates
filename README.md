<!-- Provide an overview of what your template package does and how to get started.
Consider previewing the README before uploading (https://learn.microsoft.com/en-us/nuget/nuget-org/package-readme-on-nuget-org#preview-your-readme). -->

# Dotnet templates

This is a mono repo for dotnet templates, for internal use in Arbeidstilsynet.

The package `Arbeidstilsynet.Templates` is published to our [organization in nuget.org](https://www.nuget.org/profiles/Arbeidstilsynet)

## 🏃‍♂️ Getting Started

Install our template package by running the following command.

```console
dotnet new install Arbeidstilsynet.Templates
```

Once installed, you can create new templates. We currently support `hexarch` and `common-package`.

### Hexarch Template

Our opinionated template for all applications which are going to expose an API. By adapting to this template, you will get:

- Well-tested project structure, which derives from the hexagonal architecture pattern
- Observability
- Structured Logging
- Containerized setup, with a local test setup

To create a new project, run:

```console
dotnet new hexarch -n "YourAppName"
```

### Common Package (felles-pakke)

If you want to create a common package, we are providing a template which makes it easy to maintain the package and release new versions. Typical examples for a common package are:

- Client for an external endpoint (e.g. Altinn/Brreg)
- Outsource an infrastructure adapter from your hexagonal application, which can be useful for several hexagonal applications
- Useful extension methods for cross cutting concerns (e.g. Altinn)

Before creating a new package, checkout our package repository [dotnet-common](https://github.com/Arbeidstilsynet/dotnet-common) and see if there is a similar package available.

If not, you can add a new package by running the following command in the `dotnet-common` repository:

```console
dotnet new common-package -n "YourPackageName"
```

## 🤝 Contribute

This template is intended to be maintained by all developers and should serve as common base for future (or migrated) dotnet projects.
Simply propose your changes in a PullRequest.

## 🚀 Deploy

To publish a new version of the template, create a PR and remember to increase the version number in `Arbeidstilsynet.Templates`.

To use the latest template version, run:

```console
dotnet new update
```
