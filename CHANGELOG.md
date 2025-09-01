# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 1.2.9

### Changed

- changed(deps): update all minor dependencies

## 1.2.8

### Added

- added(hexarch): static `Tracer` classes to create custom spans which can be helpful for structuring traces with opentelemetry

## 1.2.7

### Changed

- changed(hexarch): small improvements/bugfixes which occured while using the template for new projects

## 1.2.6

### Changed

- changed(deps): update all minor dependencies

## 1.2.5

### Changed

- changed(hexarch): update nuget config and fix bug in archunit test

## 1.2.4

### Changed

- changed(common-package): Updated ArchUnit tests to allow public abstract classes

## 1.2.3

### Changed

- changed(common-package): Added root namespace to common-package test project
- changed(common-package): Removed duplicate lines from .gitignore
- changed(hexarch): Removed duplicate lines from .gitignore

## 1.2.2

### Changed

- changed(hexarch): Domain section is no longer required in appsettings.json for API.Adapters

## 1.2.1

### Fixed

- fixed(hexarch): containerlogs when running in docker are now available again (this also makes it possible to see otel logs in grafana again)

## 1.2.0

### Changed

- changed(hexarch): Branched AppSettings into API, Domain and Infrastructure
- changed(hexarch): Renamed `.AddInfrastructureServices(..)` to `.AddInfrastructure(..)`

## 1.1.0

### Added

- hexarch: add support for CORS

### Changed

- hexarch: changed default port for Grafana to 4000 to avoid conflicts

## 1.0.0

Consolidated extensions-package into common-package

### Added

- added(common-package): Added .gitignore

### Changed

- changed(common-package): Single-project structure and new ArchUnit tests. A bit more flexible, which allows publishing any combination of ports/adapters, extensions or just model classes
- changed(hexarch): Updated .gitignore to handle Rider metadata

### Removed

- removed(extension-package): Obsoleted by common-package

## 0.0.11

### Changed

- changed(extension-package): update AT.Common.AspNetCore.Extensions from v0.0.2 to v0.0.3

## 0.0.10

### Changed

- chore(hexarch): Centralize Docker Compose configuration using the include directive and split database and monitoring services into dedicated snippets. Also replaced the monitoring stack in docker compose with a bundled solution.
- chore(hexarch): Use default port 8080 instead of 9008 in dockerized environment.

### Fixed

- fix(common-package): Typo in ArchUnit tests

## 0.0.9

### Fixed

- changed(extension-package): update AT.Common.AspNetCore.Extensions from v0.0.1 to v0.0.2

## 0.0.8

### Changed

- chore(extension-package): Updated the template to use the AT.Common.AspNetCore.Extensions package in order to reduce boilerplate.

## 0.0.7

### Changed

- chore(extension-package): Removed an unnecessary ArchUnit test.

## 0.0.6

### Changed

- chore(hexarch): update swashbuckle from v7 to v8 (major)

## 0.0.5

### Changed

- chore(hexarch): update scalar from v1 to v2 (major)

## 0.0.4

### Changed

- chore: Dependency update (all non minor)

## 0.0.3

### Changed

- extension-package: Updated template ReadMe

## 0.0.2

### Changed

- CI/CD: fix automatic github release when succesfully published.

## 0.0.1

### Added

- Single template project which contains all templates which are available today (`hexarch`,`common-package`,`extensions-package`).
- CI/CD: Workflow which supports building and publishing new versions of this project.

### Changed

- Moved existing templates from azure devops to this repository. Tagged the legacy repositories with a deprecation remark.
