using System;

namespace _Project.Scripts.Core.Enemy.GroupEnemyBehavior
{
    public static class DataCollectionEvents
    {
        public static event Action OnRoomEntered;
        public static event Action OnPlayerAttacked;
        public static event Action OnPortalExited;

        public static void RoomEntered() => OnRoomEntered?.Invoke();
        public static void PlayerAttacked() => OnPlayerAttacked?.Invoke();
        public static void PortalExited() => OnPortalExited?.Invoke();
    }
}
