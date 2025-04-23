using System.Collections;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Core.Character.Hand_Controller;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Time_Stability_Meter
{
    public class TimeStabilityMeter : MonoBehaviour
    {
        public float TimeStability { get; private set; }
        public float TotalTimeStability => totalTimeStability;
        public static TimeStabilityMeter Instance { get; private set; }

        [SerializeField] private float totalTimeStability = 100f;
        [SerializeField] private float startingTimeStability = 100f;
        [SerializeField] private float decreaseRate = 0.01f;

        public bool PauseTimeStabilityMeter { get; set; }

#if UNITY_EDITOR
        [Header("Editor Only")] [SerializeField]
        private bool pauseTimeStability;
#endif

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            TimeStability = startingTimeStability;
            PauseTimeStabilityMeter = true;
        }

        private void OnEnable()
        {
            PlayerController.OnDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            PlayerController.OnDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath(PlayerController killingPlayer, PlayerController playerKilled, IHandItem itemKilledBy)
        {
            if (killingPlayer != LevelSceneController.Instance.Player)
                return;

            switch (itemKilledBy)
            {
                case RangedWeapon rangedWeapon:
                    TimeStability += rangedWeapon.Stats.TimeStabilityEffect;
                    TimeStability = Mathf.Clamp(TimeStability, 0, totalTimeStability);
                    break;
                case MeleeWeapon meleeWeapon:
                    TimeStability += meleeWeapon.Stats.TimeStabilityEffect;
                    TimeStability = Mathf.Clamp(TimeStability, 0, totalTimeStability);
                    break;
            }
        }

        private void Update()
        {
            Debug.Log("current TSM"+TimeStability);
#if UNITY_EDITOR
            if (pauseTimeStability)
                return;
#endif
            if (PauseTimeStabilityMeter)
                return;
            TimeStability -= decreaseRate * Time.deltaTime;
            TimeStability = Mathf.Clamp(TimeStability, 0, totalTimeStability);
            if (TimeStability <= 0)
            {
                TimeStability = 0;
                var player = LevelSceneController.Instance.Player;

                player.TakeDamage(new IDamageable.DamageInfo(player.CurrentHealth, null));
            }
        }
        
        public void DecreaseTSM(float amount) 
        {
            StartCoroutine(DecreaseTSMValue(amount));
            Debug.Log("boss is affecting TSM");
        }

        // Coroutine to heal the boss over time
        IEnumerator DecreaseTSMValue(float amount) {
            // Decrease TSM value
            TimeStability -= amount;
            TimeStability = Mathf.Clamp(TimeStability, 0, totalTimeStability);
                    
            // Wait for 2 second before reducing again
            yield return new WaitForSeconds(2f);
        }
    }
}