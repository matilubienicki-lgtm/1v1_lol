# Multiplayer Setup Guide

## Quick Start - OÙ METTRE LES FICHIERS

### 📁 Structure de dossiers (où copier les fichiers)

```
Votre Projet Unity/
├── Assets/
│   ├── Scripts/
│   │   ├── Networking/  ← Créer ce dossier
│   │   │   ├── NetworkManager.cs
│   │   │   ├── PlayerController.cs
│   │   │   ├── GameManager.cs
│   │   │   ├── WeaponSystem.cs
│   │   │   └── PlayerHealth.cs
│   │   └── UI/  ← Créer ce dossier
│   │       └── MultiplayerUI.cs
│   ├── Scenes/
│   ├── Prefabs/
│   └── ...
```

### Étape 1: Créer les dossiers
1. Dans Unity, ouvre le dossier `Assets`
2. Crée un dossier `Scripts` (s'il n'existe pas)
3. Dans `Scripts`, crée un dossier `Networking`
4. Dans `Scripts`, crée un dossier `UI`

### Étape 2: Copier les fichiers C#
1. Télécharge les fichiers `.cs` depuis GitHub
2. Copie-les dans les bons dossiers:
   - `NetworkManager.cs` → `Assets/Scripts/Networking/`
   - `PlayerController.cs` → `Assets/Scripts/Networking/`
   - `GameManager.cs` → `Assets/Scripts/Networking/`
   - `WeaponSystem.cs` → `Assets/Scripts/Networking/`
   - `PlayerHealth.cs` → `Assets/Scripts/Networking/`
   - `MultiplayerUI.cs` → `Assets/Scripts/UI/`

### Étape 3: Importer Photon PUN2
1. Ouvre Unity → Window → Asset Store
2. Cherche "Photon PUN2"
3. Clique Download → Import
4. Accepte tous les dialogs
5. Attends la fin de l'import (2-3 minutes)

### Étape 4: Configurer Photon
1. Window → Photon Unity Networking → Highlight Settings
2. Va sur https://www.photonengine.com/
3. Crée un compte gratuit
4. Crée une nouvelle application
5. Copie l'App ID
6. Reviens à Unity, colle l'App ID dans Photon Settings

---

## COMMENT TESTER ❓

### Test 1: Vérifier que les scripts se chargent
1. Ouvre une scène Unity
2. Crée un objet vide: Clic droit → 3D Object → Cube
3. Dans l'Inspector (à droite), clique "Add Component"
4. Cherche "NetworkManager" et ajoute-le
5. Si c'est noir = ça marche ✅
6. Si c'est rouge = erreur, vérifie les dossiers

### Test 2: Vérifier la connexion Photon
1. Crée une nouvelle scène ou ouvre une existante
2. Crée un objet vide: GameObject → Create Empty
3. Ajoute le script `NetworkManager` à cet objet
4. Ajoute aussi un `PhotonView` (Window → Photon Unity Networking → PUN Wizard)
5. Lance le jeu (Play button)
6. Regarde la Console (Window → General → Console)
7. Tu devrais voir:
```
[NetworkManager] Connecting to Photon...
[NetworkManager] Connected to server.
[NetworkManager] Connected to Photon cloud.
```

### Test 3: Test multijoueur local
1. Build 1 (Game Build)
   - File → Build and Run
   - Choisis "Standalone PC"
   - Clique Build And Run
2. Dans le Game (Build 1):
   - Rentre un nom de joueur
   - Clique "Quick Play"
3. Reviens à Unity et clique Play (ou build une 2e version)
4. Dans Unity Play (Client 2):
   - Rentre un autre nom
   - Clique "Quick Play"
5. Si ça marche:
   - Les 2 joueurs apparaissent
   - Ils peuvent se voir bouger ✅

---

## STRUCTURE COMPLÈTE DE SCÈNE

### Scène "Lobby"
```
Lobby (Scene)
├── Canvas (UI)
│   ├── Panel (Lobby)
│   │   ├── Text: "1v1 LOL"
│   │   ├── InputField: playerNameInput
│   │   ├── Button: "Quick Play"
│   │   └── Button: "Create Room"
│   └── Panel (Game) - Désactivé par défaut
│       ├── Text: Timer
│       ├── Text: Score
│       └── Button: "Leave"
├── GameNetworkManager (Empty GameObject)
│   ├── NetworkManager.cs
│   ├── GameManager.cs
│   └── PhotonView
└── EventSystem
```

### Scène "Arena" (Gameplay)
```
Arena (Scene)
├── Ground (Plane)
├── Camera (Main)
├── Player (Prefab)
│   ├── CharacterController
│   ├── PhotonView
│   ├── PhotonTransformView
│   ├── PlayerController.cs
│   ├── PlayerHealth.cs
│   ├── WeaponSystem.cs
│   ├── Camera (child)
│   └── Weapon (child)
├── GameNetworkManager
│   ├── NetworkManager.cs
│   ├── GameManager.cs
│   └── PhotonView
└── Canvas (UI HUD)
    ├── Text: Timer
    ├── Text: Score
    └── Text: Player Count
```

---

## ERREURS COURANTES & SOLUTIONS

### ❌ "Assets cannot be loaded"
**Cause:** Mauvais chemin de dossier
**Solution:** Vérifie que les fichiers sont dans `Assets/Scripts/Networking/`

### ❌ "Type or namespace not found: Photon"
**Cause:** Photon PUN2 pas importé
**Solution:** 
1. Window → Asset Store
2. Cherche "Photon PUN2"
3. Télécharge et importe

### ❌ "Connection failed"
**Cause:** App ID invalide
**Solution:**
1. Window → Photon Unity Networking → Highlight Settings
2. Vérifie l'App ID (doit être un long numéro)
3. Crée un nouveau app si besoin

### ❌ "Players not visible in arena"
**Cause:** Player prefab pas configuré
**Solution:**
1. Crée un prefab "Player" avec tous les composants
2. Ajoute `PhotonView` et `PhotonTransformView`
3. Ajoute les scripts `PlayerController`, `PlayerHealth`, `WeaponSystem`

---

## CHECKLIST FINALE ✅

- [ ] Fichiers C# dans les bons dossiers
- [ ] Photon PUN2 importé
- [ ] App ID configuré dans Photon Settings
- [ ] Scène Lobby créée avec UI
- [ ] Scène Arena créée avec terrain
- [ ] Player Prefab créé avec tous les components
- [ ] Test local: 2 clients peuvent se connecter
- [ ] Les joueurs se voient et peuvent bouger
- [ ] Console montre les messages de connexion

---

## PROCHAINES ÉTAPES

1. **Ajouter les effets visuels:**
   - Muzzle flash (tir)
   - Hit marker (impact)
   - Blood spatter (sang)

2. **Ajouter les sons:**
   - Son de tir
   - Son de hit
   - Musique du lobby

3. **Améliorer le gameplay:**
   - Spawn points aléatoires
   - Armes différentes
   - Power-ups

4. **Optimiser:**
   - Réduire le lag
   - Améliorer la synchronisation
   - Anti-cheat

---

## RESSOURCES & DOCS

- **Photon Official:** https://www.photonengine.com/
- **PUN2 Docs:** https://doc.photonengine.com/en-us/pun/v2
- **Discord Photon:** https://discord.gg/photonengine
- **Reddit r/Unity3D:** Pour plus d'aide

---

**Besoin d'aide? Pose une question avec ton erreur exacte!** 🚀
