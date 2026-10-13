=====================================================
                 Inverso Studio
=====================================================

Top-Down Vehicle Controller – Arcade System

Document:
#Architecture


Version: 0.1.0

Author:
Edison Oliveira

Engine:
Unity 2021.3.45f1 LTS (Minimum Supported Version)
Compatible up to Unity 6 LTS

Language:
C#

=====================================================


# Architecture

**Architecture:** Modular Framework

---

# Purpose

This document defines the architectural principles, module organization and communication rules adopted by the Top-Down Vehicle Controller - Arcade System.

All framework systems must follow this architecture.

---

# Architecture Goals

* Modular design
* Independent systems
* Low coupling
* High cohesion
* Scalability
* Maintainability
* Extensibility
* Reusability
* Production-ready quality

---

# Architectural Principles

The framework is built around independent modules.

Each module has a single responsibility.

Each system should be replaceable without affecting unrelated systems.

Business logic must remain inside its corresponding system.

The architecture favors composition over inheritance.]

Systems own behavior. Configurations own values. Context owns references.

Discover systems. Configure behavior. Compose vehicles.

---

# Framework Structure

```text
TopDownVehicleController
?
??? Audio
??? Core
??? Data
??? Demo
??? Documentation
??? Editor
??? Gizmos
??? Input
??? Models
??? Prefabs
??? Settings
??? Vehicle
```

---

# Module Responsibilities

## Core

Shared functionality used by every module.

Contains:

* Interfaces
* Utilities
* Extensions
* Events
* Attributes
* Shared data structures

The Core module must remain completely independent.

It must never reference:

* Vehicle
* Audio
* Input
* Demo
* Editor

Every other module may reference Core.

---

## Vehicle

Contains every gameplay system related to the vehicle.

Responsibilities:

* VehicleController
* Movement
* Suspension
* Detection
* Vehicle Effects
* Vehicle Data
* Vehicle Interfaces

Vehicle-specific gameplay logic belongs only here.

---

## Input

Responsible for player input.

Uses Unity Input System.

Responsibilities:

* Input Providers
* Input Actions
* Input Abstraction

No gameplay logic should exist inside this module.

---

## Audio

Responsible for every audio system.

Responsibilities:

* Engine Audio
* Surface Audio
* Audio Configurations

Audio must not implement gameplay logic.

---

## Demo

Contains demonstration assets.

Includes:

* Scenes
* Prefabs
* Scripts
* Materials

The Demo module must never be required for the framework to function.

---

## Documentation

Contains all project documentation.

No runtime assets.

---

## Editor

Contains Unity Editor extensions.

Editor scripts must never be included in runtime assemblies.

---

## Settings

Contains project configuration assets.

Examples:

* Default Vehicle Config
* Default Surface Config
* Default Audio Config

Only asset files should exist here.

---

# Core Architecture

```text
Core
?
??? Attributes
??? Data
??? Events
??? Extensions
??? Interfaces
??? Utilities
```

Every module depends on Core.

Core depends on no other module.

---

# Vehicle Architecture

```text
Vehicle
?
??? Controllers
??? Data
??? Interfaces
??? ScriptableObjects
??? Systems
    ??? Audio
    ??? Detection
    ??? Effects
    ??? Movement
    ??? Suspension
```

---

# VehicleController

VehicleController is the central coordinator.

Responsibilities:

* Initialize systems
* Hold references
* Coordinate execution
* Dispatch events

VehicleController must not implement gameplay logic.

---

# System Responsibilities

## Movement

Controls vehicle movement.

Only movement logic.

---

## Suspension

Controls suspension simulation.

Only suspension logic.

---

## Detection

Handles collision detection.

Ground detection.

Obstacle detection.

Surface detection.

---

## Effects

Controls visual effects.

Particles.

Trails.

Dust.

Surface effects.

---

## Audio

Coordinates vehicle audio.

Engine sounds.

Surface sounds.

Movement feedback.

---

# Communication Rules

Systems communicate using:

1. Interfaces
2. Events
3. Public APIs

Avoid direct dependencies whenever possible.

Avoid circular references.

---

# Dependency Flow

```text
             Vehicle
                ?
                ?
             Input
                ?
                ?
             Audio
                ?
                ?
              Core
```

Core never references higher-level modules.

Dependencies always point toward Core.

---

# Data Flow

```text
Player Input
      ?
      ?
Input Provider
      ?
      ?
VehicleController
      ?
      ?
Movement System
      ?
      ?
Suspension
      ?
      ?
Effects
      ?
      ?
Audio
```

---

# Script Responsibilities

Every script must answer the following questions before implementation.

* What is its responsibility?
* Which module does it belong to?
* Who consumes it?
* What are its dependencies?
* How does it communicate?
* How can it be replaced?
* How will it be tested?

Implementation begins only after these questions are answered.

---

# Dependency Rules

Allowed

```text
Vehicle ? Core

Input ? Core

Audio ? Core

Demo ? Core

Vehicle ? Input

Vehicle ? Audio
```

Not Allowed

```text
Core ? Vehicle

Core ? Audio

Core ? Input

Audio ? Vehicle

Input ? Audio
```

---

# ScriptableObjects

ScriptableObject classes belong inside their respective modules.

Examples:

```text
Vehicle
??? ScriptableObjects

Audio
??? ScriptableObjects

Input
??? ScriptableObjects
```

Configuration assets belong in the Settings folder.

Example:

```text
Settings
??? DefaultVehicleConfig.asset
??? DefaultSurfaceConfig.asset
??? DefaultAudioConfig.asset
```

---

# Unity Dependencies

Mandatory packages:

* Input System
* Cinemachine

The framework assumes both packages are installed.

---

# Assembly Definitions

Each runtime module should have its own Assembly Definition.

Recommended assemblies:

```text
TopDownVehicleController.Core

TopDownVehicleController.Vehicle

TopDownVehicleController.Input

TopDownVehicleController.Audio

TopDownVehicleController.Editor
```

---

# Namespace Convention

Namespaces mirror the folder hierarchy.

Example:

```csharp
TopDownVehicleController.Core

TopDownVehicleController.Core.Interfaces

TopDownVehicleController.Vehicle.Controllers

TopDownVehicleController.Vehicle.Systems.Movement

TopDownVehicleController.Audio.Engine

TopDownVehicleController.Input.Providers
```

---

# Design Rules

* One public class per file.
* One responsibility per class.
* One module per feature.
* Keep systems independent.
* Avoid duplicated code.
* Avoid hidden dependencies.
* Keep APIs small.
* Prefer events over polling.
* Prefer composition over inheritance.
* Design for long-term scalability.

---

# Future Expansion

The architecture is prepared for additional modules without changing the existing structure.

Examples:

```text
Camera

Weapons

AI

Multiplayer

Mobile

Networking
```

Each new module should integrate through the existing architecture while preserving modularity and minimizing coupling.

---

# Final Principle

The framework is built as a collection of independent systems coordinated by a lightweight controller.

Every architectural decision should prioritize readability, maintainability, scalability and commercial-quality code suitable for long-term support and Unity Asset Store distribution.

-----------------------------------------------------
Top-Down Vehicle Controller – Arcade System
© 2026 Inverso Studio. All rights reserved.
-----------------------------------------------------
