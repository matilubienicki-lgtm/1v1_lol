using UnityEngine;
using Photon.Pun2;
using TMPro;

namespace Game.UI
{
    /// <summary>
    /// Displays match information (timer, both players stats, match status).
    /// </summary>
    public class MatchHUD : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI player1NameText;
        [SerializeField] private TextMeshProUGUI player2NameText;
        [SerializeField] private TextMeshProUGUI player1KillsText;
        [SerializeField] private TextMeshProUGUI player2KillsText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private float roundDuration = 300f;
        
        private float timeRemaining;
        private bool matchActive = false;

        private void Start()
        {
            timeRemaining = roundDuration;
            matchActive = true;
            
            if (PhotonNetwork.CurrentRoom != null)
            {
                var players = PhotonNetwork.CurrentRoom.Players;
                if (players.Count >= 1)
                {
                    player1NameText.text = players[0].NickName ?? "Player 1";
                }
                if (players.Count >= 2)
                {
                    player2NameText.text = players[1].NickName ?? "Player 2";
                }
            }
        }

        private void Update()
        {
            if (!matchActive)
                return;

            timeRemaining -= Time.deltaTime;
            
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                matchActive = false;
                statusText.text = "MATCH ENDED";
                statusText.color = Color.yellow;
            }

            UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay()
        {
            int minutes = (int)(timeRemaining / 60);
            int seconds = (int)(timeRemaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
            
            // Change color when time is running out
            if (timeRemaining < 30)
                timerText.color = Color.red;
            else if (timeRemaining < 60)
                timerText.color = Color.yellow;
            else
                timerText.color = Color.white;
        }

        public void UpdatePlayerStats(int playerIndex, string playerName, int kills)
        {
            if (playerIndex == 0)
            {
                player1NameText.text = playerName;
                player1KillsText.text = $"Kills: {kills}";
            }
            else if (playerIndex == 1)
            {
                player2NameText.text = playerName;
                player2KillsText.text = $"Kills: {kills}";
            }
        }
    }
}
