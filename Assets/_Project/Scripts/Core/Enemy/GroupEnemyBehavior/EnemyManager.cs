using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.GroupEnemyBehavior
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance { get; private set; }

        internal EnemyInputController BroadcasterEnemy; // Enemy that calls for help
        internal List<EnemyInputController> HelperEnemies = new List<EnemyInputController>(); // List of helper enemies
        private List<EnemyInputController> _allEnemies = new List<EnemyInputController>(); // List of all enemies

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        // Method to register all the enemies on awake
        public void RegisterEnemy(EnemyInputController enemy)
        {
            enemy.SetDefaultRole();
        }

        // Method to deregister all enemies on enemy death
        public void DeregisterEnemy(EnemyInputController enemy)
        {
            if (enemy == BroadcasterEnemy)
            {
                BroadcasterEnemy = null; 
                AssignNewBroadcaster(); 
            }
            HelperEnemies.Remove(enemy);
        }

        //
        public void BroadcastMessage(EnemyInputController enemy, Vector3 playerPosition)
        {
            if (BroadcasterEnemy == null) 
            {
                BroadcasterEnemy = enemy;
                enemy.MemberType = MemberType.Broadcaster;
            }
            FindHelpers(playerPosition);
        }

        // Method to find helpers
        private void FindHelpers(Vector3 playerPosition)
        {
            _allEnemies = FindObjectsOfType<EnemyInputController>().ToList();

            List<EnemyInputController> potentialHelpers = EnemyManager.Instance.GetEligibleHelpers(BroadcasterEnemy, playerPosition);
            
            HelperEnemies.Clear();
            ClearRoles();
            HelperEnemies.AddRange(potentialHelpers);

            foreach (var enemy in HelperEnemies)
            {
                enemy.AssignRoles(playerPosition);
                enemy.EngagePlayer();
            }
        }
    
    
        // Assign new broadcaster on broadcaster death
        private void AssignNewBroadcaster()
        {
            if (HelperEnemies.Count > 0)
            {
                BroadcasterEnemy = HelperEnemies[0];
                HelperEnemies.RemoveAt(0); 
                FindHelpers(BroadcasterEnemy.transform.position); // Rebroadcast the alert
            }
        }
    
        // Method to assign helpers
        public void AssignHelper(EnemyInputController enemy)
        {
            enemy.MemberType = MemberType.Helper;
        }
    
        // Method to find potential helpers
        private List<EnemyInputController> GetEligibleHelpers(EnemyInputController broadcaster, Vector3 playerPosition)
        {
            // Filter out enemies outside engagement distance
            List<EnemyInputController> potentialHelpers = _allEnemies
                .Where(e => e != broadcaster && e.MemberType == MemberType.Standalone)
                .Where(e => Vector3.Distance(e.transform.position, playerPosition) <= e.EngagementDistance)
                .Where(e => e.EnemyHUD.enemy.CurrentHealth >= e.AttackHealthThreshold)
                .OrderBy(e => Vector3.Distance(e.transform.position, playerPosition))
                .Take(3) // Only take the 3 closest
                .ToList();
        
            return potentialHelpers;
        }
    
        // Method to clear helper roles once they are removed from the helper list
        private void ClearRoles()
        {
            foreach (var enemy in _allEnemies)
            {
                if (enemy.MemberType == MemberType.Helper)
                {
                    enemy.SetDefaultRole();
                }
            }
        }
    }
}
