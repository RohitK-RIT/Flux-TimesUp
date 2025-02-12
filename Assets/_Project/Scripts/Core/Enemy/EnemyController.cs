using _Project.Scripts.Core.Backend;
using _Project.Scripts.Core.Enemy.FSM;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Ranged;
using _Project.Scripts.Gameplay.PCG;
using Unity.Mathematics;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    [RequireComponent(typeof(EnemyInputController))]
    public class EnemyController : PlayerController
    {
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
            var currentRangedWeapon = WeaponController.CurrentWeapon as RangedWeapon;
            if (!currentRangedWeapon) return;
            currentRangedWeapon.AddAmmo(240);
            currentRangedWeapon.InitializeAmo();
            Debug.Log("current amo"+currentRangedWeapon.CurrentAmmo);
            Debug.Log("max amo"+currentRangedWeapon.MaxAmmo);
            Debug.Log("Enemy reset to initial state.");
        }

        protected override void Die(PlayerController enemyPlayer, Weapon weaponKilledBy)
        {
            gameObject.SetActive(false);
            base.Die(enemyPlayer, weaponKilledBy);
            
            // Spawn loot
            LootSpawner.Instance.LootDrop(_enemyInputController.Enemy.transform.position);
        }
    }
}