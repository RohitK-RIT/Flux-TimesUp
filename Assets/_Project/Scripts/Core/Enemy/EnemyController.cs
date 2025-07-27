using _Project.Scripts.Core.Character.Hand_Controller;
using System;
using _Project.Scripts.Core.Enemy.FSM;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    [RequireComponent(typeof(EnemyInputController))]
    public class EnemyController : PlayerController
    {
        public override string FriendlyLayerName => "Enemy";
        public override string OpponentLayerName => "Player";
        
        private EnemyInputController _enemyInputController;

        protected override void Awake()
        {
            base.Awake();

            _enemyInputController = GetComponent<EnemyInputController>();
        }

        protected override void Start()
        {
            base.Start();

            _enemyInputController.Initialize(this);
            Reset();
        }

        private void OnEnable()
        {
            // Subscribe to attack input events on enable
            _enemyInputController.OnAttackInputBegan += BeginAttack;
            _enemyInputController.OnAttackInputEnded += EndAttack;
        }

        private void OnDisable()
        {
            // Subscribe to attack input events on disable
            _enemyInputController.Disable();
            _enemyInputController.OnAttackInputBegan -= BeginAttack;
            _enemyInputController.OnAttackInputEnded -= EndAttack;
        }

        public void Reset()
        {
            currentHealth = Stats.maxHealth;
            var currentRangedWeapon = HandController.CurrentItem as RangedWeapon;
            if (!currentRangedWeapon) return;
            currentRangedWeapon.InitializeAmo();
            Debug.Log("current amo" + currentRangedWeapon.CurrentAmmo);
            Debug.Log("max amo" + currentRangedWeapon.MaxAmmo);
            Debug.Log("Enemy reset to initial state.");
        }

        protected override void Die(PlayerController enemyPlayer, IHandItem itemKilledBy)
        {
            //_enemyInputController.gameObject.SetActive(false);
            base.Die(enemyPlayer, itemKilledBy);
        }
    }
}