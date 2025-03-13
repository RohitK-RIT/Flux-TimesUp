using _Project.Scripts.Core.Backend.Helper;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Character.Hand_Controller;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    /// <summary>
    /// Base class for all weapons.
    /// </summary>
    public abstract class Weapon : MonoBehaviour, IHandItem
    {
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
            gameObject.SetLayerRecursively(currentPlayerController.gameObject.layer);
        }

        /// <summary>
        /// Function called when the weapon is dropped.
        /// </summary>
        public virtual void OnDrop()
        {
            CurrentPlayerController = null;
            gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));
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
        public abstract void BeginUse();

        /// <summary>
        /// End attacking.
        /// </summary>
        public abstract void EndUse();

        /// <summary>
        /// Get the damage of the weapon.
        /// </summary>
        /// <returns>damage dealt by the weapon</returns>
        public abstract IDamageable.DamageInfo GetDamageInfo();
    }
}