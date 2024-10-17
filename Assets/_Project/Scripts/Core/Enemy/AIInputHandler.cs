using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Enemy;
using UnityEngine;

public class AIInputHandler : MonoBehaviour
{
    private PlayerDetection _playerDetection; // Reference to the PlayerDetection component
    private CharacterMovement _characterMovement; // Reference to CharacterMovement for HandleLook and in future HandleMovement
    private EnemyAttack _enemyAttack; // Reference to EnemyAttack for handling attacks

    private void Awake()
    {
        // Get references to necessary components
        _playerDetection = GetComponent<PlayerDetection>();
        _characterMovement = GetComponent<CharacterMovement>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }
    
    void Update()
    {
        // Get the closest player detected by the PlayerDetection component
        Transform closestPlayer = _playerDetection.FindClosestPlayerInRange();
        
        // If a player is found and is within the conical view, rotate towards the player
        if (closestPlayer && _playerDetection.IsPlayerInCone(closestPlayer))
        {
            RotateTowardsPlayer(closestPlayer);
            Debug.Log("Rotating towards: " + closestPlayer.name);
            
            // Attempt to attack the player if within range
            TryAttack(closestPlayer);
        }
    }
    private void RotateTowardsPlayer(Transform player)
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;  // Keep rotation on the horizontal plane
            
        // Use the HandleLook method from CharacterMovement to rotate the enemy
        _characterMovement.HandleLook(directionToPlayer);
    }
    
    
    // Tries to attack the player if within range and not on cooldown.
    private void TryAttack(Transform player)
    {
        // Check if the player is within attack range
        if (_enemyAttack.IsPlayerInAttackRange(player))
        {
            Debug.Log("Player is within attack range.");

            // If cooldown is not active, start the attack and initiate cooldown
            if (!_enemyAttack.IsOnCooldown())
            {
                _enemyAttack.StartAttack();
                StartCoroutine(_enemyAttack.StartCooldown());
            }
            else
            {
                Debug.Log("Still on cooldown.");
            }
        }
        else
        {
            Debug.Log("Player is out of attack range.");
        }
    }
}
