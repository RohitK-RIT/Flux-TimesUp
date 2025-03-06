using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Enemy.FSM;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    internal EnemyInputController broadcasterEnemy = null;
    internal List<EnemyInputController> helperEnemies = new List<EnemyInputController>();
    private List<EnemyInputController> allEnemies = new List<EnemyInputController>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEnemy(EnemyInputController enemy)
    {
        enemy.SetDefaultRole();
    }

    public void DeregisterEnemy(EnemyInputController enemy)
    {
        if (enemy == broadcasterEnemy)
        {
            broadcasterEnemy = null; 
            AssignNewBroadcaster(); 
        }
        helperEnemies.Remove(enemy);
    }

    public void EnemyDetected(EnemyInputController enemy, Vector3 playerPosition)
    {
        if (broadcasterEnemy == null) 
        {
            broadcasterEnemy = enemy;
            enemy.memberType = MemberType.Broadcaster;
        }
        BroadcastEnemyDetected(playerPosition);
    }

    private void BroadcastEnemyDetected(Vector3 playerPosition)
    {
        List<EnemyInputController> allEnemies = FindObjectsOfType<EnemyInputController>().ToList();

        List<EnemyInputController> potentialHelpers = EnemyManager.Instance.GetEligibleHelpers(broadcasterEnemy, playerPosition);
        // Filter out enemies outside engagement distance
        // List<EnemyInputController> potentialHelpers = allEnemies
        //     .Where(e => Vector3.Distance(e.transform.position, playerPosition) <= e.engagementDistance)
        //     .OrderBy(e => Vector3.Distance(e.transform.position, playerPosition))
        //     .Take(3) // Only take the 3 closest
        //     .ToList();

        helperEnemies.Clear();
        ClearRoles();
        helperEnemies.AddRange(potentialHelpers);

        // foreach (var enemy in allEnemies)
        // {
        //     enemy.OnEnemyDetected(playerPosition);
        // }
        
        foreach (var enemy in helperEnemies)
        {
            enemy.OnEnemyDetected(playerPosition);
            enemy.EngagePlayer();
        }
    }
    
    

    private void AssignNewBroadcaster()
    {
        if (helperEnemies.Count > 0)
        {
            broadcasterEnemy = helperEnemies[0];
            helperEnemies.RemoveAt(0); 
            BroadcastEnemyDetected(broadcasterEnemy.transform.position); // Rebroadcast the alert
        }
    }
    
    public void AssignHelper(EnemyInputController enemy)
    {
        // if (helperEnemies.Count < 3 && !helperEnemies.Contains(enemy))
        // {
        //     helperEnemies.Add(enemy);
        //     enemy.memberType = MemberType.Helper;
        // }
        
        enemy.memberType = MemberType.Helper;
    }
    
    public List<EnemyInputController> GetEligibleHelpers(EnemyInputController broadcaster, Vector3 playerPosition)
    {
        // Filter out enemies outside engagement distance
        List<EnemyInputController> potentialHelpers = allEnemies
            .Where(e => e != broadcaster && e.memberType == MemberType.Standalone)
            .Where(e => Vector3.Distance(e.transform.position, playerPosition) <= e.engagementDistance)
            .Where(e => e.EnemyHUD.enemy.CurrentHealth >= e.attackHealthThreshold)
            .OrderBy(e => Vector3.Distance(e.transform.position, playerPosition))
            .Take(3) // Only take the 3 closest
            .ToList();
        
        return potentialHelpers;
    }
    
    public void ClearRoles()
    {
        foreach (var enemy in allEnemies)
        {
            if (enemy.memberType == MemberType.Helper)
            {
                enemy.SetDefaultRole();
            }
        }
    }
}
