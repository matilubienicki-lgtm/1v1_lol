using UnityEngine;
using Photon.Pun2;
using Photon.Realtime;

namespace Game.Networking
{
    /// <summary>
    /// Manages game state, round management, and scoring in multiplayer mode.
    /// </summary>
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private float roundDuration = 300f; // 5 minutes
        [SerializeField] private float preRoundDuration = 5f;
        
        private float roundTimer;
        private GameState currentState = GameState.Waiting;
        private int[] playerScores = new int[2];
        
        public enum GameState
        {
            Waiting,
            PreRound,
            Playing,
            PostRound,
            Ended
        }

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
            if (PhotonNetwork.IsMasterClient)
            {
                StartPreRound();
            }
        }

        private void Update()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            switch (currentState)
            {
                case GameState.PreRound:
                    UpdatePreRound();
                    break;
                case GameState.Playing:
                    UpdatePlaying();
                    break;
            }
        }

        /// <summary>
        /// Starts the pre-round countdown.
        /// </summary>
        private void StartPreRound()
        {
            currentState = GameState.PreRound;
            roundTimer = preRoundDuration;
            photonView.RPC(nameof(RPC_StartPreRound), RpcTarget.AllBuffered);
            Debug.Log("[GameManager] Pre-round started.");
        }

        /// <summary>
        /// Updates pre-round timer and transitions to playing.
        /// </summary>
        private void UpdatePreRound()
        {
            roundTimer -= Time.deltaTime;
            
            if (roundTimer <= 0)
            {
                StartRound();
            }
        }

        /// <summary>
        /// Starts the actual round.
        /// </summary>
        private void StartRound()
        {
            currentState = GameState.Playing;
            roundTimer = roundDuration;
            photonView.RPC(nameof(RPC_StartRound), RpcTarget.AllBuffered);
            Debug.Log("[GameManager] Round started.");
        }

        /// <summary>
        /// Updates playing timer and checks for round end.
        /// </summary>
        private void UpdatePlaying()
        {
            roundTimer -= Time.deltaTime;
            
            if (roundTimer <= 0)
            {
                EndRound();
            }
        }

        /// <summary>
        /// Ends the current round and determines winner.
        /// </summary>
        private void EndRound()
        {
            currentState = GameState.PostRound;
            photonView.RPC(nameof(RPC_EndRound), RpcTarget.AllBuffered);
            Debug.Log("[GameManager] Round ended.");
        }

        /// <summary>
        /// Records a kill for a player.
        /// </summary>
        public void RecordKill(int playerIndex)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            playerScores[playerIndex]++;
            photonView.RPC(nameof(RPC_UpdateScore), RpcTarget.AllBuffered, playerIndex, playerScores[playerIndex]);
        }

        // ===== RPC Methods =====
        
        [PunRPC]
        private void RPC_StartPreRound()
        {
            Debug.Log("[GameManager] RPC: Pre-round starting");
        }

        [PunRPC]
        private void RPC_StartRound()
        {
            Debug.Log("[GameManager] RPC: Round starting");
        }

        [PunRPC]
        private void RPC_EndRound()
        {
            Debug.Log("[GameManager] RPC: Round ending");
        }

        [PunRPC]
        private void RPC_UpdateScore(int playerIndex, int score)
        {
            playerScores[playerIndex] = score;
            Debug.Log($"[GameManager] Player {playerIndex} score: {score}");
        }
    }
}
