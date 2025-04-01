using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy;
using UnityEngine;

public class BossController : EnemyController
{
    public GameObject healingOrbPrefab;
    public Transform roomBounds; // Area where the orb can spawn
    private bool orbSpawned = false;

    void Update() {
        base.Update();
        if (currentHealth <= Stats.maxHealth * 0.25f && !orbSpawned) {
            SpawnHealingOrb();
            orbSpawned = true;
        }
    }

    void SpawnHealingOrb() {
        Vector3 spawnPosition = GetRandomNavMeshPoint();
        Instantiate(healingOrbPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomNavMeshPoint() {
        // Implement NavMesh random point selection within roomBounds
        return transform.position + Random.insideUnitSphere * 5f; 
    }
}
