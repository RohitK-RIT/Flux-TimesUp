using UnityEngine;

namespace _Project.Scripts.Core.Enemy.GroupEnemyBehavior
{
    public class PlayerIdleTimer : MonoBehaviour
    {
        private float idleStartTime;
        private float lastAttackTime;
        private float totalIdleTime;
        private bool isIdle;
        private bool inRoom;

        private const float IdleThreshold = 2f;

        void OnEnable()
        {
            DataCollectionEvents.OnRoomEntered += StartIdleTracking;
            DataCollectionEvents.OnPlayerAttacked += HandleAttack;
            DataCollectionEvents.OnPortalExited += LogIdleTime;
        }

        void OnDisable()
        {
            DataCollectionEvents.OnRoomEntered -= StartIdleTracking;
            DataCollectionEvents.OnPlayerAttacked -= HandleAttack;
            DataCollectionEvents.OnPortalExited -= LogIdleTime;
        }

        void Update()
        {
            if (!inRoom) return;

            if (!isIdle && Time.time - lastAttackTime >= IdleThreshold)
            {
                StartIdleTimer();
            }
        }

        void StartIdleTracking()
        {
            inRoom = true;
            totalIdleTime = 0f;
            idleStartTime = Time.time;
            isIdle = true;
            lastAttackTime = Time.time; // assume idle until player attacks
        }

        void HandleAttack()
        {
            lastAttackTime = Time.time;

            if (isIdle)
            {
                totalIdleTime += Time.time - idleStartTime;
                isIdle = false;
            }
        }

        void StartIdleTimer()
        {
            idleStartTime = Time.time;
            isIdle = true;
        }

        void LogIdleTime()
        {
            if (isIdle)
            {
                totalIdleTime += Time.time - idleStartTime;
            }

            DataCollection.Instance.Log("PlayerEngagement", "TotalIdleTime", totalIdleTime);
            ResetState();
        }

        void ResetState()
        {
            totalIdleTime = 0f;
            isIdle = false;
            inRoom = false;
        }
    }
}
