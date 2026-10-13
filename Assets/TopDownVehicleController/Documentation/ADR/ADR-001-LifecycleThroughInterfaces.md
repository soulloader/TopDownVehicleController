# ADR-001 — Lifecycle Through Interfaces

## Status

Accepted

## Date

2026-07-08

## Project

Top-Down Vehicle Controller - Arcade System

## Context

The framework requires a consistent way to control the lifecycle of independent systems.

Vehicle systems such as movement, suspension, audio and effects need a predictable initialization and execution flow.

Direct communication between systems would create strong dependencies and make future expansion difficult.

## Decision

Framework systems will participate in the application lifecycle through interfaces.

Initial lifecycle interfaces:

* IInitializable
* IUpdatable
* IFixedTickable
* ILateTickable
* IResettable
* IValidatable
* IDisposableSystem

Each interface represents a specific responsibility in the system lifecycle.

## Consequences

### Positive

* Systems remain independent.
* VehicleController does not depend on concrete implementations.
* New systems can be added without changing existing architecture.
* Testing and maintenance become easier.

### Negative

* The architecture requires additional abstraction.
* Small systems may have more structure than strictly necessary.

## Notes

The purpose of this decision is not to create unnecessary complexity.

The abstraction exists to preserve flexibility as the framework grows.

## Related Documents

* Architecture.md
* CodingStandards.md
* ADR-002-AutomaticSystemDiscovery.md

---

# End of ADR
