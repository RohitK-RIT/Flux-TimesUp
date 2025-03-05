using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : MonoBehaviour
{
    public static GroupManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public Vector3 GetHelperPosition(Vector3 enemyPosition, Vector3 playerPosition)
    {
        // Flanking logic
        Vector3 directionToPlayer = (playerPosition - enemyPosition).normalized;
        Vector3 flankingDirection = Vector3.Cross(directionToPlayer, Vector3.up); // Perpendicular direction for flanking
        float flankDistance = 5f;

        return playerPosition + (flankingDirection * flankDistance);
    }
}
