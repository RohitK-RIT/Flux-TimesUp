using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private GameObject chargerPrefab;
    [SerializeField] private float spawnDistanceFromEnemy = 2f;  // Distance from enemy
    [SerializeField] private float spawnOffsetFromPlayer = 3f;   // Distance in front of the player
    
    internal void ChargerSpawner(Vector3 enemyPosition)
    {
        Debug.Log("Spawning charger");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("Player not found while Spawning!");
            return;
        }
        
        Vector3 playerForward = player.transform.forward.normalized; // Direction the player is facing
        Vector3 spawnCenter = player.transform.position + (playerForward * spawnOffsetFromPlayer); // In front of player
        
        // Calculate spawn positions around the enemy
        Vector3 spawnPos1 = enemyPosition + (Vector3.right * spawnDistanceFromEnemy);
        Vector3 spawnPos2 = enemyPosition + (Vector3.left * spawnDistanceFromEnemy);
        
        // Instantiate Chargers
        Instantiate(chargerPrefab, spawnPos1, Quaternion.identity);
        Instantiate(chargerPrefab, spawnPos2, Quaternion.identity);
    }
}
