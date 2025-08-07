using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.GroupEnemyBehavior
{
    public class PlayerPerformanceTracker : MonoBehaviour
    {
        private List<float> healthSnapshots = new List<float>();
        private int portalPassCount = 0;
        private int abilityUsage;

        void OnEnable()
        {
            DataCollectionEvents.OnLastEnemyKilled_PlayerHealthSnapshot += RecordHealth;
            DataCollectionEvents.OnPortalExited += HandlePortalExit;
            DataCollectionEvents.OnAbilityUsed += TrackAbility;
        }

        void OnDisable()
        {
            DataCollectionEvents.OnLastEnemyKilled_PlayerHealthSnapshot -= RecordHealth;
            DataCollectionEvents.OnPortalExited -= HandlePortalExit;
            DataCollectionEvents.OnAbilityUsed -= TrackAbility;
        }
        
        void RecordHealth(float health)
        {
            healthSnapshots.Add(health);
        }

        void HandlePortalExit()
        {
            portalPassCount++;

            // Log average health
            float avgHealth = healthSnapshots.Count > 0 ? Average(healthSnapshots) : 0f;
            DataCollection.Instance.Log("PlayerPerformance", "AverageHealth", avgHealth, $"Room:{portalPassCount}");

            // Log portal count
            DataCollection.Instance.Log("PlayerPerformance", "PortalsCrossed", portalPassCount);

            // Log ability usage per room
            DataCollection.Instance.Log("PlayerPerformance", $"AbilityUsed", abilityUsage, $"Room:{portalPassCount}");

            // Optional: Reset health snapshots/abilities per room
            healthSnapshots.Clear();
            abilityUsage=0;
        }

        void TrackAbility()
        {
            abilityUsage++;
        }

        float Average(List<float> list)
        {
            float sum = 0f;
            foreach (float f in list)
                sum += f;
            return sum / list.Count;
        }
    }
}