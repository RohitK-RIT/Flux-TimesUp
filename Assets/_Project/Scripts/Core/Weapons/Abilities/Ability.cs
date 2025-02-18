using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    /// <summary>
    /// Abstract base class for player abilities, inheriting from Weapon.
    /// </summary>
    public abstract class Ability : Weapon
    {
        /// <summary>
        /// The type of the ability.
        /// </summary>
        public abstract AbilityType Type { get; }
        public override string WeaponID  => Type.ToString();
        
        public bool IsCooldownActive => _isCooldownActive;

        /// <summary>
        /// Indicates if the cooldown is active.
        /// </summary>
        private bool _isCooldownActive;

        /// <summary>
        /// Indicates if the ability is active.
        /// </summary>
        protected bool IsAbilityActive;

        /// <summary>
        /// Indicates if the ability has been used.
        /// </summary>
        public bool Used { get; protected set; }
        
        public float CooldownTime { get; private set; }
        
        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            Used = false;
        }

        public virtual void OnUpgrade()
        {
            Debug.Log("Ability has been upgraded!");
        }

        /// <summary>
        /// Starts the cooldown period for the ability.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine.</returns>
        protected IEnumerator StartCooldown(float cooldown)
        {
            CooldownTime = cooldown;
            _isCooldownActive = true;
            yield return new WaitForSeconds(cooldown); // Wait for the cooldown period
            _isCooldownActive = false;
            IsAbilityActive = false; // Allow a new attack after cooldown
            Debug.Log("Ability is on cooldown!!");
        }
    }
}