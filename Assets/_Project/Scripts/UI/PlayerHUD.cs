using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Loadout;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Core.Weapons.Ranged;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Player HUD class.
    /// </summary>
    public class PlayerHUD : MonoBehaviour
    {
        // References to the UI components
        
        //Health Bar
        [SerializeField] public Slider healthBar;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Gradient healthGradient;
        [SerializeField] private Image healthFill;
        
        //Time Stability Bar
        [SerializeField] public Slider timeStabilityBar;
        [SerializeField] private TMP_Text tmsValueText;
        [SerializeField] private Gradient tsmGradient;
        [SerializeField] private Image tsmFill;
        //[SerializeField] private Animator animator;
        //private static readonly int IsBlinking = Animator.StringToHash("IsBlinking");
        
        //Enemies Remaining & Pick up Info
        [SerializeField] private TMP_Text enemiesRemaining;
        [SerializeField] public TMP_Text pickupText;
        
        [SerializeField] public TMP_Text currAmmo;
        [SerializeField] public TMP_Text maxAmmo;
        
        [SerializeField] public LocalPlayerController player;
        
        [SerializeField] public Image primaryIconSlot;
        [SerializeField] public Image secondaryIconSlot;
        [SerializeField] public Image meleeIconSlot;
        [SerializeField] public Image abilityIconSlot;
        [SerializeField] public GameObject abilitySlotHolder;
        [SerializeField] private GameObject overlay;

        [SerializeField] public GameObject reloadingText;
        [SerializeField] public GameObject reloadingIcon;

        private GameObject primaryOverlay;
        private GameObject secondaryOverlay;
        private GameObject meleeOverlay;
        private GameObject abilityOverlay;

        private AbilityData abilityData;
        private AbilityCooldown abilityCooldown;

        //[SerializeField] private TMP_Text currentRoomText;

        //private Ability currentAbility;

        private void Start()
        {
            // Initialize the health bar and ammo display with the player's starting values
            UpdateHealthBar();
            UpdateTimeStabilityBar();
            UpdateAmmoDisplay();
            primaryOverlay = Instantiate(overlay, primaryIconSlot.rectTransform);
            secondaryOverlay = Instantiate(overlay, secondaryIconSlot.rectTransform);
            meleeOverlay = Instantiate(overlay, meleeIconSlot.rectTransform);
            abilityOverlay = Instantiate(overlay, abilityIconSlot.rectTransform);
            abilityCooldown = abilityOverlay.GetComponent<AbilityCooldown>();
            abilityIconSlot.gameObject.SetActive(false);
            abilitySlotHolder.SetActive(false);
        }

        private void Update()
        {
            UpdateHealthBar();
            UpdateTimeStabilityBar();
            UpdateAmmoDisplay();
            UpdateReloadingText();
            UpdateLoadoutInfo();
        }
        
        public void ShowAbilityHUD(AbilityType abilityType)
        {
            abilityData = AbilityDataSystem.Instance.GetAbilityData(abilityType);
            abilityIconSlot.sprite = abilityData.Icon;
            abilitySlotHolder.SetActive(true);
            abilityIconSlot.gameObject.SetActive(true);
        }
        //Updates the current loadout of the player in real-time.
        private void UpdateLoadoutInfo()
        {
            var currentAbility = player.HandController.CurrentItem as Ability;
            if (!player) return;

            if (player.HandController.CurrentItem is Ability)
            {
                abilityIconSlot.enabled = true;
                abilityIconSlot.sprite = abilityData.Icon;
                primaryOverlay.SetActive(true);
                secondaryOverlay.SetActive(true);
                meleeOverlay.SetActive(true);
                abilityOverlay.SetActive(false);
            }

            if (currentAbility != null && currentAbility.IsCooldownActive)
            {
                Debug.Log("Current ability is on cooldown" + currentAbility.name);
                abilityCooldown.ActivateCooldown(currentAbility.CooldownTime);
            }

            primaryIconSlot.sprite = WeaponDataSystem.Instance.GetWeaponIcon(player.HandController.Weapons[0].WeaponID);
            secondaryIconSlot.sprite = WeaponDataSystem.Instance.GetWeaponIcon(player.HandController.Weapons[1].WeaponID);
            meleeIconSlot.sprite = WeaponDataSystem.Instance.GetWeaponIcon(player.HandController.Weapons[2].WeaponID);
            primaryIconSlot.enabled = true;
            secondaryIconSlot.enabled = true;
            meleeIconSlot.enabled = true;
            ShowActiveWeaponSlot();
        }
        
        //Shows the active weapon slot based on the player's current weapon.
        private void ShowActiveWeaponSlot()
        {
            if (player.HandController.CurrentItem == player.HandController.Weapons[0])
            {
                primaryOverlay.SetActive(false);
                secondaryOverlay.SetActive(true);
                meleeOverlay.SetActive(true);
                abilityOverlay.SetActive(true);
            }
            else if (player.HandController.CurrentItem == player.HandController.Weapons[1])
            {
                primaryOverlay.SetActive(true);
                secondaryOverlay.SetActive(false);
                meleeOverlay.SetActive(true);
                abilityOverlay.SetActive(true);
            }
            else if (player.HandController.CurrentItem == player.HandController.Weapons[2])
            {
               primaryOverlay.SetActive(true);
               secondaryOverlay.SetActive(true);
               meleeOverlay.SetActive(false);
               abilityOverlay.SetActive(true);
            }
        }

        // Updates the ammo display based on the player's current and total ammo
        private void UpdateAmmoDisplay()
        {
            var currentRangedWeapon = player.HandController.CurrentItem as RangedWeapon;
            if (!currentRangedWeapon) return;
            currAmmo.text = currentRangedWeapon.CurrentAmmo.ToString();
            maxAmmo.text = currentRangedWeapon.MaxAmmo.ToString();
        }

        // Updates the reloading text based on the player's current weapon state
        private void UpdateReloadingText()
        {
            var currentRangedWeapon = player.HandController.CurrentItem as RangedWeapon;
            if (!currentRangedWeapon)
                return;

            reloadingText.SetActive(currentRangedWeapon.IsReloading);
            StartCoroutine(UpdateReloadingIcon(currentRangedWeapon.IsReloading, currentRangedWeapon));
        }
        
        private System.Collections.IEnumerator UpdateReloadingIcon(bool isReloading, RangedWeapon currentRangedWeapon)
        {
            var originalRotation = reloadingIcon.transform.rotation; // Store original rotation
            var reloadTime = currentRangedWeapon.Stats.ReloadTime;
            const float totalRotation = 360f; // Full circle rotation

            if (isReloading)
            {
                reloadingIcon.transform.Rotate(Vector3.forward, totalRotation * Time.deltaTime / reloadTime);
                yield return new WaitForSeconds(reloadTime);
            }
            // Ensure it resets exactly to the original rotation
            reloadingIcon.transform.rotation = originalRotation;
        }
        
        /// <summary>
        /// Function to show pickup feedback.
        /// </summary>
        /// <param name="msg">Message to display on loot pickup.</param>
        public void ShowPickupFeedback(string msg)
        {
            pickupText.text = msg;
            pickupText.gameObject.SetActive(true);
            Invoke(nameof(HidePickupFeedback), 2f);
        }
        
        /// <summary>
        /// Function to hide pickup feedback.
        /// </summary>
        private void HidePickupFeedback()
        {
            pickupText.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Function to update the time stability bar.
        /// </summary>
        private void UpdateTimeStabilityBar()
        {
            timeStabilityBar.value = TimeStabilityMeter.Instance.TimeStability;
            timeStabilityBar.maxValue = TimeStabilityMeter.Instance.TotalTimeStability;
            tmsValueText.text = timeStabilityBar.value + " / " + timeStabilityBar.maxValue;
            tsmFill.color = tsmGradient.Evaluate(timeStabilityBar.normalizedValue);
            /*animator.SetBool(IsBlinking, false);
            if (timeStabilityBar.value < 50)
            {
                animator.SetBool(IsBlinking, true);
                animator.speed = 0.5f;
            }
            else if(timeStabilityBar.value < 25)
            {
                animator.SetBool(IsBlinking, true);
                animator.speed = 1f;
            }*/
        }
        
        // Updates the health bar based on the player's current and max health
        private void UpdateHealthBar()
        {
            healthBar.value = player.CurrentHealth;
            healthBar.maxValue = player.Stats.maxHealth;
            healthText.text = player.CurrentHealth + " / " + player.Stats.maxHealth;
            healthFill.color = healthGradient.Evaluate(healthBar.normalizedValue);
        }
    }
}