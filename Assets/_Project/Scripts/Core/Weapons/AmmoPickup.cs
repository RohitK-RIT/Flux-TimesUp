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
        
        public void OnItemPickup()
        {
            var currentWeapon = LevelSceneController.Instance.Player.WeaponController.CurrentWeapon;

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
                case MeleeWeapon meleeWeapon:
                    //add ammo to primary weapon
                    var playerWeaponController = LevelSceneController.Instance.Player.WeaponController;
                    var primaryWeapon = playerWeaponController.Weapons[0] as RangedWeapon;
                    if (primaryWeapon != null) primaryWeapon.AddAmmo(ammo);
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

        public void OnItemExitRange()
        {
        }
    }
}
