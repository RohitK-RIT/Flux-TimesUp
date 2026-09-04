<div align="center">

# War Flux

**A third-person action roguelite set in a shattered timeline — where the clock is your lifebar, and the wrong weapon costs you time.**

[![Play on itch.io](https://img.shields.io/badge/Play-itch.io-FA5C5C?logo=itchdotio&logoColor=white)](https://roosamozoomdar.itch.io/war-flux)
[![Gameplay video](https://img.shields.io/badge/Gameplay-video-FF0000?logo=youtube&logoColor=white)](https://youtu.be/DkL0jM5BdnE)
[![Unity 2022.3.38f1](https://img.shields.io/badge/Unity-2022.3.38f1-000000?logo=unity&logoColor=white)](https://unity.com/releases/editor/whats-new/2022.3.38)
[![HDRP 14](https://img.shields.io/badge/Render%20Pipeline-HDRP%2014.0.11-1f6feb)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@14.0/manual/index.html)
[![C#](https://img.shields.io/badge/C%23-140%20scripts-239120?logo=csharp&logoColor=white)](Assets/_Project/Scripts)
[![Commits](https://img.shields.io/badge/commits-1%2C060-informational)](../../commits/main)
[![No generative AI](https://img.shields.io/badge/No%20generative%20AI-used-6f42c1)](#credits)

*RIT MS Game Design & Development capstone · Team **Time's Up** · Fall 2024 – Spring 2025*

![War Flux gameplay](docs/media/hero.jpg)

<!-- Swap hero.jpg for a 10-15s gameplay GIF when you have one: meter draining mid-fight, a kill spiking it back, portal opens. -->

**[▶ Watch the gameplay video](https://youtu.be/DkL0jM5BdnE)** · **[⬇ Windows build](https://roosam.itch.io/warflux)** · **[⬇ macOS build](https://roosamozoomdar.itch.io/war-flux)**

</div>

---

## The hook

Most action games give you a health bar. **War Flux gives you a clock.**

A time-travel experiment has fractured the timeline, collapsing eras into one another — medieval castles overrun by goblins carrying modern firearms, trenches bleeding into the Wild West, sci-fi corridors where a keep should be. You play **Adam**, a Marine sent through a rift to find the source and shut it down.

Your **Time Stability Meter** drains from the moment you enter a room. Empty it and you die. Killing enemies is the only thing that puts time back — but **how much you get back depends on what you killed them with.**

That's the whole design in one line: **the strongest weapons repay you the least time.**

| Conventional shooter | War Flux |
|---|---|
| Taking cover is safe | Taking cover is a slow death |
| Health is a resource you spend | Time is a resource you **earn back by fighting** |
| Best weapon = most damage | Best weapon = **damage weighed against time returned** |
| Stalling is a valid tactic | Stalling destabilises the room — **the dead get back up** |

Let the meter slip and the world starts to fracture around you: enemies you already killed respawn, and wounded enemies call in reinforcements. The game is built to punish playing it safe.

## Play it

| Platform | Download |
|---|---|
| Windows | **[itch.io — Warflux](https://roosam.itch.io/warflux)** |
| macOS | **[itch.io — War Flux](https://roosamozoomdar.itch.io/war-flux)** (`War Flux.app.zip`, 290 MB) |

| Action | Input |
|---|---|
| Move | `W` `A` `S` `D` |
| Aim | Mouse |
| Fire / attack | Left Click |
| Aim down sights | Right Click |
| Dash / phase | `Shift` |

<table>
<tr>
<td width="50%"><img src="docs/media/shot-01-wildwest-combat.jpg" alt="Fighting through a Wild West street, stability meter draining"></td>
<td width="50%"><img src="docs/media/shot-02-portal.jpg" alt="Portal is now open - the room gate clears"></td>
</tr>
<tr>
<td width="50%"><img src="docs/media/shot-03-medieval-combat.jpg" alt="Ability VFX in a medieval castle arena"></td>
<td width="50%"><img src="docs/media/shot-04-scifi-arena.jpg" alt="Sci-fi era arena"></td>
</tr>
</table>

*If you know Enter the Gungeon, Risk of Rain 2, or Dead Cells, you already know the shape of a run — War Flux adds the clock.*

---

## Features

- **Time Stability Meter** — a continuously draining survival clock that doubles as the game's central risk/reward dial. Kills credit stability back through the killing weapon's `TimeStabilityEffect`, so era-appropriate armaments sustain you while advanced ones burn the timeline down faster than they refill it.
- **Reactive enemy population** — the spawner responds to how you play: enemies near death call in backup, and letting stability slip resurrects the ones you already put down. Designed explicitly to make defensive play unprofitable.
- **Anachronistic arenas across four eras** — Medieval, World War, Wild West, and Sci-Fi Future rooms, assembled per run from a prefab pool and destroyed on exit to keep memory flat.
- **Portal-gated progression** — clearing a room isn't enough. The exit portal opens only when the room is clear **and** your meter is topped, so the last kills of a fight have to be the efficient ones. Meet the condition and the boss arena replaces the room flow.
- **Ranged and melee loadouts with live switching** — independent stat assets, fire modes (auto, single, burst), and distinct stability yields; swap to melee when ammo runs dry or the room closes in.
- **Five cooldown-gated abilities** — Grenade, Heal, Shield, Teleport, and a **TSM Freeze** that stops the clock itself.
- **Modular enemy AI** — a per-agent state machine (Patrol → Detect → Chase → Attack → Flee → Death, plus a dedicated boss state) with group-level flanking so squads spread out instead of queueing up.
- **Show-don't-tell onboarding** — a tutorial level built around in-game video demonstrations against dummy targets, with progression gated on you actually performing each action.

---

## Architecture

Built for scalability and modularity, so four developers could add gameplay features in parallel without destabilising shipped ones.

```
Assets/_Project/Scripts/
├── Core/
│   ├── Backend/             # BaseSystem<T> singletons, scene control, currency, loot, interfaces
│   ├── Character/           # Movement, animation + IK, hand/loadout controllers, stats
│   ├── Player Controllers/  # Input abstraction, aim controller, local player
│   ├── Enemy/
│   │   ├── FSM/                 # StateManager + one class per enemy state
│   │   ├── GroupEnemyBehavior/  # Flanking, telemetry, group coordination
│   │   └── Types/               # Boss and charger variants
│   ├── Weapons/             # Ranged, melee, projectiles, fire modes, abilities
│   └── Loadout/             # Weapon data + slot management
├── Gameplay/
│   ├── PCG/                 # Grid-based dungeon generator (rooms + corridors)
│   ├── Revamp PCG/          # Room-pool arena generator driving shipped run flow
│   └── Time Stability Meter/
├── Onboarding/              # Tutorial flow, narrative + video-demo triggers
├── Audio/                   # Weapon / ability / room audio configs
└── UI/                      # Page-based UI system, HUD, menus
```

**Patterns in play**

| Pattern | Where | Why |
|---|---|---|
| **State** | [`StateManager`](Assets/_Project/Scripts/Core/Enemy/FSM/StateManager.cs) + one class per `EnemyState` | A new enemy behaviour is a new file, not another branch in a god-class |
| **Observer** | `PlayerController.OnDeath`, [`DataCollectionEvents`](Assets/_Project/Scripts/Core/Enemy/GroupEnemyBehavior/DataCollectionEvents.cs) | The stability meter credits time by *subscribing* to deaths — it never polls, and never reaches into combat code |
| **Generic singleton** | [`BaseSystem<T>`](Assets/_Project/Scripts/Core/Backend/BaseSystem.cs) | One audited implementation of instance + `DontDestroyOnLoad`, opted into per system via `IsPersistent` |
| **Interface segregation** | `IDamageable`, `IInteractable`, `IPickable`, `ICollectible`, `IHandItem` | Weapons, abilities and pickups interoperate without inheriting from one another |
| **ScriptableObject data** | `WeaponStats`, `AbilityStats` and their subclasses | Designers tune damage, cooldowns and **stability yield** as assets — no recompile, no code review to rebalance |

### How the core loop is wired

```
      ┌──────────────────────────────────────────────────┐
      │  TimeStabilityMeter (Update)                     │
      │    stability -= decreaseRate * deltaTime         │
      │    stability <= 0  ->  lethal damage to player   │
      └────────────────────┬─────────────────────────────┘
                           │ subscribes to
                           ▼
            PlayerController.OnDeath(killer, killed, weapon)
                           │
                  weapon.Stats.TimeStabilityEffect     <- the design lever:
                           │                              era-appropriate pays more
                           ▼
              stability += yield   (clamped to total)
                           │
          ┌────────────────┴────────────────┐
          │                                 │
   stability low                    room cleared AND meter full
          │                                 │
          ▼                                 ▼
  spawner resurrects the dead        portal opens -> next arena / boss
```

Two consumers read the same meter — the spawner and the room gate — which is what turns a timer into a design mechanic. Slow, cautious play drains the meter, the meter refills the room with enemies you thought you'd cleared, and the exit stays shut until you're both clear *and* topped up.

### Procedural generation

`RandomRoomGeneration` drives the run: pick a room prefab from the pool, instantiate, wait for `DungeonRoom` to report its wave cleared via `RoomWaveController`, show the portal, destroy the room on exit, repeat — until the clear-plus-full-meter condition swaps in `BossEnemyRoom`. Destroying each room on exit keeps memory flat regardless of run length. `CombatArenaCheck` starts and resumes the meter on arena entry, so the clock only runs when you're actually in a fight.

An earlier grid-based generator (`Gameplay/PCG`) with room-and-corridor placement is retained in the repo; the corridor system was never completed, and run flow shipped on the room-pool approach above.

---

## Tech stack

| | |
|---|---|
| **Engine** | Unity `2022.3.38f1` (LTS) |
| **Rendering** | High Definition Render Pipeline `14.0.11` — confirmed as the active pipeline in `GraphicsSettings`, with HDRP's volume framework handling post-processing |
| **Input** | Unity Input System `1.7.0` — a generated `PlayerInput` action asset driving an `InputController` abstraction |
| **Camera** | Cinemachine `2.10.1` — third-person rig follows a `cinemachineCameraTarget` driven by `PlayerAimController` |
| **Animation** | Animation Rigging `1.2.1` — four runtime IK rigs (body, gun hand, gun aiming, melee hand) for weapon handling; blend trees for locomotion |
| **Navigation** | Unity AI Navigation `1.1.5` — baked NavMesh, `NavMeshAgent` pathing driven by the enemy FSM |
| **UI** | uGUI + TextMeshPro `3.0.6`, page-stack UI manager |
| **Workflow** | Git LFS (4,074 tracked binaries), ParrelSync, `.gitattributes` tuned for Unity YAML merges, Jira, protected-branch rulesets |

---

## Build from source

**Prerequisites**

- Unity **2022.3.38f1** (exact version — the project is HDRP-pinned)
- [Git LFS](https://git-lfs.com) installed *before* cloning
- Disk: ~1.4 GB for the clone (289 MB of source + 1.1 GB of LFS binaries), plus several GB for Unity's generated `Library/` on first import

```bash
git lfs install
git clone https://github.com/RohitK-RIT/Flux-TimesUp.git
cd Flux-TimesUp
```

Open the folder in Unity Hub with 2022.3.38f1, then load a scene from `Assets/_Project/Scenes/`:

| Scene | What it is |
|---|---|
| `Title Scene` | Main menu — build index 0, the actual entry point |
| `Final Onboarding Scene` | Tutorial level that teaches the stability meter |
| `PCG-Level` | A procedurally generated run |
| `Test Scenes/` | Isolated harnesses for weapons, abilities, enemies, boss, IK, pathfinding (not in build settings) |

> **First import takes a while.** HDRP shader compilation against a cold library is normal.

---

## How it was built

Two semesters of coursework, 1,060 commits on `main` between September 2024 and March 2026, and eight rounds of external playtesting between October 2024 and April 2025. A few things the process is worth reading for:

**The onboarding was rebuilt twice, both times because playtesters told us to.** It started as text prompts driven by input events — and players skipped them. The second iteration wrapped each lesson in narrative delivered with a typewriter effect, then gated progression on performing the action. Advisor feedback cut to the problem: players were still being *told* what to do rather than *shown*. The third iteration replaced the text with short in-game video demonstrations against dummy targets, validated by action detection before you could advance. Same pedagogy each time, three very different implementations.

**Multiplayer was cut deliberately.** It was in scope early — the repo still carries the seams — and removing it bought the polish budget for the core loop. Scoping down was the decision that made the rest shippable.

**Branch discipline was enforced, not suggested.** A four-tier structure (`feature` → `sprint` → `dev` → `main`) with GitHub rulesets: `main` and `dev` protected behind unanimous team approval, sprint branches behind one. The two-approval rule on sprint branches was relaxed to one mid-project after it became an integration bottleneck — the kind of process tuning that only shows up once a team is actually running at speed. Every PR used a standard template linking back to a Jira issue.

---

## My role

I worked as **Producer and Lead Developer** across both semesters.

- **As producer** — held the timeline and scope to something that satisfied stakeholders and the team, kept a holistic view of the game so the parts stayed coherent, and managed workload deliberately to avoid crunch.
- **As lead developer** — owned the code structure and standards above: efficient, non-redundant, readable systems, plus code review and unblocking for the rest of the team.

I came in with 4+ years of professional Unity experience shipping mobile titles on Android, iOS and the Amazon Marketplace, covering gameplay logic, backend systems and Unity Editor tooling, and had previously led teams as both lead programmer and project manager.

The history is public and unsquashed — 1,060 commits — if you'd rather read the progression than the summary.

---

## Credits

Built by a cross-discipline team of graduate students at the **Rochester Institute of Technology**, MS Game Design & Development, under a faculty committee from the School of Interactive Games & Media. See the [contributors graph](../../graphs/contributors) for everyone who committed.

**No generative AI was used** in the production of this game.

### Third-party assets

This repository vendors licensed commercial art and VFX packages under `Assets/` — including **Synty Studios** POLYGON packs (Dungeon, Sci-Fi Worlds, War, Western) and **Hovl Studio** Magic Effects. The low-poly Synty style was a deliberate scope decision: it gave a cohesive look across four eras with no dedicated artist on the core team. These assets are covered by their own licenses, are **not** redistributable, and remain the property of their respective creators.

Those packs are marked `linguist-vendored`, so GitHub's language bar reflects the code this team actually wrote rather than the shaders that shipped with the art.

## License

No open-source license is granted. The source is published for portfolio and review purposes; the third-party assets above carry their own terms.
