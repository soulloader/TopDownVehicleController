# Development Journal

**Project:** Top-Down Vehicle Controller – Arcade System  
**Studio:** Inverso Studio  
**Author:** Edison Oliveira  
**Consolidated through:** 2026-10-28

> **Record provenance**
> - **[GIT ORIGINAL]**: Preserved from `Assets/TopDownVehicleController/Documentation/DevelopmentJournal.md`, added in commit `e7f05ac` on 2026-10-13. The original entry itself is dated 2026-07-07.
> - **[RECONSTRUCTED]**: Rebuilt from development discussions and technical decisions; not presented as verbatim Git history.
> - **[PLANNED]**: Proposed work, not confirmation of implementation.
>
## Development Principles

- **We design before we implement.**
- **Gameplay feel over unnecessary abstraction.**
- **Architecture serves creation. Creation serves fun.**
- Keep systems modular and responsibilities explicit.
- Preserve the enjoyable handling of the original Tank prototype while improving maintainability.

---

## 2026-07-07 — Architectural Foundation [GIT ORIGINAL]

**Original entry date:** 2026-07-07  
**Git provenance:** The journal was added to Git on 2026-10-13 in commit `e7f05ac`.

### Session overview

The session established the architectural foundation of the framework before the first production implementation. The goal was a scalable, modular, maintainable product suitable for future commercial releases on the Unity Asset Store.

**No production scripts were implemented during this session.** The work focused on architecture and development rules.

### Documentation recorded as completed

- `README.md`
- `CHANGELOG.md`
- `CodingStandards.md`

### Historical folder structure

The following is the structure **as recorded in the July entry**, not a claim about the current layout. The original box-drawing characters were corrupted during text export and have been normalized here.

```text
TopDownVehicleController/
├── Audio/
│   ├── Engine/
│   ├── Floor/
│   └── ScriptableObjects/
├── Core/
│   ├── Attributes/
│   ├── Data/
│   ├── Events/
│   ├── Extensions/
│   ├── Interfaces/
│   └── Utilities/
├── Data/
├── Demo/
│   ├── Materials/
│   ├── Prefabs/
│   ├── Scenes/
│   └── Scripts/
├── Documentation/
├── Editor/
├── Gizmos/
├── Input/
│   ├── Actions/
│   ├── Providers/
│   └── ScriptableObjects/
├── Models/
│   └── Tank/
│       ├── Materials/
│       ├── Meshes/
│       └── Textures/
├── Prefabs/
├── Settings/
└── Vehicle/
    ├── Controllers/
    ├── Data/
    ├── Interfaces/
    ├── ScriptableObjects/
    └── Systems/
        ├── Audio/
        ├── Detection/
        ├── Effects/
        ├── Movement/
        └── Suspension/
```

### Legacy code review

The original project was reviewed, including:

- `PlayerController`
- `TankInputs`
- `ObstacleDetector`
- `SurfaceData`
- `SurfaceType`
- `SurfaceAudio`
- `AudiosEngineTank`

**Architectural decision:** Build the framework through refactoring and extraction of reusable systems rather than rewriting the original project from scratch. The original `PlayerController` would gradually give way to independent systems. `VehicleController` would coordinate them instead of owning every responsibility.

### Design rule: “The 50-Line Rule”

The original journal introduced this as a discipline for questioning class responsibilities, not a verified automatic limit. Each interface should express one responsibility. When a class grows, ask: **“Does it still have only one responsibility?”** If not, separate its responsibilities.

### Next step recorded in the original entry

Begin implementing the Core interfaces.

---

## July–October 2026 — Core Architecture [RECONSTRUCTED]

> Exact implementation dates for the following intermediate milestones are not established by the recovered Git journal.

### Coordination and lifecycle

- `VehicleController` was designated as the central coordinator of independent vehicle systems.
- `VehicleContext` was designed to provide shared references such as `Transform`, `Rigidbody`, `Collider`, and `VehicleConfig` without requiring systems to discover these dependencies independently.
- `IVehicleSystem` and lifecycle-oriented interfaces were considered to keep the framework modular.
- **ADR-010 — Lifecycle Naming:** `Initialize()`, `Update()`, `FixedTick()`, and `LateTick()` were selected as lifecycle names in the architecture discussions. Verify the latest source files before asserting that every system uses all four methods.
- `VehicleConfig` was introduced as a configurable asset, with creation through the Unity asset menu confirmed in development discussions.

### Scope decision

**Pack 1** concentrates on the playable vehicle foundation: movement, suspension, ground/surface detection, obstacle detection, and engine audio. Camera and weapons functionality are outside the initial release scope.

---

## July–October 2026 — Vehicle Movement [RECONSTRUCTED]

The original Tank movement was adapted toward a standalone arcade vehicle system using Unity `Rigidbody` physics.

### Recorded design and behavior

- Distinct movement states: `Idle`, `Move`, and `Sprint`.
- Rigidbody-based movement with arcade responsiveness.
- Lateral velocity/grip management to avoid unwanted drifting.
- Rotation smoothing and controlled deceleration.
- Ground/surface modifiers and obstacle detection are designed to influence movement without merging their responsibilities into the movement class.
- A movement-state change event (`OnMovementStateChanged`) was discussed for communication with other systems.

### Recorded demonstration tuning

These are **development values**, not universal defaults or a guarantee of the current serialized configuration:

| Parameter | Recorded value |
|---|---:|
| Move speed | 6 |
| Sprint speed | 12 |
| Move force | 40 |
| Deceleration | 20 |
| Lateral grip | 20 |
| Rotation lerp speed | 12 |

**Validation note:** The movement feel was iteratively tested against the original Tank prototype. The exact current inspector values should be checked against the project before publication.

---

## July–October 2026 — Suspension [RECONSTRUCTED]

The vehicle suspension uses four wheel/contact sampling points and raycasts to provide arcade chassis response.

### Recorded behavior

- Four suspension sampling points: front-left, front-right, back-left, back-right.
- Spring and damper behavior.
- Ground contact evaluation and averaged ground normal for visual orientation.
- Front-edge / ramp-drop visual assistance to reduce unnatural chassis behavior at terrain transitions.
- Suspension behavior was tested and treated as a stable milestone before later audio and track work.

**Git evidence:** Commit `e7f05ac`, dated 2026-10-13, is titled `feat: complete vehicle suspension and ramp edge visual assist`. This commit title supports the suspension milestone, but does not by itself prove every detail above.

---

## July–October 2026 — Surface Detection [RECONSTRUCTED]

Surface detection was separated from suspension so terrain identification could be consumed by movement, audio, and later effects systems without duplicating detection logic.

### Recorded design

- `VehicleSurfaceDetector` uses downward detection to identify the active surface.
- `SurfaceData` and `SurfaceType` describe surface-specific behavior.
- The detector provides a surface-change notification (`OnSurfaceChanged`) for dependent systems.
- Detection should avoid treating the vehicle's own collider as the detected ground.
- Surface-dependent speed and audio behavior remain owned by their respective systems.

---

## 2026-10-27 — Engine Audio [RECONSTRUCTED]

`VehicleEngineAudio` was developed around **three dedicated looping audio sources**:

- Idle
- Move
- Sprint

### Recorded behavior

- Audio layers transition through volume crossfading.
- The system reacts to movement state rather than embedding engine audio logic in the movement implementation.
- Sprint audio is gated by movement/sprint conditions.

**Verification required:** Development notes differ on whether sprint applies in every movement direction or only while moving forward. The latest `VehicleEngineAudio.cs` and input-state logic must be checked before documenting either behavior as final.

---

## 2026-10-27 — Track Animation [RECONSTRUCTED]

`VehicleTrackAnimation` was developed and validated to animate the vehicle's left and right tracks independently.

### Recorded behavior

- UV scrolling along the Y axis.
- `MaterialPropertyBlock` to control material properties per renderer without unnecessary material duplication.
- Scrolling responds to actual vehicle velocity.
- Left/right track behavior differentiates turning.
- Recorded inversion configuration: left enabled, right disabled.

### Recorded demo references

- Renderers: `Left_Track`, `Right_Track`.
- Materials: `Treads_Tank_L`, `Treads_Tank_R`.

**Status recorded in discussions:** Tested and validated in the demonstration vehicle.

---

## 2026-10-28 — Surface Audio, Step 205.8.9 [RECONSTRUCTED]

`VehicleSurfaceAudio` was developed as a distinct system with its own `AudioSource`, using information from `VehicleSurfaceDetector` and `VehicleMovement`.

### Recorded behavior

- Surface-specific rolling audio selected from `SurfaceAudioConfig`.
- Audio stops while the vehicle is idle and loops while moving.
- Sprint adjusts pitch through a configurable multiplier.
- Pitch transitions are smoothed.
- The `Wood` surface was added for testing; enum additions were placed at the end to avoid changing earlier serialized enum indices.

### Recorded test configuration

| Setting | Recorded value |
|---|---:|
| Sprint pitch multiplier | 1.25 |
| Pitch transition duration | 0.05 seconds |
| Test surface | Wood |
| Test clip | `Surface_Wood_Rolling_Loop.wav` |
| Clip folder | `Assets/TopDownVehicleController/Audio/Floor/` |
| AudioSource volume | 1 |
| Base pitch | 1 |

**Status recorded in discussions:** Tested and validated for the Wood surface demonstration. Broader surface coverage should be verified separately.

---

## Consolidated Status as of 2026-10-28

| Area | Evidence-based status | Notes |
|---|---|---|
| Architectural foundation | Defined | Original July entry recovered from Git |
| Core coordination/context | Reconstructed implementation history | Verify latest code/API signatures |
| Vehicle movement | Developed and iteratively tested | Inspector tuning may have changed |
| Suspension | Milestone recorded in Git and discussions | Ramp-edge visual assist included |
| Surface detection | Developed | Independent from suspension |
| Engine audio | Developed | Sprint-direction rule needs verification |
| Track animation | Validated in demo discussions | Independent left/right UV behavior |
| Surface audio | Validated in Wood test discussions | Additional surfaces require separate checks |
| Commercial release | Not established as complete | Packaging, documentation, compatibility and release QA remain to be verified |

---

## Future Roadmap [PLANNED]

The following are **ideas or planned extensions**, not completed Pack 1 features:

- Additional surface profiles, rolling audio, and surface-dependent effects.
- Improved obstacle detection and debugging visualization.
- Weapon systems, targeting, and HUD functionality in a later pack.
- Optional camera module independent of the base movement package.
- Longer-term vehicle modules, including amphibious/naval and aerial behavior.

The initial commercial package should remain focused on a polished, configurable arcade vehicle foundation.

---

## Historical Reconciliation Notes

1. The original `DevelopmentJournal.md` contains a session dated **2026-07-07** and was first added to the inspected Git history in commit **`e7f05ac` on 2026-10-13**. These dates are not contradictory: one is the journal entry date and the other is the commit date.
2. The Git search discussed during recovery found no additional committed versions of this journal under the searched name. This does **not** exclude uncommitted local copies, other filenames, or external backups.
3. The original text included a pasted writing-block marker and corrupted box-drawing characters. The content has been preserved semantically, with the directory tree rendered in readable characters.
4. The July directory tree is a historical snapshot. Do not replace it with the current folder layout.
5. Reconstructed sections must remain clearly identified until verified against source code, Git diffs, or dated records.
6. Before a public release, reconcile the documented behavior against the current Unity project, particularly engine-audio sprint conditions and configuration defaults.

---

*Consolidated journal prepared from the recovered July entry and later development-session notes. It is not a verbatim transcription of every development session.*
