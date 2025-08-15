using System.Linq;
using _Project.Scripts.Core.Backend.Weapon;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Core.Loadout
{
    public class WeaponSlots : MonoBehaviour
    {
        // The ID of the weapon that this slot represents. 
        // This ID is used to uniquely identify the weapon in the loadout system.
        public string weaponId;

        // The type of weapon associated with this slot (Primary, Secondary, or Melee).
        // This helps categorize the weapon and assign it to the correct loadout slot.
        public WeaponType weaponType;
        
        //UI Element to display selected loadout weapon feedback
        [SerializeField] public GameObject overlayImage;
        
        // Indicates whether the mouse is currently hovering over the weapon slot.
        private bool isHovered = false;
        
        //UI Element to display selected loadout weapon info card
        [SerializeField] private GameObject weaponInfoCard;
        
        [SerializeField] private TMP_Text weaponName;
        [SerializeField] private TMP_Text weaponEra;
        [SerializeField] private TMP_Text weaponDps;
        [SerializeField] private TMP_Text weaponAttackMode;
        [SerializeField] private TMP_Text weaponAmmo;
        [SerializeField] private TMP_Text attackRange;

        private void Start()
        {
            weaponName.text = "";
            weaponEra.text = "";
            weaponDps.text = "";
            weaponAttackMode.text = "";
            weaponAmmo.text = "";
            attackRange.text = "";
            AssignWeaponInfo();
            weaponInfoCard.SetActive(false);
        }
        public void ShowOverlay()
        {
            if (overlayImage != null)
            {
                overlayImage.SetActive(true);
            }
        }

        public void HideOverlay()
        {
            if (overlayImage != null)
            {
                overlayImage.SetActive(false);
            }
        }
        
        private void AssignWeaponInfo()
        {
            var weaponData = WeaponDataSystem.Instance.GetWeaponInfo(weaponId);
            if (weaponData == null) return;
            // Assign weapon info to the UI elements
            
            //weapon name
            weaponName.text = "Name: " + weaponData.Stats.WeaponName;
            //weapon era
            weaponEra.text = "Era: " + weaponData.Stats.WeaponEra.ToString();
            //damage per hit
            weaponDps.text = "Damage: " +weaponData.Stats.Damage.ToString();
            
            //firing mode
            if (weaponData.Stats is RangedWeaponStats rangedWeaponStats)
            {
                weaponAttackMode.text += "Firing Mode: " + string.Join(", ", rangedWeaponStats.FireModes.Select(f => f.ToString()));
                weaponAmmo.text += "Ammo: " + rangedWeaponStats.MaxBulletCount.ToString();
            }
            
            else if (weaponData.Stats is MeleeWeaponStats meleeWeaponStats)
            {
                weaponAttackMode.text += "Attack Speed: " + meleeWeaponStats.AttackSpeed.ToString();
                attackRange.text += "Attack Range: " + meleeWeaponStats.Range.ToString();
            }
        }
    }
}
