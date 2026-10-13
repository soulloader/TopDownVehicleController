# Architecture Decision Records (ADR)

## Project

**Top-Down Vehicle Controller - Arcade System**

**Author:** Edison Oliveira

**Architecture Type:** Modular Framework

**Minimum Supported Version:** Unity 2021.3.45f1 LTS

**Target Versions:** Unity 2021 LTS through Unity 6 LTS

---

# Purpose

This document is the index of Architecture Decision Records created during the development of the framework.

Each ADR documents an important architectural decision, explaining:

* The context behind the decision.
* The chosen solution.
* The consequences of that decision.
* The impact on future development.

Architecture decisions are recorded individually and should not be modified after being accepted.

If a new direction is required, a new ADR must be created.

---

# ADR Organization

Each decision is stored in a separate document following this naming convention:

```
ADR-XXX-DecisionName.md
```

Example:

```
ADR-001-LifecycleThroughInterfaces.md
```

---

# Accepted Decisions

## ADR-001 — Lifecycle Through Interfaces

**Status:** Accepted

Framework systems participate in the application lifecycle through interfaces instead of direct dependencies.

---

## ADR-002 — Automatic System Discovery

**Status:** Accepted

VehicleController automatically discovers compatible framework systems.

---

## ADR-003 — IVehicleSystem Marker Interface

**Status:** Accepted

All vehicle systems implement a common identification contract.

---

## ADR-004 — VehicleController Responsibilities

**Status:** Accepted

VehicleController acts only as an orchestrator.

Gameplay logic must remain inside specialized systems.

---

## ADR-005 — Core Independence

**Status:** Accepted

Core must remain independent from gameplay modules.

---

## ADR-006 — VehicleContext

**Status:** Accepted

Shared vehicle references and data are provided through VehicleContext.

---

## ADR-007 — System Communication

**Status:** Accepted

Systems communicate through contracts, events and shared data instead of direct dependencies.

---

## ADR-008 — Legacy Refactoring Strategy

**Status:** Accepted

The original Tank implementation will be used as the behavioral reference during framework migration.

---

## ADR-009 — Gameplay Feel Preservation

**Status:** Accepted

The framework architecture must preserve the original vehicle experience.

Refactoring must improve structure without sacrificing:

* Vehicle responsiveness.
* Suspension behavior.
* Weight perception.
* Player enjoyment.

The player's experience is the priority.


## ADR-010 — Framework Lifecycle Naming

Decision:

Initialize()
Update()
FixedTick()
LateTick()

---

ADR-011 — Vehicle Composition Through Modules and Equipment

# Future Decisions

New architectural decisions will be added as the framework evolves.

---

# End of Document
