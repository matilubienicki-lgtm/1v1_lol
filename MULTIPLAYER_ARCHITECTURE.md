# 1v1 LOL - Multiplayer Architecture

## Overview
This document outlines the architecture for the multiplayer networking system for 1v1 LOL, a competitive FPS game.

## Technology Stack
- **Networking Framework:** Photon PUN2 (Photon Unity Networking 2)
- **Physics Engine:** Unity Physics
- **UI Framework:** Unity UI (Canvas) + TextMesh Pro
- **Language:** C#

## Core Systems

### 1. NetworkManager
- **Purpose:** Central hub for all network operations
- **Responsibilities:**
  - Connects to Photon cloud servers
  - Manages room creation and joining
  - Handles player connections/disconnections
  - Manages game state transitions
- **Key Methods:**
  - `ConnectToPhoton()` - Initiates server connection
  - `JoinRandomRoom()` - Joins available room or creates new one
  - `CreatePrivateRoom(string roomName)` - Creates private 1v1 room
  - `LeaveRoom()` - Safely leaves current room

### 2. PlayerController
- **Purpose:** Handles player character movement and synchronization
- **Responsibilities:**
  - Processes local player input
  - Handles character physics (movement, jumping, gravity)
  - Synchronizes position/rotation across network
  - Interpolates remote player movement
- **Key Methods:**
  - `HandleInput()` - Processes WASD, Space, Mouse input
  - `HandleMovement()` - Applies physics and movement
  - `OnPhotonSerializeView()` - Network synchronization callback

### 3. GameManager
- **Purpose:** Manages game flow and round state
- **Responsibilities:**
  - Controls game states (Waiting, PreRound, Playing, PostRound, Ended)
  - Manages round timers
  - Tracks player scores
  - Broadcasts game events to all players
- **Game States:**
  - **Waiting:** Players connecting
  - **PreRound:** 5-second countdown before round starts
  - **Playing:** Active gameplay (5 minutes)
  - **PostRound:** Round ended, preparing for next
  - **Ended:** Match concluded

### 4. WeaponSystem
- **Purpose:** Handles weapon firing and damage
- **Responsibilities:**
  - Fire rate limiting
  - Raycast-based hit detection
  - Synchronizes firing events
  - Communicates damage to target players
- **Key Methods:**
  - `TryFire()` - Rate-limited firing
  - `Fire()` - Performs raycast and hit detection
  - `RPC_Fire()` - Network firing event
  - `RPC_PlayerHit()` - Network damage event

### 5. MultiplayerUI
- **Purpose:** Manages all UI elements
- **Responsibilities:**
  - Lobby panel (quick play, create room)
  - In-game HUD (timer, score, player count)
  - Player name input
  - Room management buttons

## Network Data Flow

### Connection Flow
```
Game Start
  ↓
NetworkManager.ConnectToPhoton()
  ↓
Photon Servers
  ↓
JoinRandomRoom() OR CreatePrivateRoom()
  ↓
Room Joined (PhotonNetwork.InRoom = true)
  ↓
GameManager.StartPreRound()
  ↓
Gameplay
```

### Player Synchronization
```
Local Player Update
  ↓
OnPhotonSerializeView (Writing)
  ↓
Position, Rotation, Velocity → Photon
  ↓
Photon Cloud
  ↓
Remote Client Receives
  ↓
OnPhotonSerializeView (Reading)
  ↓
Interpolate to Network Position
```

### Weapon Fire Flow
```
Local Player Input (Mouse Click)
  ↓
WeaponSystem.TryFire()
  ↓
Raycast Hit Detection
  ↓
RPC_Fire() → All Players (visual)
  ↓
RPC_PlayerHit() → Damage Target
  ↓
GameManager.RecordKill() → Update Score
```

## RPC (Remote Procedure Call) Methods

RPCs are used for immediate game events that don't require continuous synchronization:

| RPC | Target | Purpose | Parameters |
|-----|--------|---------|------------|
| `RPC_StartPreRound` | All | Notify round starting | None |
| `RPC_StartRound` | All | Notify gameplay active | None |
| `RPC_EndRound` | All | Notify round ended | None |
| `RPC_UpdateScore` | All | Update player score | playerIndex, score |
| `RPC_Fire` | All | Play firing effects | position, direction |
| `RPC_PlayerHit` | All | Apply hit effects | targetName, damage |

## File Structure

```
Assets/
├── Scripts/
│   ├── Networking/
│   │   ├── NetworkManager.cs      # Connection & room management
│   │   ├── PlayerController.cs    # Player movement & sync
│   │   ├── GameManager.cs         # Game flow & scoring
│   │   ├── WeaponSystem.cs        # Weapon & hit detection
│   │   └── PlayerHealth.cs        # Health & damage system
│   └── UI/
│       └── MultiplayerUI.cs       # UI management
├── Scenes/
│   ├── Lobby.unity
│   └── Arena.unity
└── Prefabs/
    ├── Player.prefab
    └── Weapon.prefab
```

## Setup Instructions

### Prerequisites
1. Unity 2020 LTS or newer
2. Photon PUN2 asset from Unity Asset Store
3. Photon account with App ID

### Initial Setup
1. Import Photon PUN2 from Asset Store
2. Create Photon account at https://www.photonengine.com/
3. Create application and copy App ID
4. In Unity: Window → Photon Unity Networking → Highlight Settings
5. Paste App ID

### Scene Setup
1. Create NetworkManager prefab (add to both Lobby and Arena scenes)
2. Attach `NetworkManager.cs` script
3. Attach `GameManager.cs` script
4. Create player prefab with:
   - CharacterController component
   - PhotonTransformView component
   - `PlayerController.cs` script
   - `WeaponSystem.cs` script
5. Create UI with `MultiplayerUI.cs` script

## Performance Considerations

### Synchronization Rate
- **Position/Rotation:** ~20 packets/second (OnPhotonSerializeView)
- **Weapon Fire:** Immediate (RPC)
- **Score Update:** Immediate (RPC)

### Network Optimization
1. Only send data that has changed (delta compression)
2. Use interpolation to smooth remote player movement
3. Limit RPC calls for high-frequency events
4. Use interest management to reduce data sent

### Bandwidth Estimate
- Per player per second: ~2-5 KB
- 2 players × 300 seconds match: ~1.2-3 MB

## Future Enhancements

1. **Anti-Cheat:** Server-side validation of hits and scores
2. **Matchmaking:** ELO rating system
3. **Replays:** Record and playback matches
4. **Voice Chat:** In-game voice communication
5. **Cosmetics:** Skins, weapon appearances
6. **Spectating:** Watch ongoing matches
7. **Tournaments:** Bracket-based competitions
8. **Server Authority:** Move more logic server-side for security

## Debugging

### Common Issues

**Players not synchronizing:**
- Check PhotonNetwork.InRoom
- Verify OnPhotonSerializeView is called
- Check network send/receive rate in Photon settings

**High latency/lag:**
- Reduce update frequency if bandwidth limited
- Check interpolation factor
- Verify Photon region selection

**RPC not firing:**
- Ensure PhotonView is on the same GameObject
- Check RpcTarget (AllBuffered vs. All)
- Verify players are in same room

## References
- [Photon PUN2 Documentation](https://doc.photonengine.com/en-us/pun/v2)
- [Photon Scripting API](https://doc-api.photonengine.com/en/pun/v2/html/annotated.html)
- [Unity Networking Best Practices](https://docs.unity3d.com/Manual/BestPracticeGuides.html)
