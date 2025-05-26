<!-- Provide an overview of what your template package does and how to get started.
Consider previewing the README before uploading (https://learn.microsoft.com/en-us/nuget/nuget-org/package-readme-on-nuget-org#preview-your-readme). -->


# 🏃‍♂️ Getting Started

```console
dotnet new install Arbeidstilsynet.Templates
```

Once installed, you can create new templates. We currently support `hexarch`, `common-package`, `extension-package`.
TBD: when to use which

```console
dotnet new hexarch -n "YourAppName"
```

```console
dotnet new common-package -n "YourPackageName"
```

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