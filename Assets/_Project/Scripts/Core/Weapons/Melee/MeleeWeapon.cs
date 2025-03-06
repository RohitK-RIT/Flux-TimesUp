using System;
using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Melee
{
    /// <summary>
    /// Melee weapon class.
    /// </summary>
    public class MeleeWeapon : Weapon
    {
        /// <summary>
        /// Melee weapon stats.
        /// </summary>
        [SerializeField] private MeleeWeaponStats stats;

        public override string WeaponID => stats.WeaponID;

        public MeleeWeaponStats Stats => stats;
        
        private DateTime _lastAttackTime = DateTime.MinValue;

        /// <summary>
        /// Coroutine for attacking.
        /// </summary>
        protected override IEnumerator OnAttack()
        {
            // Attack until the attack ends
            while (true)
            {
                // Wait for the attack speed and then fire the bullet.
                yield return new WaitWhile(() => (DateTime.Now - _lastAttackTime).Seconds < 1 / stats.AttackSpeed);
                Slash();
                _lastAttackTime = DateTime.Now;
                
                yield return new WaitForSeconds(1 / stats.AttackSpeed);
            }
        }

        /// <summary>
        /// Do the attack.
        /// </summary>
        private void Slash()
        {
            // Check for enemies in the attack range
            var collidersFound = new Collider[20];
            var count = Physics.OverlapSphereNonAlloc(CurrentPlayerController.transform.position, stats.Range, collidersFound, ~CurrentPlayerController.FriendlyLayer,
                QueryTriggerInteraction.Ignore);

            // Remove the enemies that are out of attack FOV
            for (var i = 0; i < count; i++)
            {
                if (!collidersFound[i])
                    continue;

                var direction = collidersFound[i].transform.position - CurrentPlayerController.transform.position;
                var angle = Vector3.Angle(CurrentPlayerController.MovementController.Body.forward, direction);

                // Deal damage to the enemies in the attack FOV
                if (angle > stats.AttackFOV)
                    continue;

                var colliderLayerMask = 1 << collidersFound[i].gameObject.layer;

                if ((colliderLayerMask & CurrentPlayerController.OpponentLayer) == 0)
                    continue;

                if (Physics.Raycast(CurrentPlayerController.transform.position, direction, out var raycastHit, stats.Range, ~CurrentPlayerController.FriendlyLayer,
                        QueryTriggerInteraction.Ignore) && raycastHit.collider != collidersFound[i])
                    continue;

                // Check if the enemy is a player and deal damage
                var playerController = collidersFound[i].gameObject.GetComponent<PlayerController>();
                playerController?.TakeDamage(this, GetDamage());
            }
        }

        public override float GetDamage()
        {
            // TODO: Implement era specific damage calculation
            return stats.Damage;
        }
    }
}