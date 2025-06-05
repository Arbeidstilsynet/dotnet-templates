# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 1.0.0

Consolidated extensions-package into common-package

### Changed

- changed(common-package): Single-project structure and new ArchUnit tests. A bit more flexible, which allows publishing any combination of ports/adapters, extensions or just model classes

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
