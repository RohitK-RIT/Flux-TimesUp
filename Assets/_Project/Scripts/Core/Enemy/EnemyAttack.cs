using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Weapon<RangedWeaponStats> weapon; // Reference to the weapon
    [SerializeField] private float attackRange = 5f; // Attack range
    [SerializeField] private float attackCooldown = 5f; // Cooldown time between attacks

    private bool _isCooldownActive; // Tracks whether the enemy is in cooldown

    private float _lastAttackTime;
    private void Awake()
    {
        // Ensure a weapon is attached
        if (weapon == null)
        {
            weapon = GetComponentInChildren<Weapon<RangedWeaponStats>>();
        }
    }

    
    // Method to check if player is within the attack range.
    public bool IsPlayerInAttackRange(Transform player)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        var chk = distanceToPlayer <= attackRange;
        
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("IsPlayerInAttackRange:"+ chk);
            return true;
        }
        else
        {
            Debug.Log("IsPlayerInAttackRange:"+ chk);
            return false;
        }
    }

    
    // Start the attack if not on cooldown.
    public void StartAttack()
    {
        if (!_isCooldownActive)
        {
            Debug.Log("Attacking");

            _lastAttackTime = Time.time; // Record the time of the last attack
            weapon.BeginAttack();
        }
    }
    
    // Check if the cooldown has finished
    public bool IsOnCooldown()
    {
        return Time.time - _lastAttackTime < attackCooldown;
    }

    
    // Coroutine to handle attack cooldown.
    public IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        Debug.Log("Cooldown finished, ready to attack again.");
    }
}
