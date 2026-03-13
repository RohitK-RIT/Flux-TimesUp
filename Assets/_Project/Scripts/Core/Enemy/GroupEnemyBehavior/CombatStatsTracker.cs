using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.GroupEnemyBehavior
{
    public class CombatStatsTracker : MonoBehaviour
    {
        private int totalAttacks = 0;
        private int roomsCompleted = 0;
        private int abilityUsage;

        void OnEnable()
        {
            DataCollectionEvents.OnPlayerAttacked += TrackAttack;
            DataCollectionEvents.OnPortalExited += HandleRoomComplete;
            DataCollectionEvents.OnAbilityUsed += TrackAbility;
        }

        void OnDisable()
        {
            DataCollectionEvents.OnPlayerAttacked -= TrackAttack;
            DataCollectionEvents.OnPortalExited -= HandleRoomComplete;
            DataCollectionEvents.OnAbilityUsed -= TrackAbility;
        }

        void TrackAttack()
        {
            totalAttacks++;
        }

        void TrackAbility()
        {
            abilityUsage++;
        }

        void HandleRoomComplete()
        {
            roomsCompleted++;

            // Log average attacks per room
            float avgAttacks = roomsCompleted > 0 ? (float)totalAttacks / roomsCompleted : 0;
            DataCollection.Instance.Log("Combat", "AvgAttacksPerRoom", avgAttacks, $"Rooms:{roomsCompleted}");

            // Log ability usage
            DataCollection.Instance.Log("Combat", $"AbilityUsed_", abilityUsage, $"Room:{roomsCompleted}");
        }
    }
}