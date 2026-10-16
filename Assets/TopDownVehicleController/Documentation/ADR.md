# Architecture Decision Records (ADR)

## Project

**Top-Down Vehicle Controller – Arcade System**

**Studio:** Inverso Studio

**Author:** Edison Oliveira

**Architecture Type:** Modular Framework

**Official Development Version:** Unity 2022.3.62f3 LTS

**Minimum Supported Version Candidate:** Unity 2021.3.45f2 LTS

### Compatibility Status

| Unity Version | Status |
|---|---|
| Unity 2022.3.62f3 LTS | Validated |
| Unity 2021.3.45f2 LTS | Pending Validation |
| Unity 6 LTS | Not Yet Validated |

---

# Purpose

This document is the central index of Architecture Decision Records created during development of the framework.

Each ADR may document:

- The context behind a decision.
- The proposed or accepted solution.
- The consequences of that decision.
- The impact on future development.

Accepted architectural decisions should be preserved as historical records.

If an accepted decision must be replaced, a new ADR should document the change and reference the previous decision.

---

# ADR Organization

Individual ADR documents are stored in the `Documentation/ADR` directory.

Naming convention:

```text
ADR-XXX-DecisionName.md
```

An ADR may initially be recorded in this index before a dedicated document is created.

Missing individual files do not automatically invalidate decisions previously recorded in the index.

---

# ADR Status Definitions

**Proposed:** An architectural direction under consideration. Not yet accepted as a binding architectural decision.

**Accepted:** An approved architectural decision.

**Superseded:** A previously accepted decision replaced by a newer ADR.

**Status Not Explicitly Recorded:** The individual document does not formally specify its status.

---

# Architecture Decision Index

## ADR-001 — Lifecycle Through Interfaces

**Status:** Accepted

**Document:** `ADR/ADR-001-LifecycleThroughInterfaces.md`

Framework systems participate in the application lifecycle through interfaces instead of direct dependencies.

The individual document defines the initial lifecycle interfaces and records the consequences of this decision.

---

## ADR-002 — Automatic System Discovery

**Status:** Accepted (Index Record)

**Individual Document:** Not currently available

VehicleController automatically discovers compatible framework systems.

---

## ADR-003 — IVehicleSystem Marker Interface

**Status:** Accepted (Index Record)

**Individual Document:** Not currently available

All vehicle systems implement a common identification contract.

---

## ADR-004 — VehicleController Responsibilities

**Status:** Accepted (Index Record)

**Individual Document:** Not currently available

VehicleController acts only as an orchestrator.

Gameplay logic must remain inside specialized systems.

---

## ADR-005 — Core Independence

**Status:** Accepted (Index Record)

**Individual Document:** Not currently available

Core must remain independent from gameplay modules.

---

## ADR-006 — VehicleContext

**Status:** Accepted (Index Record)

**Individual Document:** Not currently available

Shared vehicle references and data are provided through VehicleContext.

---

## ADR-007 — System Communication

**Status:** Accepted (Index Record)

**Individual Document:** Not currently available

Systems communicate through contracts, events, and shared data instead of direct dependencies.

---

## ADR-008 — Legacy Refactoring Strategy

**Status:** Accepted (Index Record)

**Individual Document:** Not currently available

The original Tank implementation will be used as the behavioral reference during framework migration.

---

## ADR-009 — Gameplay Feel Preservation

**Status:** Accepted (Index Record)

**Individual Document:** Not currently available

The framework architecture must preserve the original vehicle experience.

Refactoring must improve structure without sacrificing:

- Vehicle responsiveness.
- Suspension behavior.
- Weight perception.
- Player enjoyment.

The player's experience is the priority.

---

## ADR-010 — Framework Lifecycle Naming

**Status:** Not Explicitly Recorded

**Individual Document:** Not currently available

The framework lifecycle uses the following method names:

```text
Initialize()
Update()
FixedTick()
LateTick()
```

---

## ADR-011 — Vehicle Composition Through Modules and Equipment

**Status:** Proposed

**Document:** `ADR/ADR-011-VehicleCompositionThroughModulesandEquipment.md`

The proposal distinguishes three independent concepts:

- **Vehicle:** Represents the physical entity.
- **Modules:** Extend vehicle capabilities.
- **Equipment:** Represents optional components installed on the vehicle.

This proposal aims to support different vehicle types and configurations while preserving modularity and reusability.

The document describes a long-term architectural direction.

Implementation details remain subject to future development and approval.

---

## ADR-015 — The VehicleController

**Status:** Not Explicitly Recorded

**Document:** `ADR/ADR-015-TheVehicleController.md`

The VehicleController must never contain gameplay logic.

Gameplay belongs exclusively to Systems.

This principle establishes a strict separation between orchestration and gameplay implementation.

---

# Missing Individual ADR Documents

The following decisions are currently recorded in this index without corresponding individual documents:

- ADR-002
- ADR-003
- ADR-004
- ADR-005
- ADR-006
- ADR-007
- ADR-008
- ADR-009
- ADR-010

These records should not be recreated from assumptions.

Individual documents may be prepared in future documentation work using verified architectural decisions.

---

# Unity Version Policy

The official development environment is **Unity 2022.3.62f3 LTS**.

Unity 2021.3.45f2 LTS remains a candidate for the minimum supported version, pending validation.

Unity 6 LTS has not yet been validated.

Unity version migrations do not automatically invalidate accepted architectural decisions.

Architectural changes required by future migrations must be documented through new ADRs when appropriate.

---

# Future Decisions

New architectural decisions will be added as the framework evolves.

Proposed decisions must not be treated as accepted until their status has been formally updated.

Accepted decisions should be preserved as historical records.

Any decision that supersedes an earlier one must reference the previous ADR.

---

# End of Document

Top-Down Vehicle Controller – Arcade System  
© 2026 Inverso Studio. All rights reserved.