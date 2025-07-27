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
    [RequireComponent(typeof(Collider))]
    public abstract class Weapon : MonoBehaviour, IHandItem, IInteractable, ICollectible
    {
        /// <summary>
        /// Current player controller.
        /// </summary>
        public PlayerController Owner { get; private set; }

        public abstract string WeaponID { get; }

        protected bool Equipped { get; private set; }

        [SerializeField] private Vector3 holdPositionOffset;
        [SerializeField] private Vector3 holdRotationOffset;

        private Collider _interactableCollider;

        #region Interactable functions

        private void Awake()
        {
            _interactableCollider = GetComponent<Collider>();
            if (_interactableCollider)
                _interactableCollider.isTrigger = true;
        }

        public void OnHoverEnter(PlayerController controller)
        {
            Debug.Log($"Hovering over weapon: {WeaponID}");
        }

        public void OnHoverExit()
        {
            Debug.Log($"Hover exited from weapon: {WeaponID}");
        }

        #endregion

        #region Collectible functions

        public abstract void OnCollected(PlayerController playerController);

        #endregion

        #region Pickable functions

        public virtual void OnPickup(PlayerController owner)
        {
            Owner = owner;
            gameObject.SetLayerRecursively(owner.FriendlyLayerName);

            transform.localPosition = holdPositionOffset;
            transform.localRotation = Quaternion.Euler(holdRotationOffset);

            if (_interactableCollider)
                _interactableCollider.enabled = false;
        }

        public virtual void OnDrop()
        {
            Owner = null;
            gameObject.SetLayerRecursively("Pickup");

            transform.localRotation = Quaternion.identity;
            
            if (_interactableCollider)
                _interactableCollider.enabled = true;
        }

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