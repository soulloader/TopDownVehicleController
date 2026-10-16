# Changelog

All notable changes to **Top-Down Vehicle Controller – Arcade System** will be documented in this file.

This project follows the principles of **Keep a Changelog** and **Semantic Versioning (SemVer)**.

---

## [Unreleased]

### Added

#### Core and Architecture

- Implemented core lifecycle interfaces.
- Implemented vehicle system interfaces.
- Implemented VehicleContext.
- Implemented VehicleController orchestration.
- Established separation between system orchestration and gameplay logic.
- Added individual Architecture Decision Records.
- Documented the VehicleController responsibility principle in ADR-015.

#### Vehicle Systems

- Implemented the initial Vehicle Movement system.
- Implemented Vehicle Steering.
- Implemented Vehicle Suspension.
- Implemented Ground Detection.
- Implemented Obstacle Detection.
- Implemented Surface Detection.
- Implemented initial vehicle audio systems.
- Implemented initial vehicle effects.
- Implemented vehicle configuration infrastructure.

#### Input

- Integrated the Unity Input System.
- Established the Input System as a required framework dependency.

#### Demonstration

- Added an initial demonstration scene.
- Added a functional Tank prefab.
- Added vehicle configuration examples.
- Performed initial Unity package import testing.

### Changed

#### Unity Development Environment

- Migrated the primary development environment from Unity 2021.3.45f2 LTS to Unity 2022.3.62f3 LTS.
- Updated project settings and package references for Unity 2022.
- Established Unity 2022.3.62f3 LTS as the official development version.
- Reclassified Unity 2021.3.45f2 LTS as a minimum supported version candidate pending validation.
- Removed unverified claims of Unity 6 LTS compatibility.

#### Dependencies

- Clarified that the Unity Input System is required.
- Clarified that Cinemachine is optional and is not a core framework dependency.
- Documented that the current demonstration uses the Universal Render Pipeline (URP).

#### Documentation

- Updated README.md.
- Updated Architecture.md.
- Updated ADR.md.
- Updated CodingStandards.md.
- Updated ROADMAP.md.
- Updated Philosophy.md.
- Updated LICENSE.md.
- Updated Unity compatibility information.
- Clarified the distinction between implemented functionality and commercial release readiness.

### Fixed

- Corrected outdated Unity version references in project documentation.
- Corrected documentation describing Cinemachine as mandatory.
- Corrected unsupported compatibility claims.
- Aligned architectural documentation with the current modular framework structure.

### Validation

- Confirmed functional operation in the Unity 2022 development environment.
- Performed initial import testing in a clean Unity 2022 URP project with the Input System installed.
- Verified URP asset references during migration.

### Pending Validation

- Complete regression testing of vehicle systems.
- Validate Unity 2021.3.45f2 LTS compatibility.
- Evaluate Unity 6 LTS compatibility.
- Validate compatibility with additional render pipelines if support is intended.
- Complete clean-project installation testing.
- Review redistribution rights for demonstration resources.
- Finalize documentation and commercial packaging.

### Deprecated

- None.

### Removed

- No framework feature removal is recorded for this development period.

### Security

- No security changes recorded.

---

## [0.1.0] - 2026-07-07

### Added

- Initial project structure.
- Modular folder organization.
- Project documentation.
- README.md.
- CHANGELOG.md.
- CodingStandards.md.
- Architecture documentation.
- Roadmap documentation.
- CONTRIBUTING guide.
- License documentation.
- Modular framework philosophy.
- Initial module hierarchy.
- Vehicle module structure.
- Core module structure.
- Namespace conventions.
- Assembly Definition planning.
- VehicleController architecture definition.
- Independent system architecture.
- Development workflow definition.
- Single Responsibility Principle guidelines.
- Composition over Inheritance guidelines.
- XML documentation guidelines.
- Script organization standards.
- Folder organization standards.
- Naming conventions.
- Initial Asset Store project structure.
- Official Unity support policy.
- Mandatory dependency policy.
- Initial project roadmap.

### Changed

- None.

### Deprecated

- None.

### Removed

- None.

### Fixed

- None.

### Security

- None.

---

## Planned Development

The following items represent development objectives rather than completed release features.

### Core

- Core Utilities.
- Core Events.
- Core Extensions.
- Core Data improvements.
- Core Attributes.
- Public API review.
- Module independence validation.

### Vehicle

- Movement refinement.
- Suspension refinement.
- Detection improvements.
- Audio integration improvements.
- Effects integration improvements.
- Configuration usability improvements.
- Regression testing.

### Input

- Input abstraction improvements.
- Input provider validation.
- Integration documentation.

### Audio

- Surface audio refinement.
- Engine audio refinement.
- Audio configuration improvements.

### Effects

- Particle system refinement.
- Surface effects improvements.
- Visual feedback improvements.

### Documentation

- API documentation.
- User manual.
- Integration guide.
- Code examples.
- Commercial package documentation.

### Samples

- Final demonstration scene.
- Example vehicle prefab validation.
- Sample configuration validation.
- Redistribution rights review.

### Future Expansions

- Weapon Systems.
- Vehicle Equipment.
- Advanced Camera System.
- Additional Vehicle Types.
- Specialized Movement Modules.

These expansions are not required for the initial Base Vehicle Controller release.

---

# Versioning Policy

This project follows **Semantic Versioning (SemVer)**.

Version format:

**MAJOR.MINOR.PATCH**

## MAJOR

Breaking changes.

Examples:

- Incompatible public API redesign.
- Breaking architectural changes.
- Removal of previously supported functionality.

## MINOR

New backward-compatible functionality.

Examples:

- New gameplay systems.
- New modules.
- New configuration options.

## PATCH

Backward-compatible bug fixes and maintenance improvements.

Examples:

- Bug fixes.
- Documentation corrections.
- Internal optimizations.
- Minor editor improvements.

During pre-1.0 development, APIs and architecture may still evolve.

Public compatibility guarantees must be established before the first stable commercial release.

---

# Unity Compatibility Policy

## Official Development Version

**Unity 2022.3.62f3 LTS**

This version is used for primary development and current validation.

## Minimum Supported Version Candidate

**Unity 2021.3.45f2 LTS**

Compatibility remains pending validation.

This version must not be advertised as officially supported until testing is completed.

## Future Compatibility Candidate

**Unity 6 LTS**

Compatibility has not yet been validated.

## Earlier Versions

No official support commitment has been established for Unity versions earlier than the minimum supported version candidate.

---

# Dependencies

## Required

- Unity Input System.

## Optional

- Cinemachine.

Cinemachine is not a mandatory dependency of the core framework.

## Demonstration Render Pipeline

The current demonstration project uses the Universal Render Pipeline (URP).

Compatibility with other render pipelines remains unvalidated.

---

# Release Notes

## 0.1.0 — Initial Foundation

The initial project milestone established the foundation of the framework, including project organization, documentation standards, modular architecture, development conventions, and long-term scalability planning.

This historical milestone does not represent confirmation of a completed public Asset Store release.

## Unreleased — Active Development

The framework has progressed beyond the initial architecture phase.

Core vehicle systems have been implemented, and the primary development environment has migrated to Unity 2022.3.62f3 LTS.

Commercial release preparation remains in progress.

---

# End of Document

Top-Down Vehicle Controller – Arcade System  
© 2026 Inverso Studio. All rights reserved.