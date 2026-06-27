using UnityEngine;
using Photon.Pun2;

namespace Game.Networking
{
    /// <summary>
    /// Manages player health and death state across the network.
    /// </summary>
    public class PlayerHealth : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float maxHealth = 100f;
        
        private float currentHealth;
        private bool isDead = false;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        /// <summary>
        /// Takes damage and checks for death.
        /// </summary>
        public void TakeDamage(float damageAmount)
        {
            if (isDead) return;
            
            currentHealth -= damageAmount;
            photonView.RPC(nameof(RPC_TakeDamage), RpcTarget.AllBuffered, damageAmount, currentHealth);
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Handles player death.
        /// </summary>
        private void Die()
        {
            isDead = true;
            photonView.RPC(nameof(RPC_Die), RpcTarget.AllBuffered);
            Debug.Log($"[PlayerHealth] {photonView.Owner.NickName} died.");
        }

        /// <summary>
        /// Respawns the player with full health.
        /// </summary>
        public void Respawn()
        {
            if (!photonView.IsMine) return;
            
            currentHealth = maxHealth;
            isDead = false;
            photonView.RPC(nameof(RPC_Respawn), RpcTarget.AllBuffered);
            Debug.Log("[PlayerHealth] Respawning.");
        }

        public float GetHealth() => currentHealth;
        public float GetMaxHealth() => maxHealth;
        public bool IsDead() => isDead;

        // ===== RPC Methods =====
        
        [PunRPC]
        private void RPC_TakeDamage(float damage, float newHealth)
        {
            Debug.Log($"[PlayerHealth] {photonView.Owner.NickName} took {damage} damage. Health: {newHealth}/{maxHealth}");
        }

        [PunRPC]
        private void RPC_Die()
        {
            Debug.Log($"[PlayerHealth] {photonView.Owner.NickName} has been defeated.");
            gameObject.SetActive(false);
        }

        [PunRPC]
        private void RPC_Respawn()
        {
            Debug.Log($"[PlayerHealth] {photonView.Owner.NickName} respawned.");
            gameObject.SetActive(true);
        }
    }
}
