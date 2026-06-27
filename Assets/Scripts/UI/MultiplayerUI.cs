using UnityEngine;
using UnityEngine.UI;
using Photon.Pun2;
using TMPro;

namespace Game.UI
{
    /// <summary>
    /// Manages the multiplayer UI including lobby, room browser, and in-game HUD.
    /// </summary>
    public class MultiplayerUI : MonoBehaviour
    {
        [SerializeField] private GameObject lobbyPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private Button quickPlayButton;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button leaveButton;
        [SerializeField] private TextMeshProUGUI playerCountText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private InputField playerNameInput;

        private void Start()
        {
            SetupButtons();
            UpdateUI();
        }

        private void SetupButtons()
        {
            quickPlayButton.onClick.AddListener(OnQuickPlayClicked);
            createRoomButton.onClick.AddListener(OnCreateRoomClicked);
            leaveButton.onClick.AddListener(OnLeaveClicked);
        }

        private void OnQuickPlayClicked()
        {
            string playerName = playerNameInput.text;
            if (string.IsNullOrEmpty(playerName))
            {
                playerName = "Player" + Random.Range(1000, 9999);
            }
            
            PhotonNetwork.LocalPlayer.NickName = playerName;
            NetworkManager.Instance.JoinRandomRoom();
            
            Debug.Log($"[MultiplayerUI] Quick play as {playerName}");
        }

        private void OnCreateRoomClicked()
        {
            string roomName = "Room_" + Random.Range(1000, 9999);
            string playerName = playerNameInput.text;
            
            if (string.IsNullOrEmpty(playerName))
            {
                playerName = "Player" + Random.Range(1000, 9999);
            }
            
            PhotonNetwork.LocalPlayer.NickName = playerName;
            NetworkManager.Instance.CreatePrivateRoom(roomName);
            
            Debug.Log($"[MultiplayerUI] Created room {roomName}");
        }

        private void OnLeaveClicked()
        {
            NetworkManager.Instance.LeaveRoom();
            Debug.Log("[MultiplayerUI] Left room");
        }

        private void UpdateUI()
        {
            if (PhotonNetwork.InRoom)
            {
                lobbyPanel.SetActive(false);
                gamePanel.SetActive(true);
                playerCountText.text = $"Players: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
            }
            else
            {
                lobbyPanel.SetActive(true);
                gamePanel.SetActive(false);
            }
        }

        public void UpdateTimer(float timeRemaining)
        {
            int minutes = (int)(timeRemaining / 60);
            int seconds = (int)(timeRemaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        public void UpdateScore(int player1Score, int player2Score)
        {
            scoreText.text = $"{player1Score} - {player2Score}";
        }
    }
}
