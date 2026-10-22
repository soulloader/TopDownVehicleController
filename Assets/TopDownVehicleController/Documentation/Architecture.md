=====================================================
                 Inverso Studio
=====================================================

Top-Down Vehicle Controller – Arcade System

Document:
Architecture

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

=====================================================

# Architecture

**Architecture:** Modular Framework

---

# Purpose

This document defines the architectural principles, module organization, dependency boundaries, and communication rules adopted by the Top-Down Vehicle Controller – Arcade System.

All framework systems must follow this architecture.

---

# Architecture Goals

- Modular design
- Independent systems
- Low coupling
- High cohesion
- Scalability
- Maintainability
- Extensibility
- Reusability
- Production-ready quality

---

# Architectural Principles

The framework is built around independent modules.

Each module has a single responsibility.

Each system should be replaceable without affecting unrelated systems.

Gameplay logic must remain inside its corresponding system.

The architecture favors composition over inheritance.

**Systems own behavior. Configurations own values. Context owns references.**

**Discover systems. Configure behavior. Compose vehicles.**

Architecture must support gameplay quality and development efficiency.

---

# Framework Structure

The framework is organized into the following principal directories:

```text
TopDownVehicleController
|
+-- Audio
+-- Camera
+-- Core
+-- Data
+-- Demo
+-- Documentation
+-- Input
+-- Models
+-- Vehicle
```

Additional directories may be introduced when required by the architecture.

Editor extensions, Gizmos, and configuration resources must remain organized according to their responsibilities.

---

# Module Responsibilities

## Core

Shared functionality used by framework modules.

Contains:

- Interfaces
- Utilities
- Extensions
- Events
- Attributes
- Shared data structures

The Core module must remain independent of higher-level modules.

It must never reference:

- Vehicle
- Audio
- Input
- Demo
- Editor

Every other module may reference Core.

---

## Camera

Responsible for modular camera behavior and target tracking.

Responsibilities:

- Camera system coordination
- Smooth target following
- Velocity-based look-ahead
- Dynamic zoom based on target speed
- Configurable camera roll
- Runtime camera target switching
- Camera configuration and data management

The Camera module communicates with targets through ICameraTarget.

Camera systems implement ICameraSystem and operate on a shared CameraFrame.

VehicleCameraController coordinates camera systems without implementing their individual behaviors.

The Camera module must not depend directly on VehicleController, VehicleMovement, or vehicle-specific gameplay implementations.

Camera functionality must remain independent of Cinemachine.

---

## Vehicle

Contains gameplay systems related to vehicle behavior.

Responsibilities:

- VehicleController
- Movement
- Suspension
- Detection
- Vehicle Effects
- Vehicle Data
- Vehicle Interfaces

Vehicle-specific gameplay logic belongs in this module.

---

## Input

Responsible for player input abstraction.

Uses the Unity Input System.

Responsibilities:

- Input Providers
- Input Actions
- Input Abstraction

No vehicle gameplay logic should exist inside this module.

---

## Audio

Responsible for shared audio functionality and related resources.

Responsibilities:

- Engine Audio Resources
- Surface Audio Resources
- Audio Configurations

Vehicle-specific audio behavior may be implemented within the Vehicle module while respecting module boundaries.

Audio systems must not implement unrelated gameplay logic.

---

## Demo

Contains demonstration assets.

Includes:

- Scenes
- Prefabs
- Scripts
- Materials
- Textures
- Sample Configurations

The Demo module must never be required for the core framework to function.

---

## Documentation

Contains project documentation, architecture decisions, coding standards, and development records.

No runtime gameplay logic belongs in this directory.

---

## Editor

Reserved for Unity Editor extensions.

Editor scripts must never be included in runtime assemblies.

Editor functionality must not become a mandatory runtime dependency.

---

## Settings and Configurations

Configuration assets should be organized according to their purpose.

Examples:

- Default Vehicle Config
- Default Surface Config
- Default Audio Config

Runtime configuration types belong to their respective modules.

Unity project rendering settings are separate from vehicle gameplay configurations.

---

# Core Architecture

```text
Core
|
+-- Attributes
+-- Data
+-- Events
+-- Extensions
+-- Interfaces
+-- Utilities
```

Every framework module may depend on Core.

Core depends on no higher-level framework module.

---
# Camera Architecture

```text
Camera
|
+-- Configurations
|   +-- VehicleCameraConfig
|
+-- Controllers
|   +-- VehicleCameraController
|
+-- Data
|   +-- CameraContext
|   +-- CameraFrame
|
+-- Interfaces
|   +-- ICameraSystem
|   +-- ICameraTarget
|
+-- Systems
|   +-- Dynamics
|   |   +-- CameraDynamics
|   |
|   +-- Follow
|   |   +-- CameraFollow
|   |
|   +-- LookAhead
|   |   +-- CameraLookAhead
|   |
|   +-- Roll
|       +-- CameraRoll
|       +-- ICameraRollStrategy
|       +-- MotionRollStrategy
|       +-- SteeringRollStrategy
|
+-- Targets
    +-- RigidbodyCameraTarget
```

The Camera module consists of independent systems coordinated by VehicleCameraController.

CameraContext provides shared references and configuration.

CameraFrame carries the calculated camera position and look-at position between systems.

ICameraTarget abstracts target position, orientation, and velocity.

Camera systems must remain independent of vehicle-specific movement implementations.

Camera target switching is coordinated by VehicleCameraController through CameraContext.

# Vehicle Architecture

```text
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

The Vehicle module is organized around independent systems coordinated by the VehicleController.

---

# VehicleController

VehicleController is the central coordinator of vehicle systems.

Responsibilities:

- Initialize systems
- Hold and distribute required references
- Coordinate execution
- Dispatch relevant events

VehicleController must not implement individual gameplay systems.

Movement, suspension, detection, audio, and effects must remain within their respective components.

---

# System Responsibilities

## Movement

Controls vehicle movement.

Responsible for movement-related behavior.

Movement logic must not be duplicated in unrelated systems.

---

## Suspension

Controls vehicle suspension simulation.

Responsible for suspension behavior, ground interaction, and associated vehicle stabilization.

Suspension logic must remain isolated from unrelated gameplay systems.

---

## Detection

Handles environment and obstacle detection.

Examples:

- Ground Detection
- Obstacle Detection
- Surface Detection

Detection systems provide information to other systems without taking ownership of unrelated behavior.

---

## Effects

Controls vehicle visual effects.

Examples:

- Particles
- Trails
- Dust
- Surface Effects

Visual effects should react to vehicle state and environmental information.

---

## Audio

Coordinates vehicle-related audio behavior.

Examples:

- Engine Sounds
- Surface Sounds
- Movement Feedback

Audio systems should react to relevant vehicle states without controlling vehicle movement.

---

# Communication Rules

Systems communicate using:

1. Interfaces
2. Events
3. Public APIs

Avoid direct dependencies whenever possible.

Avoid circular references.

Communication must preserve clear ownership of responsibilities.

---

# Dependency Flow

Dependencies must follow the framework's architectural boundaries.

```text
Vehicle ------> Core
   |
   +----------> Input abstractions
   |
   +----------> Audio abstractions

Camera -------> Core (when shared abstractions are needed)

Input --------> Core

Audio --------> Core

Demo ---------> Framework modules
```

The diagram represents permitted architectural relationships, not a mandatory dependency for every system.

Core never references higher-level modules.

Dependencies must not create circular references.

---

# Data Flow

A typical vehicle execution flow is:

```text
Player Input
     |
     v
Input Provider
     |
     v
VehicleController
     |
     +----> Movement System
     |
     +----> Suspension System
     |
     +----> Detection Systems
     |
     +----> Effects Systems
     |
     +----> Audio Systems
```

The VehicleController coordinates execution.

Individual systems own their behavior.

Systems may exchange information through the approved communication mechanisms.

The diagram is conceptual and does not prescribe a strict sequential execution order.

---

# Script Responsibilities

Every script must answer the following questions before implementation:

- What is its responsibility?
- Which module does it belong to?
- Who consumes it?
- What are its dependencies?
- How does it communicate?
- How can it be replaced?
- How will it be tested?

Implementation begins only after these questions are answered.

---

# Dependency Rules

## Allowed

```text
Vehicle -> Core

Camera -> Core

Input -> Core

Audio -> Core

Demo -> Core

Vehicle -> Input abstractions

Vehicle -> Audio abstractions
```

These relationships must remain intentional and must not introduce circular dependencies.

## Not Allowed

```text
Core -> Vehicle

Core -> Audio

Core -> Input

Audio -> Vehicle

Input -> Audio
```

Higher-level gameplay modules must not become dependencies of Core.

---

# ScriptableObjects

ScriptableObject classes belong inside their respective modules.

Examples:

```text
Vehicle
|
+-- Configurations

Audio
|
+-- ScriptableObjects

Input
|
+-- ScriptableObjects
```

Configuration assets should be stored in appropriate configuration or data directories.

Examples:

```text
Vehicle
|
+-- Configurations
    |
    +-- VehicleConfig.asset
```

Additional configuration assets may be introduced as their corresponding systems are implemented.

---

# Unity Dependencies

## Official Development Environment

**Unity 2022.3.62f3 LTS**

This is the official Unity Editor version used for current development and validation.

## Minimum Supported Version Candidate

**Unity 2021.3.45f2 LTS**

Compatibility must be validated before official support is declared.

## Future Compatibility

**Unity 6 LTS**

Compatibility has not yet been validated.

## Mandatory Package

- Unity Input System

The framework uses Unity's Input System for input handling.


## Optional Package

No additional camera package is required.

The framework provides its own modular camera system.

The Camera module operates independently of Cinemachine.

Third-party camera integrations may be implemented externally without modifying the core framework.

## Render Pipeline

The current development environment and demonstration assets use the Universal Render Pipeline (URP).

URP is required to reproduce the current demonstration rendering setup as tested.

Compatibility with other render pipelines has not yet been validated.

---

# Assembly Definitions

Each runtime module should have its own Assembly Definition when the architecture and dependency structure require it.

Recommended assemblies:

```text
TopDownVehicleController.Core

TopDownVehicleController.Vehicle

TopDownVehicleController.Input

TopDownVehicleController.Audio

TopDownVehicleController.Editor
```

Editor assemblies must remain separate from runtime assemblies.

Assembly Definitions should reflect actual module dependencies and avoid circular references.

---

# Namespace Convention

Namespaces mirror the folder hierarchy.

Examples:

```csharp
TopDownVehicleController.Core

TopDownVehicleController.Core.Interfaces

TopDownVehicleController.Vehicle.Controllers

TopDownVehicleController.Vehicle.Systems.Movement

TopDownVehicleController.Audio.Engine

TopDownVehicleController.Input.Providers
```

Namespace conventions must remain consistent throughout the framework.

---

# Design Rules

- One public class per file.
- One responsibility per class.
- One module per feature.
- Keep systems independent.
- Avoid duplicated code.
- Avoid hidden dependencies.
- Keep APIs small.
- Prefer events over unnecessary polling.
- Prefer composition over inheritance.
- Design for long-term scalability.
- Preserve established gameplay behavior during architectural refactoring.
- Validate changes before integration.

---

# Future Expansion

The architecture is designed to accommodate additional modules without requiring unnecessary changes to existing systems.

Examples:

```text

Weapons

AI

Multiplayer

Mobile

Networking
```

Each new module should integrate through the existing architecture while preserving modularity and minimizing coupling.

Future modules are architectural plans and are not part of the initial feature guarantee.

---

# Final Principle

The framework is built as a collection of independent systems coordinated by a lightweight controller.

Every architectural decision should prioritize readability, maintainability, scalability, gameplay quality, and commercial-quality code suitable for long-term support and Unity Asset Store distribution.

**Architecture serves creation. Creation serves fun.**

-----------------------------------------------------
Top-Down Vehicle Controller – Arcade System  
© 2026 Inverso Studio. All rights reserved.
-----------------------------------------------------