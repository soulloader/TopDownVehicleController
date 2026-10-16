=====================================================
                 Inverso Studio
=====================================================

Top-Down Vehicle Controller – Arcade System

Document:
CodingStandards

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

# Coding Standards

## Purpose

This document defines the coding conventions, naming standards, organizational principles, and development practices adopted by the Top-Down Vehicle Controller – Arcade System.

Its purpose is to maintain consistency, readability, modularity, and long-term maintainability throughout the framework.

All new framework code should follow these standards.

---

# Overview

Top-Down Vehicle Controller – Arcade System is a modular framework for Unity designed to create arcade-style top-down vehicle games.

The framework is built around independent systems, making it easier to maintain, extend, customize, and integrate into different projects.

The goal is to provide a clean, scalable, and production-ready architecture suitable for commercial game development and Unity Asset Store distribution.

---

# Project Philosophy

The framework follows these principles:

- Modular Architecture
- Single Responsibility Principle
- Reusable Components
- Clean and Maintainable Code
- Composition over Inheritance
- Low Coupling
- High Cohesion
- High Readability
- Commercial Asset Quality
- Long-Term Scalability

**Architecture serves creation. Creation serves fun.**

**Code is a tool. Gameplay is the goal.**

---

# Unity Development Environment

## Official Development Version

**Unity 2022.3.62f3 LTS**

This is the official Unity Editor version used for current framework development and validation.

## Minimum Supported Version Candidate

**Unity 2021.3.45f2 LTS**

Compatibility remains pending validation.

## Future Compatibility

**Unity 6 LTS**

Compatibility has not yet been validated.

## Development Rules

- Use the official development version for primary implementation and testing.
- Do not declare compatibility with an untested Unity version.
- Avoid unnecessary dependencies on version-specific APIs.
- Validate compatibility before expanding the official support matrix.
- Preserve established gameplay behavior when migrating Unity versions.

---

# Folder Structure

The project is organized primarily by modules rather than by general asset types.

Each module owns the resources and systems associated with its responsibility.

Example:

```text id="62ps4g"
TopDownVehicleController
|
+-- Audio
+-- Core
+-- Data
+-- Demo
+-- Documentation
+-- Input
+-- Models
+-- Vehicle
```

Example of the Vehicle module:

```text id="j9at50"
Vehicle
|
+-- Configurations
+-- Controllers
+-- Data
+-- Interfaces
+-- Prefabs
+-- Systems
    |
    +-- Audio
    +-- Detection
    +-- Effects
    +-- Movement
    +-- Suspension
```

The folder structure may evolve as new modules are implemented.

---

# Naming Conventions

## Folders

- Use PascalCase.
- Keep folders organized alphabetically whenever practical.
- Organize primarily by module and responsibility.
- Avoid ambiguous or duplicated folder names.
- Keep related resources within their responsible modules.

Examples:

```text id="m56wkc"
Controllers
Data
Interfaces
Movement
Prefabs
Suspension
Systems
```

## Scripts

- Use one public class per file.
- The filename must match the public class name.
- Use PascalCase for class names.
- Choose descriptive names based on responsibility.
- Avoid generic names that do not explain the system's purpose.

Examples:

```text id="0ik54x"
VehicleController.cs
VehicleMovement.cs
VehicleSuspension.cs
SurfaceDetector.cs
ObstacleDetector.cs
VehicleConfig.cs
```

## Classes

Use PascalCase for class names.

Example:

```csharp id="3qcn86"
public class VehicleMovement
{
}
```

## Interfaces

Use PascalCase with the `I` prefix.

Examples:

```csharp id="2j7m0m"
IInitializable
IUpdatable
IFixedTickable
ILateTickable
IVehicleSystem
```

## Methods

Use PascalCase for method names.

Examples:

```csharp id="upafda"
Initialize()
FixedTick()
LateTick()
```

Lifecycle method naming must follow the established architectural decisions.

## Local Variables and Parameters

Use camelCase for local variables and method parameters.

Example:

```csharp id="1qj90v"
float moveSpeed;
float rotationSpeed;
```

## Public Properties

Use PascalCase for public properties.

Example:

```csharp id="15szlq"
public bool IsGrounded { get; private set; }
```

---

# Namespace Convention

Namespaces should follow the physical folder structure whenever practical.

Examples:

```csharp id="rpgdss"
TopDownVehicleController.Core

TopDownVehicleController.Core.Interfaces

TopDownVehicleController.Vehicle.Controllers

TopDownVehicleController.Vehicle.Systems.Movement

TopDownVehicleController.Vehicle.Systems.Suspension

TopDownVehicleController.Input.Providers
```

Namespaces must remain consistent across the framework.

Avoid unnecessary namespace changes that would break existing integrations.

---

# Architecture

The framework follows a modular architecture.

The `VehicleController` is responsible only for coordinating systems.

**The VehicleController must never contain gameplay logic.**

**Gameplay belongs exclusively to Systems.**

This principle is established by ADR-015.

Example:

```text id="o6zkqw"
VehicleController
        |
        +-- VehicleMovement
        +-- VehicleSuspension
        +-- Detection Systems
        +-- Audio Systems
        +-- Effects Systems
```

Each system must own its behavior.

The controller must not accumulate movement, suspension, audio, visual effects, or other gameplay implementations.

---

# Single Responsibility Principle

Each class should perform one clearly defined responsibility.

Examples of specialized systems:

- VehicleMovement
- VehicleSuspension
- SurfaceDetector
- ObstacleDetector

An architectural violation would be a `VehicleController` containing movement calculations, suspension physics, audio processing, particles, and input implementation.

Responsibilities must be separated into appropriate systems.

---

# Code Organization

- Keep classes focused.
- Avoid duplicated code.
- Prefer composition over inheritance when appropriate.
- Separate gameplay logic from visual presentation.
- Keep systems independent.
- Avoid unnecessary direct dependencies.
- Avoid circular references.
- Maintain clear ownership of behavior.
- Keep public APIs focused and understandable.

Systems should be independently maintainable whenever possible.

---

# System Lifecycle

Framework systems participate in the application lifecycle through interfaces.

This approach is established by ADR-001.

The original lifecycle interface set includes:

```text id="dl20vm"
IInitializable
IUpdatable
IFixedTickable
ILateTickable
IResettable
IValidatable
IDisposableSystem
```

Lifecycle methods must follow the established naming convention.

Examples:

```csharp id="7tmh4x"
Initialize()
Update()
FixedTick()
LateTick()
```

A system should implement only the lifecycle contracts required by its responsibility.

The controller coordinates execution without taking ownership of system-specific gameplay logic.

---

# Dependencies and Communication

Systems should communicate through established architectural mechanisms:

1. Interfaces
2. Events
3. Public APIs

Use shared context data where appropriate.

Avoid direct dependencies when an abstraction provides a cleaner solution.

Avoid circular references between modules.

The Core module must remain independent of higher-level gameplay modules.

---

# Data and Configuration

Configuration data should be separated from behavior whenever appropriate.

Prefer ScriptableObjects for reusable configuration assets.

Examples:

```text id="2uyhce"
VehicleConfig
SurfaceConfig
InputConfig
```

These names represent configuration concepts; not every example is necessarily implemented in the current version.

Configuration classes should belong to their responsible modules.

Runtime systems own behavior.

Configuration assets own reusable values.

Shared contexts own references required by systems.

**Systems own behavior. Configurations own values. Context owns references.**

---

# Documentation Standards

Every public system should contain a brief XML summary explaining its responsibility.

Example:

```csharp id="4sc2hv"
/// <summary>
/// Controls vehicle movement behavior.
/// </summary>
public class VehicleMovement
{
}
```

Complex methods should include comments when necessary to explain intent.

Avoid comments that merely repeat obvious code behavior.

Documentation should explain architectural responsibilities and important design decisions.

Public APIs should be documented clearly enough for developers integrating the framework.

---

# Development Workflow

Before implementing a new script, define its responsibility within the architecture.

The following questions should be answered:

1. What is the responsibility of this script?
2. Which module does it belong to?
3. Who consumes it?
4. What are its dependencies?
5. How does it communicate with other systems?
6. How can it be replaced?
7. How will it be tested?

The recommended development sequence is:

1. Define responsibility.
2. Define module.
3. Define dependencies.
4. Design communication.
5. Document architecture.
6. Implement.
7. Test.
8. Integrate.
9. Document changes.

**We design before we implement.**

---

# Testing and Validation

Each system should be tested before integration whenever practical.

Testing should verify:

- Expected behavior
- System initialization
- Correct interaction with other modules
- Absence of unnecessary dependencies
- Stability of existing gameplay behavior

Architectural refactoring must not compromise the intended arcade vehicle experience.

The original vehicle behavior remains an important reference when evaluating gameplay changes.

---

# Architectural Decision Records

Important architectural decisions must be recorded in the project's ADR documentation.

The `ADR.md` file serves as the central index.

Individual ADR files contain detailed decision records when available.

Accepted decisions should not be silently rewritten.

Proposed decisions must not be treated as accepted.

If an architectural direction changes, the modification should be documented through an appropriate new decision record.

---

# Future Updates

This document will evolve together with the framework.

Updates must preserve established architectural decisions unless a new decision formally supersedes them.

Coding standards should remain consistent across development versions and commercial releases.

---

# Design Decisions

## 2026-07-06 — Initial Coding Standards

The following standards were established during the initial framework planning:

- Framework organized by modules instead of general asset types.
- VehicleController acts only as the coordinator.
- Systems follow the Single Responsibility Principle.
- Folder names use PascalCase.
- Folder structure follows alphabetical order whenever practical.
- Namespaces mirror the folder hierarchy.
- Architecture designed for scalability and commercial Unity Asset Store distribution.

## 2026-10-14 — Documentation Alignment

The documentation has been aligned with the current development environment:

- Unity 2022.3.62f3 LTS established as the official development version.
- Unity 2021.3.45f2 LTS retained as a minimum supported version candidate.
- Unity 6 LTS compatibility remains unvalidated.
- Existing architectural principles preserved.
- Coding conventions reorganized for clarity and maintainability.

---

# Final Principle

The framework must remain readable, modular, maintainable, and extensible.

Architectural decisions must serve gameplay quality.

**Architecture serves creation. Creation serves fun.**

-----------------------------------------------------
Top-Down Vehicle Controller – Arcade System  
© 2026 Inverso Studio. All rights reserved.
-----------------------------------------------------