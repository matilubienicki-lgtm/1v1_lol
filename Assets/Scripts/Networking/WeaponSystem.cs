using UnityEngine;
using Photon.Pun2;

namespace Game.Networking
{
    /// <summary>
    /// Handles weapon firing, damage, and synchronization across the network.
    /// </summary>
    public class WeaponSystem : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float fireRate = 0.1f;
        [SerializeField] private float damage = 25f;
        [SerializeField] private float range = 100f;
        [SerializeField] private Transform shootPoint;
        
        private float lastFireTime = 0f;
        private Camera mainCamera;

        private void Start()
        {
            if (!photonView.IsMine)
            {
                enabled = false;
                return;
            }
            
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = GetComponentInChildren<Camera>();
            }
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            
            if (Input.GetMouseButton(0))
            {
                TryFire();
            }
        }

        /// <summary>
        /// Attempts to fire the weapon with rate limiting.
        /// </summary>
        private void TryFire()
        {
            if (Time.time - lastFireTime < fireRate)
                return;
            
            lastFireTime = Time.time;
            Fire();
        }

        /// <summary>
        /// Performs the actual firing and hit detection.
        /// </summary>
        private void Fire()
        {
            Vector3 shootDirection = mainCamera.transform.forward;
            
            if (Physics.Raycast(shootPoint.position, shootDirection, out RaycastHit hit, range))
            {
                // Hit detected
                if (hit.collider.TryGetComponent<PlayerController>(out var playerController))
                {
                    photonView.RPC(nameof(RPC_PlayerHit), RpcTarget.AllBuffered, hit.collider.gameObject.name, damage);
                }
            }
            
            // Send fire event to all players
            photonView.RPC(nameof(RPC_Fire), RpcTarget.AllBuffered, shootPoint.position, shootDirection);
        }

        // ===== RPC Methods =====
        
        [PunRPC]
        private void RPC_Fire(Vector3 position, Vector3 direction)
        {
            Debug.Log($"[WeaponSystem] RPC: Fire from {position} in direction {direction}");
            // Visual effects (muzzle flash, sound) would go here
        }

        [PunRPC]
        private void RPC_PlayerHit(string targetName, float damageAmount)
        {
            Debug.Log($"[WeaponSystem] RPC: {targetName} hit for {damageAmount} damage");
        }
    }
}
