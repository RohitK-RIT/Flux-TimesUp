using System;

namespace _Project.Scripts.Core.Enemy.GroupEnemyBehavior
{
    public static class DataCollectionEvents
    {
        public static event Action OnRoomEntered;
        public static event Action OnPlayerAttacked;
        public static event Action OnPortalExited;
        public static event Action<string, float> OnEnemySpawned; // enemyID, timestamp
        public static event Action<string, float> OnEnemyDied;    // enemyID, lifespan

        public static void RoomEntered() => OnRoomEntered?.Invoke();
        public static void PlayerAttacked() => OnPlayerAttacked?.Invoke();
        public static void PortalExited() => OnPortalExited?.Invoke();
        public static event Action OnAbilityUsed;
        public static void AbilityUsed() => OnAbilityUsed?.Invoke();
        
        public static void EnemySpawned(string id, float spawnTime) => OnEnemySpawned?.Invoke(id, spawnTime);
        public static void EnemyDied(string id, float lifespan) => OnEnemyDied?.Invoke(id, lifespan);
        
    }
}
