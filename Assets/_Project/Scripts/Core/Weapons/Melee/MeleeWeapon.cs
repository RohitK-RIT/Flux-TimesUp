using System;
using System.Collections;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Enemy.GroupEnemyBehavior;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Melee
{
    /// <summary>
    /// Melee weapon class.
    /// </summary>
    public sealed class MeleeWeapon : Weapon
    {
        public event Action OnAttackBegin;
        public override string WeaponID => stats.WeaponID;
        public override string DisplayName => stats.WeaponName;
        public MeleeWeaponStats Stats => stats;

        /// <summary>
        /// Is the weapon currently attacking.
        /// </summary>
        public bool IsAttacking => _attackCoroutine != null;

        /// <summary>
        /// Melee weapon stats.
        /// </summary>
        [SerializeField] private MeleeWeaponStats stats;

        private float _lastAttackTime = float.MinValue;
        private Coroutine _attackCoroutine;
        private AudioSource _shootingAudioSource;
        private AudioPlayer _audioPlayer;

        protected override void Awake()
        {
            base.Awake();

            _shootingAudioSource = GetComponent<AudioSource>();
            _audioPlayer = GetComponent<AudioPlayer>();
        }

        public override void OnCollected(PlayerController playerController)
        {
            if (playerController.HandController.Weapons[1])
                return;

            playerController.HandController.OnItemInteracted(this);
        }

        public override void BeginUse()
        {
            if (IsAttacking)
                return;

            _attackCoroutine = StartCoroutine(OnAttack());
        }

        public override void EndUse()
        {
            if (!IsAttacking)
                return;

            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }

        /// <summary>
        /// Coroutine for attacking.
        /// </summary>
        private IEnumerator OnAttack()
        {
            // Attack until the attack ends
            while (true)
            {
                // Wait for the attack speed and then fire the bullet.
                yield return new WaitUntil(() => Time.time - _lastAttackTime >= 1 / stats.AttackSpeed);

                if (Owner is LocalPlayerController)
                    Debug.Log($"Melee Check {Time.time} - {_lastAttackTime} = {Time.time - _lastAttackTime} >= {1 / stats.AttackSpeed}");

                OnAttackBegin?.Invoke();
                Invoke(nameof(Slash), 2.2f / (stats.AttackSpeed * 2f)); // Halfway through the animation
                // Slash();
                _lastAttackTime = Time.time;
            }
        }

        /// <summary>
        /// Do the attack.
        /// </summary>
        private void Slash()
        {
            DataCollectionEvents.PlayerAttacked();
            _audioPlayer.PlayAttackClip(_shootingAudioSource);
            // Check for enemies in the attack range
            var collidersFound = new Collider[20];

            int opponentMask = 1 << LayerMask.NameToLayer(Owner.OpponentLayerName);
            var count = Physics.OverlapSphereNonAlloc(
                Owner.transform.position,
                stats.Range,
                collidersFound,
                opponentMask,
                QueryTriggerInteraction.Collide
            );

            // Remove the enemies that are out of attack FOV
            for (var i = 0; i < count; i++)
            {
                if (!collidersFound[i])
                    continue;

                var direction = collidersFound[i].transform.position - Owner.transform.position;
                var angle = Vector3.Angle(Owner.MovementController.Body.forward, direction);

                // Deal damage to the enemies in the attack FOV
                if (angle > stats.AttackFOV)
                    continue;

                // Check if the enemy is a player and deal damage
                var playerController = collidersFound[i].gameObject.GetComponent<IDamageable>();
                playerController?.TakeDamage(GetDamageInfo());
            }
        }

        public override IDamageable.DamageInfo GetDamageInfo()
        {
            // TODO: Implement era specific damage calculation
            return new IDamageable.DamageInfo(stats.Damage, this);
        }

#if UNITY_EDITOR
        [ContextMenu("Copy Weapon ID")]
        private void CopyWeaponID()
        {
            GUIUtility.systemCopyBuffer = stats.WeaponID;
        }
#endif
    }
}