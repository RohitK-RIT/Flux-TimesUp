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
    public abstract class Weapon : MonoBehaviour, IHandItem, IInteractable
    {
        /// <summary>
        /// Current player controller.
        /// </summary>
        public PlayerController Owner { get; private set; }

        public abstract string WeaponID { get; }

        protected bool Equipped { get; private set; }

        [SerializeField] private Vector3 holdPositionOffset;
        [SerializeField] private Vector3 holdRotationOffset;

        #region Interactable functions

        public void OnHoverEnter(PlayerController controller)
        {
            Debug.Log($"Hovering over weapon: {WeaponID}");
        }

        public void OnHoverExit()
        {
            Debug.Log($"Hover exited from weapon: {WeaponID}");
        }

        #region Pickable functions

        public virtual void OnPickup(PlayerController owner)
        {
            Owner = owner;
            gameObject.SetLayerRecursively(owner.FriendlyLayerName);

            transform.localPosition = holdPositionOffset;
            transform.localRotation = Quaternion.Euler(holdRotationOffset);
        }

        public virtual void OnDrop()
        {
            Owner = null;
            gameObject.SetLayerRecursively("Pickup");
            
            transform.localRotation = Quaternion.identity;
        }

        #endregion

        #endregion

        #region Hand Item functions

        public virtual void OnEquip()
        {
            Equipped = true;
        }

        public virtual void OnUnequip()
        {
            Equipped = false;
        }

        public abstract void BeginUse();

        public abstract void EndUse();

        #endregion

        /// <summary>
        /// Get the damage of the weapon.
        /// </summary>
        /// <returns>damage dealt by the weapon</returns>
        public abstract IDamageable.DamageInfo GetDamageInfo();
    }
}