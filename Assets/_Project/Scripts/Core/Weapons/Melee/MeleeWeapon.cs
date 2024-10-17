using System.Collections;
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
            // Do the attack
            // Check if the attack hit something
            // If it did, apply damage
        }
    }
}