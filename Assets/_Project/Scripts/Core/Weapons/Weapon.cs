using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    /// <summary>
    /// Base class for all weapons.
    /// </summary>
    public abstract class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Coroutine for attacking.
        /// </summary>
        protected Coroutine AttackCoroutine;

        /// <summary>
        /// Is the weapon currently attacking.
        /// </summary>
        protected bool Attacking { get; private set; }

        /// <summary>
        /// Current player controller.
        /// </summary>
        public PlayerController CurrentPlayerController { get; private set; }
        
        public abstract string WeaponID { get; }

        /// <summary>
        /// Function called when the weapon is picked up.
        /// </summary>
        /// <param name="currentPlayerController">the player controller that will control the weapon</param>
        public virtual void OnPickup(PlayerController currentPlayerController)
        {
            CurrentPlayerController = currentPlayerController;
            gameObject.layer = currentPlayerController.gameObject.layer;
        }

        /// <summary>
        /// Function called when the weapon is dropped.
        /// </summary>
        public virtual void OnDrop()
        {
            CurrentPlayerController = null;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        /// <summary>
        /// Function called when the weapon is equipped.
        /// </summary>
        public virtual void OnEquip() { }

        /// <summary>
        /// Function called when the weapon is unequipped.
        /// </summary>
        public virtual void OnUnequip() { }

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

        /// <summary>
        /// Get the damage of the weapon.
        /// </summary>
        /// <returns>damage dealt by the weapon</returns>
        public virtual float GetDamage()
        {
            // TODO: Implement damage calculation in base classes
            return 0f;
        }
    }
}