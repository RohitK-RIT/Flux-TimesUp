using System.Collections.Generic;
using _Project.Scripts.Core.Backend.Scene_Control;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.GroupEnemyBehavior
{
    public class EnemyPerformanceTracker : MonoBehaviour
    {
        private float roomStartTime;
        private float roomEndTime;
        private bool isRoomTracking = false;

        private float gameStartTime;
        private float lastEnemyDeathTime;

        private int totalEnemiesKilled = 0;
        private float totalEnemyLifespan = 0f;

        private Dictionary<string, float> activeEnemies = new Dictionary<string, float>();

        void OnEnable()
        {
            DataCollectionEvents.OnEnemySpawned += TrackEnemySpawn;
            DataCollectionEvents.OnEnemyDied += TrackEnemyDeath;
            DataCollectionEvents.OnRoomEntered += StartRoomTimer;
            DataCollectionEvents.OnPortalExited += LogRoomPerformance;
        }

        void OnDisable()
        {
            DataCollectionEvents.OnEnemySpawned -= TrackEnemySpawn;
            DataCollectionEvents.OnEnemyDied -= TrackEnemyDeath;
            DataCollectionEvents.OnRoomEntered -= StartRoomTimer;
            DataCollectionEvents.OnPortalExited -= LogRoomPerformance;
        }

        void StartRoomTimer()
        {
            roomStartTime = Time.time;
            isRoomTracking = true;

            if (gameStartTime == 0)
            {
                gameStartTime = Time.time; // Start global timer at first room
            }
        }

        void TrackEnemySpawn(string enemyID, float spawnTime)
        {
            if (!activeEnemies.ContainsKey(enemyID))
            {
                activeEnemies.Add(enemyID, spawnTime);
            }
        }

        void TrackEnemyDeath(string enemyID, float lifespan)
        {
            totalEnemiesKilled++;
            totalEnemyLifespan += lifespan;

            if (activeEnemies.ContainsKey(enemyID))
            {
                float spawnTime = activeEnemies[enemyID];
                float timeOfDeath = Time.time;

                lastEnemyDeathTime = timeOfDeath;
                activeEnemies.Remove(enemyID);
            }

            if (activeEnemies.Count == 0 && isRoomTracking)
            {
                roomEndTime = Time.time;
                float currentPlayerHealth = LevelSceneController.Instance.Player.CurrentHealth;
                DataCollectionEvents.LastEnemyKilled_PlayerHealthSnapshot(currentPlayerHealth);
            }
        }

        void LogRoomPerformance()
        {
            if (isRoomTracking)
            {
                float roomClearTime = roomEndTime - roomStartTime;
                DataCollection.Instance.Log("EnemyPerformance", "RoomClearTime", roomClearTime);
            }

            float averageLifespan = totalEnemiesKilled > 0 ? totalEnemyLifespan / totalEnemiesKilled : 0f;
            DataCollection.Instance.Log("EnemyPerformance", "AverageEnemyLifespan", averageLifespan, $"Total:{totalEnemiesKilled}");

            float gameClearTime = lastEnemyDeathTime - gameStartTime;
            DataCollection.Instance.Log("EnemyPerformance", "TotalEnemyDefeatTime", gameClearTime);

            isRoomTracking = false;
            activeEnemies.Clear(); // reset for new room
        }
    }
}