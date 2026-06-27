using UnityEngine;
using Photon.Pun2;
using TMPro;

namespace Game.UI
{
    /// <summary>
    /// Displays in-game HUD for player (health, ammo, score).
    /// </summary>
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI killsText;
        [SerializeField] private TextMeshProUGUI deathsText;
        [SerializeField] private Image healthBar;
        
        private PlayerHealth playerHealth;
        private PlayerStats playerStats;
        private float maxHealth = 100f;

        private void Start()
        {
            // Find local player
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                PhotonView pv = player.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    playerHealth = player.GetComponent<PlayerHealth>();
                    playerStats = player.GetComponent<PlayerStats>();
                    maxHealth = playerHealth.GetMaxHealth();
                    break;
                }
            }
        }

        private void Update()
        {
            if (playerHealth == null || playerStats == null)
                return;

            UpdateHealthDisplay();
            UpdateStatsDisplay();
        }

        private void UpdateHealthDisplay()
        {
            float health = playerHealth.GetHealth();
            healthText.text = $"Health: {health:F0}/{maxHealth:F0}";
            
            if (healthBar != null)
            {
                healthBar.fillAmount = health / maxHealth;
                // Change color based on health
                if (health > 60)
                    healthBar.color = Color.green;
                else if (health > 30)
                    healthBar.color = Color.yellow;
                else
                    healthBar.color = Color.red;
            }
        }

        private void UpdateStatsDisplay()
        {
            killsText.text = $"Kills: {playerStats.GetKills()}";
            deathsText.text = $"Deaths: {playerStats.GetDeaths()}";
            scoreText.text = $"Score: {playerStats.GetScore()}";
        }
    }
}
