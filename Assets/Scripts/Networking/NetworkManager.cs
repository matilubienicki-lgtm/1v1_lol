using UnityEngine;
using Photon.Pun2;
using Photon.Realtime;

namespace Game.Networking
{
    /// <summary>
    /// Main networking manager for multiplayer gameplay.
    /// Handles player connections, room management, and game synchronization.
    /// </summary>
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager Instance { get; private set; }
        
        [SerializeField] private string gameVersion = "1.0";
        [SerializeField] private int maxPlayersPerRoom = 2;
        
        private bool isConnecting = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ConnectToPhoton();
        }

        /// <summary>
        /// Initiates connection to Photon servers.
        /// </summary>
        public void ConnectToPhoton()
        {
            if (isConnecting) return;
            
            isConnecting = true;
            
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            
            Debug.Log("[NetworkManager] Connecting to Photon...");
        }

        /// <summary>
        /// Creates or joins a matchmaking room.
        /// </summary>
        public void JoinRandomRoom()
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = (byte)maxPlayersPerRoom,
                IsVisible = true,
                IsOpen = true
            };
            
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("[NetworkManager] Joining random room...");
        }

        /// <summary>
        /// Creates a new private room for 1v1.
        /// </summary>
        public void CreatePrivateRoom(string roomName)
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = (byte)maxPlayersPerRoom,
                IsVisible = false,
                IsOpen = true
            };
            
            PhotonNetwork.CreateRoom(roomName, roomOptions);
            Debug.Log($"[NetworkManager] Creating room: {roomName}");
        }

        /// <summary>
        /// Leaves the current room.
        /// </summary>
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("[NetworkManager] Leaving room...");
        }

        // ===== Photon Callbacks =====
        
        public override void OnConnected()
        {
            Debug.Log("[NetworkManager] Connected to server.");
        }

        public override void OnConnectedToPhoton()
        {
            Debug.Log("[NetworkManager] Connected to Photon cloud.");
            isConnecting = false;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarning($"[NetworkManager] Disconnected: {cause}");
            isConnecting = false;
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"[NetworkManager] Joined room: {PhotonNetwork.CurrentRoom.Name}. Players: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"[NetworkManager] {newPlayer.NickName} joined the room.");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"[NetworkManager] {otherPlayer.NickName} left the room.");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"[NetworkManager] Failed to join room: {message}");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"[NetworkManager] Failed to create room: {message}");
        }
    }
}
