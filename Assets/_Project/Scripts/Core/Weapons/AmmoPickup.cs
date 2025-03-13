using System;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    public class AmmoPickup : MonoBehaviour, IPickupItem
    {
        [SerializeField] private int ammo;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnItemEnterRange();
            }
        }

        public void OnItemPickup()
        {
            var currentWeapon = LevelSceneController.Instance.Player.HandController.CurrentItem;

            switch (currentWeapon)
            {
                case RangedWeapon rangedWeapon:
                    switch (rangedWeapon.Stats.WeaponType)
                    {
                        case WeaponType.Secondary:
                            // add ammo to secondary weapon
                            rangedWeapon.AddAmmo(ammo);
                            break;
                        // add ammo to primary weapon by default
                        case WeaponType.Primary:
                            rangedWeapon.AddAmmo(ammo);
                            break;
                    }

                    break;
                default:
                    //add ammo to primary weapon
                    var playerWeaponController = LevelSceneController.Instance.Player.HandController;
                    if (playerWeaponController.Weapons[0] is RangedWeapon primaryWeapon)
                        primaryWeapon.AddAmmo(ammo);
                    break;
            }

            var msg = "You picked up " + ammo + " ammo.";
            LevelSceneController.Instance.playerHUD.ShowPickupFeedback(msg);
            Destroy(gameObject);
        }

        public void OnItemEnterRange()
        {
            this.OnItemPickup();
        }

        public void OnItemExitRange() { }
    }
}