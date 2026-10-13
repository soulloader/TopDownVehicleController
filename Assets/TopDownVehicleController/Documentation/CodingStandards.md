=====================================================
                 Inverso Studio
=====================================================

Top-Down Vehicle Controller – Arcade System

Document:
#CodingStandards


Version: 0.1.0

Author:
Edison Oliveira

Engine:
Unity 2021.3.45f1 LTS (Minimum Supported Version)
Compatible up to Unity 6 LTS

Language:
C#

=====================================================




Top-Down Vehicle Controller - Arcade System

Author: Edison Oliveira

Engine: Unity 6 LTS

Language: C#

Architecture: Modular Framework

Version: 0.1.0

Overview

Top-Down Vehicle Controller - Arcade System is a modular framework for Unity designed to create arcade-style top-down vehicle games.

The framework is built around independent systems, making it easy to maintain, extend, customize and integrate into different projects.

The goal is to provide a clean, scalable and production-ready architecture suitable for commercial game development and Asset Store distribution.

Vision

To provide a clean, modular and production-ready arcade vehicle framework for Unity that is easy to integrate, extend and maintain.

Project Philosophy
Modular architecture.
Single Responsibility Principle.
Reusable components.
Clean and maintainable code.
Easy to extend.
High readability.
Commercial asset quality.
Long-term scalability.
Folder Structure

The project is organized by modules instead of asset types.

Example:

TopDownVehicleController
│
├── Core
├── Vehicle
├── Input
├── Audio
├── Demo
└── Documentation

Example of the Vehicle module:

Vehicle
│
├── Controllers
├── Data
├── Models
├── Prefabs
└── Systems
Naming Convention
Folders
Use PascalCase.
Keep folders organized alphabetically whenever possible.
Organize by module rather than asset type.

Examples:

Controllers
Data
Models
Prefabs
Systems
Scripts
One public class per file.
File name must match the public class name.
Use PascalCase.

Examples:

VehicleController.cs
VehicleMovement.cs
VehicleSurfaceDetector.cs
VehicleSuspension.cs
VehicleEffects.cs
VehicleConfig.cs
Namespaces

Namespaces should follow the physical folder structure.

Example:

TopDownVehicleController.Vehicle.Controllers
TopDownVehicleController.Vehicle.Systems.Movement
TopDownVehicleController.Core
Architecture

The framework follows a modular architecture.

The VehicleController is responsible only for coordinating systems.

Business logic must remain inside the corresponding system.

Example:

VehicleController
        │
        ├── VehicleMovement
        ├── VehicleSuspension
        ├── VehicleSurfaceDetector
        └── VehicleEffects

Avoid placing all project logic inside the VehicleController.

Single Responsibility Principle

Each class should perform one responsibility only.

Good examples:

VehicleMovement
VehicleSuspension
VehicleSurfaceDetector
VehicleEffects

Bad example:

VehicleController containing movement, audio, suspension, particles, input and UI logic.
Code Organization
Keep classes focused.
Avoid duplicated code.
Prefer composition over inheritance whenever possible.
Separate gameplay logic from visual systems.
Keep systems independent.
Data

Configuration data should be separated from logic whenever appropriate.

Prefer ScriptableObjects for reusable configurations.

Examples:

VehicleConfig
SurfaceConfig
InputConfig
Documentation

Every public system should contain a brief XML summary explaining its responsibility.

Example:

/// Controls the vehicle movement system.

Complex methods should include comments only when necessary to explain intent.

Avoid obvious comments.

Future Updates

This document will evolve together with the framework.

Every important architectural decision should be documented here.

Design Decisions
2026-07-06
Framework organized by modules instead of asset types.
VehicleController acts only as the coordinator.
Systems follow the Single Responsibility Principle.
Folder names use PascalCase.
Folder structure follows alphabetical order.
Namespaces mirror the folder hierarchy.
Architecture designed for scalability and commercial Asset Store distribution.




**************************************************
Perfeito! Acho que demos um passo importante hoje.

Na prática, você deixou de ter apenas um projeto Unity e passou a ter um framework profissional com uma identidade definida.

Até agora construímos:

✅ Estrutura inicial das pastas.
✅ Arquitetura modular.
✅ Filosofia do projeto.
✅ Padrão de nomenclatura.
✅ Convenções de desenvolvimento.
✅ Primeiro documento oficial (CodingStandards.md).

O que faremos a seguir

Eu seguiria esta ordem:

Fase 1 – Fundação
✅ Estrutura de pastas
✅ Coding Standards
⏳ README.md
⏳ CHANGELOG.md
Fase 2 – Arquitetura
Criar os namespaces.
Definir os Assembly Definitions (.asmdef).
Criar a comunicação entre módulos.
Fase 3 – Core

Começar pelo núcleo do framework:

Core
├── Data
├── Interfaces
└── Utilities

Aqui nascerão as ferramentas que todos os outros módulos poderão utilizar.

Fase 4 – Vehicle

Criar o primeiro script do projeto:

Vehicle
└── Controllers
    └── VehicleController.cs

Mas ele será um orquestrador, não um script de milhares de linhas.

Fase 5 – Systems

Desenvolver um sistema de cada vez:

Movement

↓

Suspension

↓

Detection

↓

Effects

↓

Audio

Cada sistema será criado, testado e documentado antes de passarmos para o próximo.

Uma proposta para este projeto

Gostaria que adotássemos uma regra durante todo o desenvolvimento:

Nenhum script será escrito sem antes definir sua responsabilidade na arquitetura.

Ou seja, antes de criar um único .cs, responderemos perguntas como:

Qual é a responsabilidade desse script?
De qual módulo ele faz parte?
Quem pode utilizá-lo?
De quem ele depende?
Como ele se comunica com os outros sistemas?

Essa abordagem evita o acúmulo de "gambiarras" e ajuda a manter o framework limpo mesmo quando ele tiver centenas de scripts.

Pela forma como você está conduzindo o projeto, acredito que temos condições de construir um framework com qualidade suficiente para 
competir na Unity Asset Store, não apenas pela funcionalidade, mas também pela organização, documentação e arquitetura. 
Isso costuma ser um diferencial importante para quem compra um asset pensando em usá-lo em projetos reais.

-----------------------------------------------------
Top-Down Vehicle Controller – Arcade System
© 2026 Inverso Studio. All rights reserved.
-----------------------------------------------------