using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public GameObject healingOrbPrefab;
    //public Transform roomBounds; // Area where the orb can spawn
    private bool orbSpawned = false;
    internal EnemyController _enemyController;

    void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
    }
    
    void Update() {
        //base.Update();
        if (_enemyController.currentHealth <= _enemyController.Stats.maxHealth * 0.25f && !orbSpawned) {
            SpawnHealingOrb();
            orbSpawned = true;
        }
    }

    void SpawnHealingOrb() {
        Vector3 spawnPosition = GetRandomNavMeshPoint();
        if (spawnPosition != Vector3.zero) { // Ensure a valid point was found
            Instantiate(healingOrbPrefab, spawnPosition, Quaternion.identity);
        } else {
            Debug.LogWarning("Failed to find a valid NavMesh point for Healing Orb!");
        }
    }

    Vector3 GetRandomNavMeshPoint() {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * 5f; // Generate a random point near the boss
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas)) {
            return new Vector3(hit.position.x, 1f, hit.position.z); // Return a valid NavMesh point
        }

        return Vector3.zero; // Return an invalid point if no valid position was found
    }
}
