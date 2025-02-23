using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Melee
{
    /// <summary>
    /// Melee weapon class.
    /// </summary>
    public sealed class MeleeWeapon : Weapon
    {
        /// <summary>
        /// Melee weapon stats.
        /// </summary>
        [SerializeField] private MeleeWeaponStats stats;

        public override string WeaponID  => stats.WeaponID;
        
        public MeleeWeaponStats Stats => stats;

        /// <summary>
        /// Coroutine for attacking.
        /// </summary>
        protected override IEnumerator OnAttack()
        {
            // Attack until the attack ends
            while (true)
            {
                Slash();

                yield return new WaitForSeconds(1 / stats.AttackSpeed);
            }
        }

        /// <summary>
        /// Do the attack.
        /// </summary>
        private void Slash()
        {
            // Check for enemies in the attack range
            var enemiesColliders = new Collider[20];
            var count = Physics.OverlapSphereNonAlloc(CurrentPlayerController.transform.position, stats.Range, enemiesColliders, CurrentPlayerController.OpponentLayer);

            // Remove the enemies that are out of attack FOV
            for (var i = 0; i < count; i++)
            {
                if (!enemiesColliders[i])
                    continue;

                var direction = enemiesColliders[i].transform.position - CurrentPlayerController.transform.position;
                var angle = Vector3.Angle(transform.forward, direction);

                // Deal damage to the enemies in the attack FOV
                if (angle > stats.AttackFOV)
                    continue;

                // Check if the enemy is a player and deal damage
                var playerController = enemiesColliders[i].gameObject.GetComponent<PlayerController>();
                playerController?.TakeDamage(this, GetDamage());
            }
        }

        protected override float GetDamage()
        {
            // TODO: Implement era specific damage calculation
            return stats.Damage;
        }
    }
}