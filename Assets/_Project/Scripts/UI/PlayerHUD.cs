using System;
using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Loadout;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Core.Weapons.Ranged;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Player HUD class.
    /// </summary>
    public class PlayerHUD : MonoBehaviour
    {
        // References to the UI components
        [SerializeField] public Slider healthBar;
        [SerializeField] private Slider timeStabilityBar;
        [SerializeField] public TMP_Text currAmmo;
        [SerializeField] public TMP_Text maxAmmo;
        [SerializeField] public TMP_Text PickupText;
        [SerializeField] public LocalPlayerController player;
        
        [SerializeField] public Image primaryWeaponSlotHolder;
        [SerializeField] public Image secondaryWeaponSlotHolder;
        [SerializeField] public Image meleeWeaponSlotHolder;
        [SerializeField] public Image abilitySlotHolder;
        [SerializeField] private GameObject overlay;

        [SerializeField] public GameObject reloadingText;

        //[SerializeField] private TMP_Text objectiveText;
        [SerializeField] private TMP_Text coinsText;
        
        private GameObject primaryOverlay;
        private GameObject secondaryOverlay;
        private GameObject meleeOverlay;
        private GameObject abilityOverlay;

        private AbilityData abilityData;

        private void Start()
        {
            // Initialize the health bar and ammo display with the player's starting values
            UpdateHealthBar();
            UpdateTimeStabilityBar();
            UpdateAmmoDisplay();
            primaryOverlay = Instantiate(overlay, primaryWeaponSlotHolder.rectTransform);
            secondaryOverlay = Instantiate(overlay, secondaryWeaponSlotHolder.rectTransform);
            meleeOverlay = Instantiate(overlay, meleeWeaponSlotHolder.rectTransform);
            abilityOverlay = Instantiate(overlay, abilitySlotHolder.rectTransform);
            abilitySlotHolder.gameObject.SetActive(false);
        }

        private void Update()
        {
            // Update the health bar and ammo display in real-time
            UpdateHealthBar();
            UpdateTimeStabilityBar();
            UpdateAmmoDisplay();
            UpdateReloadingText();
            //UpdateObjectiveText();
            UpdateCoinsText();
            UpdateLoadoutInfo();
        }
        public void ShowAbilityHUD(AbilityType abilityType)
        {
            abilityData = AbilityDataSystem.Instance.GetAbilityData(abilityType);
            abilitySlotHolder.sprite = abilityData.Icon;
            abilitySlotHolder.gameObject.SetActive(true);
        }
        //Updates the current loadout of the player in real-time.
        private void UpdateLoadoutInfo()
        {
            if (!player) return;

            if (player.WeaponController.CurrentWeapon is Ability)
            {
                abilitySlotHolder.enabled = true;
                abilitySlotHolder.sprite = abilityData.Icon;
                primaryOverlay.SetActive(true);
                secondaryOverlay.SetActive(true);
                meleeOverlay.SetActive(true);
                abilityOverlay.SetActive(false);
            }
            primaryWeaponSlotHolder.sprite = WeaponDataSystem.Instance.GetWeaponIcon(player.WeaponController.Weapons[0].WeaponID);
            secondaryWeaponSlotHolder.sprite = WeaponDataSystem.Instance.GetWeaponIcon(player.WeaponController.Weapons[1].WeaponID);
            meleeWeaponSlotHolder.sprite = WeaponDataSystem.Instance.GetWeaponIcon(player.WeaponController.Weapons[2].WeaponID);
            primaryWeaponSlotHolder.enabled = true;
            secondaryWeaponSlotHolder.enabled = true;
            meleeWeaponSlotHolder.enabled = true;
            ShowActiveWeaponSlot();
        }
        
        //Shows the active weapon slot based on the player's current weapon.
        private void ShowActiveWeaponSlot()
        {
            if (player.WeaponController.CurrentWeapon == player.WeaponController.Weapons[0])
            {
                primaryOverlay.SetActive(false);
                secondaryOverlay.SetActive(true);
                meleeOverlay.SetActive(true);
                abilityOverlay.SetActive(true);
            }
            else if (player.WeaponController.CurrentWeapon == player.WeaponController.Weapons[1])
            {
                primaryOverlay.SetActive(true);
                secondaryOverlay.SetActive(false);
                meleeOverlay.SetActive(true);
                abilityOverlay.SetActive(true);
            }
            else if (player.WeaponController.CurrentWeapon == player.WeaponController.Weapons[2])
            {
               primaryOverlay.SetActive(true);
               secondaryOverlay.SetActive(true);
               meleeOverlay.SetActive(false);
               abilityOverlay.SetActive(true);
            }
        }

        // Updates the health bar based on the player's current and max health
        private void UpdateHealthBar()
        {
            healthBar.value = player.CurrentHealth;
            healthBar.maxValue = player.Stats.maxHealth;
        }
        
        /// <summary>
        /// Function to update the time stability bar.
        /// </summary>
        private void UpdateTimeStabilityBar()
        {
            timeStabilityBar.value = TimeStabilityMeter.Instance.TimeStability;
            timeStabilityBar.maxValue = TimeStabilityMeter.Instance.InitialTimeStability;
        }

        // Updates the ammo display based on the player's current and total ammo
        private void UpdateAmmoDisplay()
        {
            var currentRangedWeapon = player.WeaponController.CurrentWeapon as RangedWeapon;
            if (!currentRangedWeapon) return;
            currAmmo.text = currentRangedWeapon.CurrentAmmo.ToString();
            maxAmmo.text = currentRangedWeapon.MaxAmmo.ToString();
        }

        // Updates the reloading text based on the player's current weapon state
        private void UpdateReloadingText()
        {
            var currentRangedWeapon = player.WeaponController.CurrentWeapon as RangedWeapon;
            if (!currentRangedWeapon)
                return;

            reloadingText.SetActive(currentRangedWeapon.IsReloading);
        }

        /*private void UpdateObjectiveText()
        {
            objectiveText.text = LevelSceneController.Instance.NumberOfEnemies.ToString();
        }*/

        private void UpdateCoinsText()
        {
            coinsText.text = player?.GetCoins().ToString();
        }
        
        /// <summary>
        /// Function to show pickup feedback.
        /// </summary>
        /// <param name="msg">Message to display on loot pickup.</param>
        public void ShowPickupFeedback(string msg)
        {
            PickupText.text = msg;
            PickupText.gameObject.SetActive(true);
            Invoke(nameof(HidePickupFeedback), 2f);
        }
        
        /// <summary>
        /// Function to hide pickup feedback.
        /// </summary>
        private void HidePickupFeedback()
        {
            PickupText.gameObject.SetActive(false);
        }
    }
}