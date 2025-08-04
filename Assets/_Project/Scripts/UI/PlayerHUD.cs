using System;
using System.Linq;
using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Enemy.EnemySpawner;
using _Project.Scripts.Core.Loadout;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using _Project.Scripts.Gameplay.Revamp_PCG;
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

        [Header("Health Bar")] [SerializeField]
        public Slider healthBar;

        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Image healthFill;
        [SerializeField] private Gradient healthGradient;

        [Header("Time Stability Bar")] [SerializeField]
        public Slider timeStabilityBar;

        [SerializeField] private TMP_Text tmsValueText;
        [SerializeField] private Image tsmFill;

        [SerializeField] private Gradient tsmGradient;
        //[SerializeField] private Animator animator;
        //private static readonly int IsBlinking = Animator.StringToHash("IsBlinking");

        [Header("Enemies Remaining & Pickup Info")] [SerializeField]
        private TMP_Text enemiesRemaining;

        [SerializeField] public TMP_Text pickupText;

        [Header("Loadout Information")] [SerializeField]
        public TMP_Text currAmmo;

        [SerializeField] public TMP_Text maxAmmo;
        [SerializeField] private GameObject ammo;
        [SerializeField] public Image primaryIconSlot;
        [SerializeField] public Image meleeIconSlot;
        [SerializeField] public Image abilityIconSlot;
        [SerializeField] private GameObject overlay;
        [SerializeField] public GameObject reloadingText;
        [SerializeField] public GameObject reloadingIcon;

        [Header("References")] [SerializeField]
        private LocalPlayerController player;

        [SerializeField] private RandomRoomGeneration randomRoomGeneration;

        [SerializeField] private RectTransform pickupTextPrefab;

        private RectTransform PickupTextInstance
        {
            get
            {
                if(!_pickupTextInstance)
                {
                    _pickupTextInstance = Instantiate(pickupTextPrefab);
                    _pickupTextInstance.gameObject.SetActive(false);
                }
                
                return _pickupTextInstance;
            }
        }

        // Updates the reloading text based on the player's current weapon state
        private Coroutine _reloadingCoroutine;
        private GameObject _primaryOverlay;
        private GameObject _meleeOverlay;
        private GameObject _abilityOverlay;
        private AbilityData _abilityData;
        private AbilityCooldown _abilityCooldown;

        private RoomWaveController _roomWaveController;

        private RectTransform _pickupTextInstance;
        private IInteractable _currentInteractable;

        private void Start()
        {
            // Initialize the health bar and ammo display with the player's starting values
            UpdateHealthBar();
            UpdateTimeStabilityBar();
            UpdateAmmoDisplay();

            _primaryOverlay = Instantiate(overlay, primaryIconSlot.rectTransform.parent);
            _meleeOverlay = Instantiate(overlay, meleeIconSlot.rectTransform.parent);
            _abilityOverlay = Instantiate(overlay, abilityIconSlot.rectTransform.parent);
            _abilityCooldown = _abilityOverlay.GetComponent<AbilityCooldown>();

            _roomWaveController = FindObjectOfType<RoomWaveController>();
            if (_roomWaveController == null)
            {
                Debug.LogError("RoomWaveController not found in the scene.");
            }
        }

        private void OnEnable()
        {
            player.HandController.OnAmmoPicked += OnAmmoPickup;
            player.HandController.OnAbilityPicked += OnAbilityPicked;
        }

        private void OnDisable()
        {
            player.HandController.OnAmmoPicked -= OnAmmoPickup;
            player.HandController.OnAbilityPicked -= OnAbilityPicked;
        }

        private void Update()
        {
            if (!player) return;
            if (player.HandController.CurrentItem is Ability or MeleeWeapon)
            {
                ammo.SetActive(false);
                reloadingText.SetActive(false);
            }
            UpdateHealthBar();
            UpdateTimeStabilityBar();
            UpdateAmmoDisplay();
            UpdateReloadingText();
            UpdateLoadoutInfo();
            UpdateEnemiesRemaining();
            UpdateInteractable();
        }

        // Updates the number of enemies remaining in the room
        private void UpdateEnemiesRemaining()
        {
            if (randomRoomGeneration.hasInstantiatedBossRoom)
            {
                enemiesRemaining.gameObject.SetActive(false);
                return;
            }
            _roomWaveController = FindObjectOfType<RoomWaveController>();
            if (_roomWaveController == null) return;
            var enemiesCount = 0;
            foreach (var enemy in _roomWaveController.EnemiesInRoom)
            {
                if (enemy != null && enemy.activeInHierarchy)
                {
                    enemiesCount++;
                }
            }
            if (enemiesCount > 0)
            {
                enemiesRemaining.text = "Enemies Remaining: " + enemiesCount.ToString();
            }
            else if(enemiesCount == 0)
            {
                enemiesRemaining.text = "Portal is now open!";
            }
        }

        // Shows the ability HUD with the specified ability type
        private void ShowAbilityHUD(AbilityType abilityType)
        {
            _abilityData = AbilityDataSystem.Instance.GetAbilityData(abilityType);
            abilityIconSlot.sprite = _abilityData.Icon;
            abilityIconSlot.color = Color.white;
        }

        //Updates the current loadout of the player in real-time.
        private void UpdateLoadoutInfo()
        {
            if (!player)
                return;

            var currentAbility = player.HandController.CurrentAbility;
            if (currentAbility)
            {
                abilityIconSlot.enabled = true;
                abilityIconSlot.sprite = _abilityData.Icon;
                if (currentAbility.IsCooldownActive)
                {
                    Debug.Log("Current ability is on cooldown" + currentAbility.name);
                    _abilityCooldown.ActivateCooldown(currentAbility.CooldownTime);
                }
            }

            primaryIconSlot.sprite = player.HandController.Weapons[0] ? WeaponDataSystem.Instance.GetWeaponIcon(player.HandController.Weapons[0].WeaponID) : null;
            meleeIconSlot.sprite = player.HandController.Weapons[1] ? WeaponDataSystem.Instance.GetWeaponIcon(player.HandController.Weapons[1].WeaponID) : null;
            primaryIconSlot.enabled = true;
            meleeIconSlot.enabled = true;

            var currentItem = player.HandController.CurrentItem;
            _primaryOverlay.SetActive(currentItem != null && !currentItem.Equals(player.HandController.Weapons[0]));
            _meleeOverlay.SetActive(currentItem != null && !currentItem.Equals(player.HandController.Weapons[1]));
            _abilityOverlay.SetActive(currentItem != null && !currentItem.Equals(currentAbility));
        }

        // Updates the ammo display based on the player's current and total ammo
        private void UpdateAmmoDisplay()
        {
            var currentRangedWeapon = player.HandController.CurrentItem as RangedWeapon;
            if (!currentRangedWeapon) return;
            ammo.SetActive(true);
            currAmmo.text = currentRangedWeapon.CurrentAmmo.ToString();
            maxAmmo.text = currentRangedWeapon.MaxAmmo.ToString();
        }

        private void UpdateReloadingText()
        {
            var currentRangedWeapon = player.HandController.CurrentItem as RangedWeapon;
            if (!currentRangedWeapon)
                return;

            bool isReloading = currentRangedWeapon.IsReloading;
            reloadingText.SetActive(isReloading);

            if (isReloading)
            {
                if (_reloadingCoroutine == null) // Don't start multiple coroutines
                {
                    _reloadingCoroutine = StartCoroutine(UpdateReloadingIcon(currentRangedWeapon));
                }
            }

            else
            {
                if (_reloadingCoroutine != null)
                {
                    StopCoroutine(_reloadingCoroutine);
                    _reloadingCoroutine = null;
                    reloadingIcon.transform.rotation = Quaternion.identity; // Reset icon rotation to default
                }
            }
        }

        private System.Collections.IEnumerator UpdateReloadingIcon(RangedWeapon currentRangedWeapon)
        {
            float reloadTime = currentRangedWeapon.Stats.ReloadTime;
            float elapsedTime = 0f;

            while (elapsedTime < reloadTime)
            {
                float rotationAmount = (elapsedTime / reloadTime) * 360f;
                reloadingIcon.transform.localRotation = Quaternion.Euler(0f, 0f, -rotationAmount); // Rotate in local space
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            reloadingIcon.transform.localRotation = Quaternion.identity; // Ensure perfect reset
            _reloadingCoroutine = null;
        }

        /// <summary>
        /// Function for event when ammo is picked up.
        /// </summary>
        /// <param name="amount">the amount of ammo that is picked up</param>
        private void OnAmmoPickup(int amount)
        {
            ShowPickupFeedback($"You picked up {amount} ammo.");
        }

        /// <summary>
        /// Function for event when an ability is picked up by the player.
        /// </summary>
        /// <param name="type">type of the ability that is picked up</param>
        private void OnAbilityPicked(AbilityType type)
        {
            ShowPickupFeedback($"You picked up {type}");
            ShowAbilityHUD(type);
        }

        /// <summary>
        /// Function to show pickup feedback.
        /// </summary>
        /// <param name="msg">Message to display on loot pickup.</param>
        private void ShowPickupFeedback(string msg)
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

        /// <summary>
        /// Updates the pickup text.
        /// </summary>
        private void UpdateInteractable()
        {
            var currentInteractable = player.CurrentInteractable;
            if (currentInteractable == null)
            {
                PickupTextInstance.gameObject.SetActive(false);
                return;
            }

            PickupTextInstance.transform.SetParent(currentInteractable.transform);
            try
            {
                PickupTextInstance.GetComponentInChildren<TMP_Text>().SetText($"Press F to pick up {currentInteractable.DisplayName}");
            }
            catch (Exception e)
            {
                Debug.LogError("Error setting pickup text:");
                Debug.LogException(e);
            }

            PickupTextInstance.transform.localPosition = Vector3.up;
            PickupTextInstance.gameObject.SetActive(true);
        }
    }
}