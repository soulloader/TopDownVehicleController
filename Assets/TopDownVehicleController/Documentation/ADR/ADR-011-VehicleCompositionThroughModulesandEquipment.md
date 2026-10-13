# ADR-011 — Vehicle Composition Through Modules and Equipment

**Status:** Proposed

**Author:** Edison Oliveira

**Framework:** Top-Down Vehicle Controller – Arcade System

---

## Context

As the framework evolved, it became clear that a vehicle should not be defined by its visual model or by a fixed set of features.

Different vehicles may share the same movement systems while exposing different capabilities and equipment.

Examples:

* A Tank and a Leopard share tracked movement.
* A Jeep and a Centauro share wheeled movement.
* An Apache helicopter shares input, movement coordination and weapon systems.
* An Amphibious Tank changes its movement mode without changing its identity as a vehicle.

---

## Decision

The framework will distinguish three independent concepts:

### Vehicle

Represents the physical entity.

Examples:

* Tank
* Leopard
* Jeep
* Centauro
* Apache

---

### Modules

Modules extend the vehicle's capabilities.

Examples:

* Flight
* Amphibious
* Hover
* AI Driver
* Multiplayer

Modules may be added or removed without changing the vehicle architecture.

---

### Equipment

Equipment represents optional components installed on the vehicle.

Examples:

* Machine Gun
* Cannon
* Missile Launcher
* Rocket Launcher
* Smoke Launcher
* Radar

Equipment should remain independent from the vehicle implementation whenever possible.

---

## Motivation

Separating Vehicle, Modules and Equipment improves:

* Modularity
* Reusability
* Scalability
* Future extensibility

This architecture allows the same framework to support a wide range of arcade vehicles while keeping systems independent.

---

## Status 

Proposed.

This ADR represents a long-term architectural direction.

Implementation details will be defined in future development phases after the core framework has been completed.
