=====================================================
                 Inverso Studio
=====================================================

Top-Down Vehicle Controller – Arcade System

This document describes the development roadmap for the
Top-Down Vehicle Controller – Arcade System.

It contains both the current development milestones and the
long-term vision for future framework expansion.

Document:
# Roadmap

Version: 0.1.0

Author:
Edison Oliveira

Engine:
Unity 2021.3.45f1 LTS (Minimum Supported Version)
Compatible up to Unity 6 LTS

Language:
C#

=====================================================

## Version 0.1.0 - Foundation
### Current Product

The current release focuses on a modular Top-Down Vehicle Controller Framework.

Primary goals:

Modular Architecture
Arcade Vehicle Movement
Suspension System
Surface Reaction System
Audio System
Weapon System
Vehicle Configuration
Editor Tools

- [x] Project structure
- [x] Coding standards
- [x] Development journal
- [x] Architecture Decision Records
- [ ] Core lifecycle interfaces
- [ ] Vehicle interfaces
- [ ] VehicleContext
- [ ] VehicleController

---

## Version 0.2.0 - Vehicle Systems

- [ ] Movement System
- [ ] Suspension System
- [ ] Detection System
- [ ] Audio System
- [ ] Effects System

---
Version 1.0.0

First Public Release

• Complete Documentation

• Demo Project

• Vehicle Framework

• Editor Tools

• Stable API

• Asset Store Ready


## Version 0.3.0 - Demo

- [ ] Demo Scene
- [ ] Tank Prefab
- [ ] Documentation


## Long-Term Vision

The current architecture was intentionally designed to evolve without requiring changes to the Core.

Future modules may include:

• Combat Framework
• Inventory Framework
• Save Framework
• AI Framework
• Naval Module
• Aircraft Module
• Character Module

***********************************************************

## Future Expansion
Inverso Gameplay Framework

Core

├── Movement
├── Combat
├── Inventory
├── Save
├── AI
└── HUD

Modules

├── Vehicle
├── Naval
├── Aircraft
├── Hover
└── Character

These modules are not part of the current product scope.


Current Development Philosophy

Finish before expanding.

Every new module must build upon a stable Core.

The long-term vision must never compromise the quality of the current release.
-----------------------------------------------------
Top-Down Vehicle Controller – Arcade System
© 2026 Inverso Studio. All rights reserved.
-----------------------------------------------------