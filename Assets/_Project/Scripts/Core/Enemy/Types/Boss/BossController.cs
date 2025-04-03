using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Core.Enemy.Types.Boss
{
    public class BossController : MonoBehaviour
    {
        // Prefab for the healing orb
        public GameObject healingOrbPrefab;
        
        // Flag to ensure only one orb is spawned
        private bool _orbSpawned;
        
        // Reference to the enemy's controller
        internal EnemyController EnemyController;
        
        [SerializeField] internal GameObject healBossVFX;

        void Awake()
        {
            EnemyController = GetComponent<EnemyController>();
        }
    
        void Update() {
            // Check if the enemy's health is below 25% and an orb hasn't been spawned yet
            if (EnemyController.currentHealth <= EnemyController.Stats.maxHealth * 0.25f && !_orbSpawned) {
                
                // Spawn a healing orb
                SpawnHealingOrb();
                
                // Set flag to prevent multiple spawns
                _orbSpawned = true;
            }
        }

        // Spawns a healing orb at a valid NavMesh position
        void SpawnHealingOrb() {
            
            // Get a valid spawn position
            Vector3 spawnPosition = GetRandomNavMeshPoint();
            
            // Ensure a valid position was found
            if (spawnPosition != Vector3.zero) 
            {
                Instantiate(healingOrbPrefab, spawnPosition, Quaternion.identity);
            } else {
                Debug.LogWarning("Failed to find a valid NavMesh point for Healing Orb!");
            }
        }

        // Finds a random valid position on the NavMesh near the boss
        Vector3 GetRandomNavMeshPoint() {
            
            // Generate a random point near the boss
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * 10f;
            NavMeshHit hit;

            // Check if the generated point is on the NavMesh
            if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas)) {
                return new Vector3(hit.position.x, 1f, hit.position.z);
            }

            // Return an invalid point if no valid position was found
            return Vector3.zero; 
        }
    }
}
