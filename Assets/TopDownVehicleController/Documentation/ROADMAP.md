=====================================================
                 Inverso Studio
=====================================================

Top-Down Vehicle Controller – Arcade System

Document:
ROADMAP

Version: 0.1.0

Author:
Edison Oliveira

Engine:
Unity 2022.3.62f3 LTS (Official Development Version)

Minimum Supported Version Candidate:
Unity 2021.3.45f2 LTS (Pending Validation)

Compatibility:
Unity 2022.3.62f3 LTS (Validated)
Unity 2021.3.45f2 LTS (Pending Validation)
Unity 6 LTS (Not Yet Validated)

Language:
C#

Architecture:
Modular Framework

=====================================================
---

# Development Roadmap

## Purpose

This document defines the development milestones, release objectives, and long-term vision of the Top-Down Vehicle Controller – Arcade System.

The roadmap distinguishes implemented functionality from completed validation and commercial release readiness.

The primary objective is to deliver a stable, modular, reusable arcade vehicle controller before expanding into additional gameplay systems.

**Finish before expanding.**

**---

# Development Status

The following status definitions apply throughout this document:

- **Completed:** Implemented and confirmed within the current development scope.

- **Implemented — Validation Pending:** Implementation exists, but release-level testing or verification remains.

- **In Progress:** Development or refinement is ongoing.

- **Planned:** Intended for a future development phase.

- **Proposed:** Under architectural or product consideration.

A completed implementation does not automatically indicate commercial release readiness.

**---

# Unity Compatibility

| Unity Version | Status |

|---|---|

| Unity 2022.3.62f3 LTS | Current development and validation environment |

| Unity 2021.3.45f2 LTS | Compatibility validation pending |

| Unity 6 LTS | Not yet validated |

## Dependencies

**Unity Input System:** Required.

**Cinemachine:** Optional; not a core framework dependency.

**Universal Render Pipeline (URP):** Used by the current demonstration project.

Compatibility with other render pipelines has not yet been validated.

**---

# Product Strategy

The commercial framework will be developed in independent stages.

## Pack 1 — Base Vehicle Controller

The initial commercial product focuses on the core arcade vehicle experience.

Primary systems:

- Vehicle Controller

- Vehicle Movement

- Vehicle Steering

- Vehicle Suspension

- Ground Detection

- Obstacle Detection

- Surface Detection

- Engine and Surface Audio

- Basic Vehicle Effects

- Vehicle Configuration

The first release must prioritize stability, usability, modularity, and gameplay quality.

## Future Packs and Expansions

Potential expansions include:

- Weapon Systems

- Vehicle Equipment

- Future camera enhancements and optional integrations

- Additional Vehicle Types

- Advanced Surface Reactions

- Specialized Movement Modules

These expansions are not prerequisites for the initial commercial release unless a later product decision changes the scope.

The existing modular Camera module is part of the current framework; future enhancements are separate from its implemented foundation.

**---

# Version 0.1.0 — Foundation

## Objective

Establish the architectural foundation required for a modular vehicle framework.

### Architecture and Documentation

- [x] Initial project structure

- [x] Coding standards

- [x] Development journal

- [x] Architecture documentation

- [x] Architecture Decision Records

- [x] Core lifecycle interface architecture

- [x] Vehicle system interface architecture

- [x] VehicleContext implementation

- [x] VehicleController implementation

### Architecture Principles

The framework follows these principles:

- Modular Architecture

- Single Responsibility Principle

- Separation of Responsibilities

- Reusable Systems

- Low Coupling

- Composition over Inheritance where appropriate

The VehicleController is responsible for orchestrating systems.

**The VehicleController must never contain gameplay logic.**

**Gameplay belongs exclusively to Systems.**

This principle is documented in ADR-015.

### Remaining Foundation Work

- [ ] Review public APIs for commercial use

- [ ] Verify module independence

- [ ] Review namespace consistency

- [ ] Validate package dependencies

- [ ] Complete foundation-level regression testing

**---

# Version 0.2.0 — Vehicle Systems

## Objective

Implement and refine the systems responsible for arcade vehicle behavior.

### Movement

- [x] Vehicle Movement implementation

- [x] Vehicle Steering implementation

- [ ] Complete movement regression testing

- [ ] Finalize movement configuration for commercial release

### Suspension

- [x] Vehicle Suspension implementation

- [x] Suspension behavior testing

- [ ] Complete suspension regression testing across supported environments

- [ ] Finalize suspension configuration and documentation

### Detection

- [x] Ground Detection implementation

- [x] Obstacle Detection implementation

- [x] Surface Detection implementation

- [ ] Complete detection-system regression testing

### Audio

- [x] Engine Audio implementation

- [x] Surface Audio implementation

- [ ] Finalize audio configuration and integration testing

### Effects

- [x] Initial vehicle effects implementation

- [ ] Validate effects across demonstration scenarios

- [ ] Finalize effects configuration for release

### Vehicle Configuration

- [x] Initial Vehicle Configuration architecture

- [ ] Review Inspector usability

- [ ] Validate configuration behavior

- [ ] Finalize commercial configuration defaults

### Gameplay Validation

The framework must preserve:

- Responsive arcade movement

- Predictable steering

- Stable suspension

- Vehicle weight perception

- Enjoyable vehicle handling

**Architecture must not compromise gameplay quality.**

**---

**# Camera Module — Implemented Foundation (Current Development)

## Objective

Provide a reusable top-down camera independent of Cinemachine and vehicle movement implementations.

### Implemented and Demo-Tested

- [x] Modular camera architecture and orchestration through `VehicleCameraController`
- [x] Camera contracts: `ICameraTarget` and `ICameraSystem`
- [x] Shared camera data: `CameraContext` and `CameraFrame`
- [x] Smooth follow (`CameraFollow`)
- [x] Velocity-based look-ahead (`CameraLookAhead`)
- [x] Speed-based dynamic zoom (`CameraDynamics`)
- [x] Optional camera roll (`CameraRoll`)
- [x] Steering and motion roll strategies
- [x] Runtime target switching with state reset where appropriate
- [x] Demonstration with two tank targets and keyboard switching (`T`)
- [x] Camera configuration exposed through `VehicleCameraConfig`
- [x] Architecture and README documentation updated

### Remaining Camera Validation

- [ ] Camera regression testing across supported Unity versions
- [ ] Test target destruction, replacement, and disabled targets in integration scenarios
- [ ] Validate clean-project import and camera setup instructions
- [ ] Review configuration defaults and public APIs for release
- [ ] Confirm camera behavior across representative vehicle speeds and frame rates

**Implementation status:** Completed within the current demo scope. Commercial release validation remains pending.

The camera does not require Cinemachine. The current controller derives its fixed top-down orientation from the configured follow offset; `CameraFrame.LookAtPosition` is available to systems but does not currently drive orientation.

---

# Version 0.3.0 — Demonstration and Integration**

## Objective

Provide a functional demonstration of the framework and validate its integration into a Unity project.

### Demonstration

- [x] Initial demonstration scene

- [x] Tank prefab

- [x] Functional demonstration in Unity 2022

- [x] Initial import testing using a Unity package

- [ ] Final demonstration scene review

- [ ] Final demonstration documentation

- [ ] Verify all included asset redistribution rights

- [ ] Validate clean-project installation and configuration

- [ ] Complete demonstration regression testing

### Demonstration Requirements

The demonstration should communicate the framework's capabilities without requiring users to understand its internal architecture before testing it.

The demonstration should provide clear examples of:

- Vehicle movement

- Steering

- Suspension

- Surface interaction

- Audio feedback

- Basic effects

- Configuration
- Modular camera follow, look-ahead, zoom, and roll
- Runtime camera target switching between demo tanks

The demonstration project currently uses URP.

Support for other render pipelines remains unvalidated.

**---

# Version 1.0.0 — First Public Release

## Objective

Release the first commercial version of the Top-Down Vehicle Controller – Arcade System.

### Release Requirements

- [ ] Stable vehicle framework

- [ ] Finalized public API

- [ ] Complete user documentation

- [ ] Installation and setup instructions

- [ ] Configuration reference

- [ ] Functional demonstration scene
- [ ] Camera module release-level validation and documentation

- [ ] Validated package dependencies

- [ ] Compatibility testing

- [ ] Clean-project import testing

- [ ] Regression testing

- [ ] Asset licensing and redistribution review

- [ ] Unity Asset Store submission preparation

- [ ] Final commercial release review

### Release Scope

The first public release focuses on the Base Vehicle Controller.

Weapon systems, advanced equipment, specialized vehicle modules, and unrelated gameplay frameworks are outside the initial release scope.

Editor tooling may be improved when necessary for configuration and usability, but a separate advanced editor-tools package is not required for the first release.

### Release Quality

The initial release should be:

- Functional

- Stable

- Modular

- Documented

- Easy to integrate

- Easy to configure

- Maintainable

- Suitable for commercial Unity projects

**---

# Future Development — Vehicle Framework Expansion

## Advanced Vehicle Behavior

Potential improvements include:

- Acceleration and deceleration profiles

- Advanced braking behavior

- Surface-dependent friction

- Additional suspension configurations

- Advanced visual effects

- Advanced audio behavior

## Camera System

A future camera system may provide vehicle-oriented camera behavior without requiring Cinemachine as a core dependency.

## Weapons and Equipment

Potential systems include:

- Machine Gun

- Cannon

- Missile Launcher

- Rocket Launcher

- Smoke Launcher

- Radar

Weapon and equipment systems should remain modular and independent of vehicle movement.

## Vehicle Types

Potential supported vehicle types include:

- Tracked Vehicles

- Wheeled Vehicles

- Amphibious Vehicles

- Hover Vehicles

- Aircraft

Different vehicle types may require specialized movement systems.

**---

# Proposed Architecture — Vehicle, Modules and Equipment

ADR-011 proposes distinguishing three concepts:

## Vehicle

Represents the physical vehicle entity.

## Modules

Extend the vehicle's capabilities.

Examples:

- Flight

- Amphibious

- Hover

- AI Driver

- Multiplayer

## Equipment

Represents optional installed components.

Examples:

- Machine Gun

- Cannon

- Missile Launcher

- Radar

**Status: Proposed**

This architectural direction is not yet accepted as a binding implementation requirement.

Implementation details will be determined after the core framework has reached sufficient stability.

**---

# Long-Term Vision

## Inverso Gameplay Framework

The long-term vision extends beyond vehicle control.

Potential gameplay systems include:

- Combat Framework

- Inventory Framework

- Save Framework

- AI Framework

- HUD Framework

- Naval Module

- Aircraft Module

- Character Module

Conceptual organization:

```text

InversoGameplayFramework

|

+-- Core

|   +-- Movement

|   +-- Combat

|   +-- Inventory

|   +-- Save

|   +-- AI

|   +-- HUD

|

+-- Modules

    +-- Vehicle

    +-- Naval

    +-- Aircraft

    +-- Hover

    +-- Character

```

These modules are part of the long-term vision and are not included in the current commercial product scope.

**---

# Development Priorities

Development should follow this priority order:

1. Preserve gameplay quality.

2. Stabilize the Base Vehicle Controller.

3. Validate system independence.

4. Complete integration testing.

5. Finalize documentation.

6. Validate commercial packaging.

7. Prepare the first public release.

8. Expand only after the base product is stable.

**---

# Development Philosophy

**We design before we implement.**

**Architecture serves creation. Creation serves fun.**

**Code is a tool. Gameplay is the goal.**

The framework must remain modular without becoming unnecessarily complex.

Every new module must build upon a stable foundation.

The long-term vision must never compromise the quality of the current release.

**Finish before expanding.**

**---

# Roadmap Maintenance

This roadmap must be reviewed as development progresses.

Completed tasks should reflect verified implementation.

Release readiness must be evaluated separately from implementation status.

New architectural decisions should be documented through ADRs when appropriate.

Future expansions should not be added to the current release scope without an explicit product decision.

**---

# End of Document

Top-Down Vehicle Controller – Arcade System

© 2026 Inverso Studio. All rights reserved.
