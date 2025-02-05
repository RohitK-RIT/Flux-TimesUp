using System;
using _Project.Scripts.Core.Backend.Scene_Control;
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
        public float InitialTimeStability => initialTimeStability;
        public static TimeStabilityMeter Instance { get; private set; }
        [SerializeField] private float initialTimeStability = 100f;
        [SerializeField] private float decreaseRate = 0.01f;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            TimeStability = initialTimeStability;
        }

        private void Start()
        {
            foreach (var enemy in LevelSceneController.Instance.enemies)
            {
                enemy.OnDeath += OnEnemyDeath;
            }
        }

        private void OnEnemyDeath(PlayerController killingPlayer, PlayerController playerKilled, Weapon weaponKilledBy)
        {
            if(killingPlayer != LevelSceneController.Instance.Player)
                return;
            
            switch (weaponKilledBy)
            {
                case RangedWeapon rangedWeapon:
                    TimeStability += rangedWeapon.Stats.TimeStabilityEffect;
                    break;
                case MeleeWeapon meleeWeapon:
                    TimeStability += meleeWeapon.Stats.TimeStabilityEffect;
                    break;
            }
        }

        private void Update()
        {
            TimeStability -= decreaseRate * Time.deltaTime;
            TimeStability = Mathf.Clamp(TimeStability, 0, initialTimeStability);
            if(TimeStability <= 0)
            {
                TimeStability = 0;
                var player = LevelSceneController.Instance.Player;
                player.TakeDamage(null, player.CurrentHealth);
            }
        }
    }
}