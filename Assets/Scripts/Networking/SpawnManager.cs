using UnityEngine;
using Photon.Pun2;
using System.Collections.Generic;

namespace Game.Networking
{
    /// <summary>
    /// Manages player spawn points and respawning.
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance { get; private set; }
        
        [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
        [SerializeField] private float respawnDelay = 3f;
        
        private int lastSpawnIndex = -1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            if (spawnPoints.Count == 0)
            {
                Debug.LogWarning("[SpawnManager] No spawn points assigned!");
            }
        }

        /// <summary>
        /// Gets a spawn point, alternating between the two available points.
        /// </summary>
        public Vector3 GetSpawnPoint()
        {
            if (spawnPoints.Count == 0)
            {
                return Vector3.zero;
            }

            lastSpawnIndex = (lastSpawnIndex + 1) % spawnPoints.Count;
            return spawnPoints[lastSpawnIndex].position;
        }

        /// <summary>
        /// Gets a spawn point by index.
        /// </summary>
        public Vector3 GetSpawnPointByIndex(int index)
        {
            if (index < 0 || index >= spawnPoints.Count)
            {
                return Vector3.zero;
            }
            return spawnPoints[index].position;
        }

        /// <summary>
        /// Adds a spawn point to the list.
        /// </summary>
        public void AddSpawnPoint(Transform spawnPoint)
        {
            spawnPoints.Add(spawnPoint);
            Debug.Log($"[SpawnManager] Spawn point added. Total: {spawnPoints.Count}");
        }

        /// <summary>
        /// Gets the respawn delay.
        /// </summary>
        public float GetRespawnDelay() => respawnDelay;
    }
}
