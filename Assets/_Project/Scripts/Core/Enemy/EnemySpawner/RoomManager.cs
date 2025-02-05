using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private List<Vector3> originalSpawnPoints = new List<Vector3>(); // Stores enemy spawn positions
    private List<GameObject> enemiesInRoom = new List<GameObject>(); // Stores references to original enemies

    private void Awake()
    {
        StoreEnemyPositions();
    }

    private void StoreEnemyPositions()
    {
        // Get all enemies that are children of the room prefab
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Enemy")) // Ensure enemies have the "Enemy" tag
            {
                originalSpawnPoints.Add(child.position);
                enemiesInRoom.Add(child.gameObject);
            }
        }

        Debug.Log($"Room '{gameObject.name}' initialized with {originalSpawnPoints.Count} enemies.");
    }

    private void Start()
    {
        Debug.Log("Spawn Points: " + string.Join(", ", GetSpawnPoints().ConvertAll(p => p.ToString())));
        Debug.Log("Enemies: " + string.Join(", ", GetEnemies().ConvertAll(e => e.name)));
    }

    public List<Vector3> GetSpawnPoints()
    {
        return originalSpawnPoints;
    }

    public List<GameObject> GetEnemies()
    {
        return enemiesInRoom;
    }
}
