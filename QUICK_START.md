# 🚀 QUICK START - 1v1 LOL Multiplayer

## ⚡ 5 MINUTES POUR TESTER

### AVANT TOUT - Pré-requis
✅ Unity 2020 LTS ou plus récent  
✅ Photon PUN2 importé  
✅ App ID configuré  

---

## ÉTAPE 1: Créer les dossiers

```
Assets/
├── Scripts/
│   ├── Networking/  ← Mettre tous les scripts .cs ici
│   └── UI/
├── Prefabs/
└── Scenes/
```

---

## ÉTAPE 2: Créer la scène Arena

1. File → New Scene → Save as "Arena"
2. Ajoute:
   - **Plane** (Ground) - Échelonner à (50, 1, 50)
   - **Cube** en hauteur (Spawn Point 1) - Position: (-10, 2, 0)
   - **Cube** en hauteur (Spawn Point 2) - Position: (10, 2, 0)
   - **Directional Light**
   - **Camera** (Main)

---

## ÉTAPE 3: Setup GameNetworkManager

1. GameObject → Create Empty → Rename "GameNetworkManager"
2. Add Components:
   - `NetworkManager.cs`
   - `GameManager.cs`
   - `SpawnManager.cs`
   - `PhotonView`

3. Assign Spawn Points:
   - Sélectionne les 2 Cubes (spawn points)
   - Drag them dans SpawnManager → Spawn Points

---

## ÉTAPE 4: Créer Player Prefab

1. Crée un Capsule: GameObject → 3D Object → Capsule
2. Rename → "Player"
3. Add Components:
   - `CharacterController`
   - `PhotonView` ⚠️ IMPORTANT
   - `PhotonTransformView`
   - `PlayerController.cs`
   - `PlayerHealth.cs`
   - `WeaponSystem.cs`
   - `PlayerStats.cs`

4. Child Objects:
   - Crée un objet vide "ShootPoint" (child du Player)
   - Position: (0, 0.3, 0.8)
   - Dans WeaponSystem, assign ShootPoint

5. Drag le Player dans Prefabs/ → "Player.prefab"

---

## ÉTAPE 5: Setup UI

1. Crée un Canvas: GameObject → UI → Panel
2. Ajoute du texte pour afficher:
   - Timer (Match duration)
   - Kills/Deaths
   - Health bar
   - Player names

3. Add script `MatchHUD.cs` et assign tous les texts

---

## ÉTAPE 6: TESTER! 🎮

### Test Local (2 joueurs sur le même PC)

1. **Build 1:**
   - File → Build Settings
   - Add Scene: Arena
   - Build And Run

2. **Build 2 (Optional):**
   - Dans l'éditeur Unity
   - Play button

3. **Gameplay:**
   ```
   Build 1:
   - Rentre "Player1"
   - Click "Quick Play"
   
   Build 2 (Unity Editor):
   - Rentre "Player2"
   - Click "Quick Play"
   
   ✅ Si les joueurs se voient = ÇA MARCHE!
   ```

---

## ✅ CHECKLIST FINALE

- [ ] Scripts dans les bons dossiers
- [ ] Photon configuré avec App ID
- [ ] Scène Arena créée
- [ ] GameNetworkManager setup
- [ ] Player Prefab créé avec tous les scripts
- [ ] Spawn points assignés
- [ ] UI créée avec MatchHUD
- [ ] 2 clients peuvent se connecter
- [ ] Les joueurs apparaissent à différents endroits
- [ ] Ils peuvent bouger et se voir
- [ ] Tirer tue l'adversaire
- [ ] UI montre kills/deaths

---

## 🐛 ERREURS COURANTES

### "Photon not found"
→ Photon PUN2 pas importé. File → Asset Store → Cherche PUN2

### "Players not visible"
→ PhotonView pas assigné sur Player prefab
→ Spawn points pas assignés à SpawnManager

### "Can't connect to room"
→ App ID invalide
→ Vérifier Window → Photon Settings

### "Lag énorme"
→ Normal pour une première build
→ Augmente la région dans Photon Settings

---

## 🎯 NEXT STEPS

Après avoir testé:
1. Ajouter des effets visuels (muzzle flash, hit effect)
2. Ajouter du son (tir, hit, ambiance)
3. Améliorer la UI
4. Balance du gameplay (dégâts, vitesse)
5. Système de construction (optionnel)

---

**Ready? Let's go! 🚀**
