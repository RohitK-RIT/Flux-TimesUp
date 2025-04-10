using System;
using _Project.Scripts.Core.Backend.Helper;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Character.Hand_Controller;
using _Project.Scripts.Core.Player_Controllers;
using TMPro;
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
        public PlayerController CurrentPlayerController { get; private set; }


        public abstract string WeaponID { get; }

        protected bool Equipped { get; private set; }

        private TMP_Text _pickUpInstruction;
        private BoxCollider _pickupCollider;

        private void Awake()
        {
            try
            {
                _pickupCollider = gameObject.GetComponent<BoxCollider>();

                _pickUpInstruction = GetComponentInChildren<TMP_Text>();
                _pickUpInstruction.text = "Press 'F' to pick up";
                _pickUpInstruction.gameObject.SetActive(false);

                gameObject.SetLayerRecursively("Pickup");
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
            }
        }

        /// <summary>
        /// Function called when the weapon is picked up.
        /// </summary>
        /// <param name="controller">the player controller that will control the weapon</param>
        public virtual void OnPickup(PlayerController controller)
        {
            try
            {
                CurrentPlayerController = controller;
                gameObject.SetLayerRecursively(controller.FriendlyLayerName);
                _pickupCollider.enabled = false;
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
            }
        }

        /// <summary>
        /// Function called when the weapon is dropped.
        /// </summary>
        public virtual void OnDrop()
        {
            try
            {
                CurrentPlayerController = null;
                gameObject.SetLayerRecursively("Pickup");
                _pickupCollider.enabled = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
            }
        }

        public void OnHoverEnter(PlayerController controller)
        {
            _pickUpInstruction.gameObject.SetActive(true);
        }

        public void OnHoverExit()
        {
            _pickUpInstruction.gameObject.SetActive(false);
        }

        /// <summary>
        /// Function called when the weapon is equipped.
        /// </summary>
        public virtual void OnEquip()
        {
            Equipped = true;
        }

        /// <summary>
        /// Function called when the weapon is unequipped.
        /// </summary>
        public virtual void OnUnequip()
        {
            Equipped = false;
        }

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