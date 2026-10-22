=====================================================

                 Inverso Studio

=====================================================

Top-Down Vehicle Controller – Arcade System

Document:
README

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

# Top-Down Vehicle Controller – Arcade System

**Inverso Studio**
**Author:** Edison Oliveira
**Version:** 0.1.0 (development)
**Language:** C#
**Architecture:** Modular Framework

> **Architecture serves creation. Creation serves fun.**

A modular framework for building arcade-style top-down vehicle games in Unity.

---

## Overview

Top-Down Vehicle Controller – Arcade System is designed around independent systems with clear responsibilities. Rather than placing gameplay logic inside one large controller, the framework separates movement, suspension, detection, audio, effects, and camera behavior into focused modules.

The project prioritizes readability, maintainability, extensibility, and enjoyable arcade gameplay. It is under active development toward a commercial Unity Asset Store release.

**This is a development version, not a finished commercial package.** Features and compatibility claims are subject to further testing.

## Vision

Build a production-ready, reusable vehicle framework that is straightforward to configure and extend across projects of different sizes.

## Core Principles

- Modular architecture and single responsibility
- Composition over inheritance
- Low coupling and high cohesion
- Reusable components and clear interfaces
- Readable, maintainable C# code
- Performance-conscious implementation
- Scalable systems and long-term support
- Gameplay quality as the primary goal

> **We design before we implement.**
> **Code is a tool. Gameplay is the goal.**

---

## Development Status

### Vehicle Foundation — Pack 1

The initial pack focuses on the following areas:

- Core architecture and vehicle coordination
- Vehicle movement and steering
- Suspension
- Ground, surface, and obstacle detection
- Engine and surface audio
- Basic visual effects

These areas form the Pack 1 development scope. Their inclusion in this list does not imply that every system is feature-complete or commercially validated.

### Modular Camera — Implemented and Tested in the Demo

The current camera module includes:

- Smooth target following
- Velocity-based look-ahead
- Dynamic zoom based on vehicle speed
- Optional camera roll
- Two roll strategies: **Steering** and **Motion**
- Runtime switching between camera targets
- ScriptableObject-based camera configuration
- A Rigidbody-backed implementation of `ICameraTarget`

The camera has been tested in the development demo, including target switching between two vehicles and smooth zoom transitions. Additional integration and release testing remain necessary.

---

## Unity Compatibility

| Unity version | Status |
|---|---|
| **2022.3.62f3 LTS** | Official development baseline; validated in current project tests |
| **2021.3.45f2 LTS** | Minimum support candidate; pending validation |
| **Unity 6 LTS** | Not yet validated |

The current development environment uses **Unity 2022.3.62f3 LTS**.

The commercial support matrix will be finalized after compatibility testing. An unvalidated Unity version is not an officially supported version.

## Dependencies and Rendering

### Required Package

- **Unity Input System** — used for player input handling.

The package must be installed and configured in the target Unity project for the framework's input integration.

### Camera

The framework includes its **own modular camera system**. **Cinemachine is not required** for the camera module or the core vehicle controller.

Third-party camera integrations may be developed separately without introducing them as core dependencies.

### Render Pipeline

The development environment and current demo use the **Universal Render Pipeline (URP)**.

URP is required to reproduce the demo's currently tested materials and rendering setup. Compatibility with other render pipelines has not yet been validated.

---

## Folder Structure

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

### Vehicle Module

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

### Camera Module

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
|   +-- Follow
|   |   +-- CameraFollow
|   +-- LookAhead
|   |   +-- CameraLookAhead
|   +-- Roll
|       +-- CameraRoll
|       +-- ICameraRollStrategy
|       +-- MotionRollStrategy
|       +-- SteeringRollStrategy
|
+-- Targets
    +-- RigidbodyCameraTarget
```

The folder structure may evolve as development continues.

---

## Module Overview

### Core

Shared utilities, interfaces, events, extensions, and common architectural components. Core must remain independent of higher-level framework modules.

### Vehicle

Vehicle gameplay systems and coordination through `VehicleController`. Movement, suspension, detection, audio, and effects remain the responsibilities of their respective systems.

### Camera

Independent camera behavior coordinated by `VehicleCameraController`. The module communicates with tracked objects through `ICameraTarget`, rather than depending directly on vehicle-specific movement implementations.

### Input

Input abstraction using the Unity Input System.

### Audio

Shared audio resources and related functionality. Vehicle-specific audio behavior may live inside the Vehicle module.

### Data

Shared data and configuration resources.

### Models

Vehicle meshes, materials, and related visual resources.

### Demo

Example scenes, prefabs, scripts, and sample configurations. Demo content must not be a required runtime dependency.

### Documentation

Architecture decisions, coding standards, development records, roadmap, and other project documents.

---

## Architecture Overview

The framework separates coordination from implementation.

```text
VehicleController
    |
    +-- Movement
    +-- Suspension
    +-- Detection
    +-- Audio
    +-- Effects

VehicleCameraController
    |
    +-- CameraFollow
    +-- CameraLookAhead
    +-- CameraDynamics
    +-- CameraRoll
```

The camera controller and vehicle controller are independent. The camera follows an `ICameraTarget`, not a specific vehicle controller.

### Camera Data Flow

1. `RigidbodyCameraTarget` implements `ICameraTarget`, exposing the target transform and velocity.
2. `VehicleCameraController` creates and shares a `CameraContext`.
3. Each camera system updates a shared `CameraFrame` during `LateTick`.
4. The controller applies the calculated camera position and roll.
5. `SetTarget(ICameraTarget)` changes the tracked object at runtime.

`CameraRoll` resets its internal state when the target changes. `CameraDynamics` retains its current zoom state so the transition toward the new target's speed remains smooth.

`CameraFrame.LookAtPosition` is currently calculated as shared frame data; the controller uses the configured follow offset to establish its fixed top-down camera orientation.

### Dependency Boundaries

- Camera systems depend on the `ICameraTarget` contract, not `VehicleController` or `VehicleMovement`.
- `CameraContext` holds shared references and configuration.
- `CameraFrame` carries intermediate camera calculations.
- `ICameraSystem` defines `Initialize(CameraContext)` and `LateTick(ref CameraFrame)`.
- Cinemachine is not a dependency of the camera module.
- Demo content must not be required by runtime modules.

See `Architecture.md` for the complete architectural rules.

---

## Camera Configuration

Camera settings are stored in a `VehicleCameraConfig` ScriptableObject.

| Group | Parameters |
|---|---|
| Follow | `followOffset`, `followSmoothTime` |
| Look Ahead | `lookAheadDistance`, `speedInfluence` |
| Camera Dynamics | `maxSpeedZoomOut`, `dynamicsSmoothTime`, `speedForMaxZoomOut` |
| Camera Roll | `enableCameraRoll`, `cameraRollMode`, `maxRollAngle`, `rollSmoothTime`, `rollSensitivity` |

Camera roll is **disabled by default in the configuration class**. It can be enabled and adjusted per configuration asset.

The current demo includes a camera target-switching test: pressing **T** switches between the two configured targets. This test script belongs to the Demo module and is not required for production integration.

### Integrating a Camera Target

A tracked object must provide an implementation of `ICameraTarget`:

```csharp
public interface ICameraTarget
{
    Transform TargetTransform { get; }
    Vector3 Velocity { get; }
}
```

For Rigidbody-based vehicles, `RigidbodyCameraTarget` supplies this implementation. Other target adapters can implement the same interface without changing the camera systems.

The `VehicleCameraController` needs a camera transform, a target provider, and a `VehicleCameraConfig` asset. Runtime target changes can be requested through `SetTarget(ICameraTarget)`.

This section describes the existing API; a complete release-ready setup guide will be prepared and validated before distribution.

---

## Development Workflow

Each new system follows a structured process:

1. Define its responsibility.
2. Identify its module.
3. Define dependencies.
4. Design communication.
5. Document the architecture.
6. Implement.
7. Test.
8. Integrate.
9. Document changes.

Architectural changes should preserve validated gameplay behavior.

---

## Project Documentation

Documentation is maintained in the `Documentation` directory. Principal documents include:

- `README.md`
- `CHANGELOG.md`
- `CodingStandards.md`
- `Architecture.md`
- `ADR.md`
- `DevelopmentJournal.md`
- `ROADMAP.md`
- `LICENSE.md`

Additional documents may be introduced as the project evolves.

---

## Roadmap

### Pack 1 — Base Vehicle Controller

The initial pack targets:

- Core architecture
- Vehicle controller
- Movement and steering
- Suspension
- Ground and obstacle detection
- Engine and surface audio
- Basic visual effects

### Camera Module

The initial modular camera implementation is already present and tested in the demo. Future work includes further integration testing, documentation, packaging, and commercial release validation.

### Future Packs and Modules

Planned expansion areas include:

- Weapon systems
- AI systems
- Multiplayer support
- Mobile support
- Additional vehicle types
- Specialized vehicle modules

Future features are development goals, not guarantees for the initial release.

---

## Official Support Policy

The current baseline is **Unity 2022.3.62f3 LTS**. The updated framework has been tested in the development project, including controller testing and clean-project package import.

**Unity 2021.3.45f2 LTS** remains a support candidate pending compatibility validation.

**Unity 6 LTS** has not yet been validated.

Commercial compatibility guarantees will apply only to versions explicitly listed in release documentation after testing.

## Versioning

The project follows Semantic Versioning (SemVer).

Current development version: **0.1.0**.

Future version numbers will reflect development and release milestones.

## License

See `LICENSE.md` for licensing information.

## Author

**Edison Oliveira**
Independent Game Developer
**Inverso Studio**

---

© 2026 Inverso Studio. All rights reserved.
