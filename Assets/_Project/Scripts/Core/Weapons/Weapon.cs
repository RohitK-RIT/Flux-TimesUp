using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    /// <summary>
    /// Base class for all weapons.
    /// </summary>
    /// <typeparam name="T">struct for weapon stats</typeparam>
    public abstract class Weapon<T> : MonoBehaviour where T : WeaponStats
    {
        /// <summary>
        /// Stats of the weapon.
        /// </summary>
        [SerializeField] protected T stats;

        /// <summary>
        /// Coroutine for attacking.
        /// </summary>
        protected Coroutine AttackCoroutine;

        /// <summary>
        /// Is the weapon currently attacking.
        /// </summary>
        protected bool Attacking { get; private set; }

        /// <summary>
        /// Start attacking.
        /// </summary>
        public virtual void BeginAttack()
        {
            // End the previous attack if it's still running
            EndAttack();
            
            Attacking = true;
            
            // Start the new attack
            AttackCoroutine = StartCoroutine(OnAttack());
        }

        /// <summary>
        /// End attacking.
        /// </summary>
        public virtual void EndAttack()
        {
            // End the previous attack if it's still running
            if (AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);

            AttackCoroutine = null;

            Attacking = false;
        }

        /// <summary>
        /// Coroutine for attacking.
        /// </summary>
        protected abstract IEnumerator OnAttack();
    }
}