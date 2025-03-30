using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Onboarding
{
    public class OnboardingManager : MonoBehaviour
    {
        private LocalPlayerController _playerController;
        private LocalInputController _inputController;
        
        [SerializeField] private GameObject onboardingPanel;
        [SerializeField] private Image controlsIcon;
        [SerializeField] private TMP_Text controlsName;
        [SerializeField] private TMP_Text controlsDescription;
        
        [SerializeField] private ControlsDataSystem controlsDataSystem;
        
        private int index = 0;

        private void Awake()
        {
            // Find and assign the player controller (assuming only one player in the scene)
            _inputController = FindObjectOfType<LocalInputController>();
            _playerController = FindObjectOfType<LocalPlayerController>();
            if (!_playerController) return;
        }
        
        private void Start()
        {
            controlsIcon.sprite = controlsDataSystem.controlsDatabase[index].controlsIcon;
            controlsName.text = controlsDataSystem.controlsDatabase[index].controlsName;
            controlsDescription.text = controlsDataSystem.controlsDatabase[index].controlsDescription;
        }
        
        /// <summary>
        /// Subscribe for input events.
        /// </summary>
        private void OnEnable()
        {
            // Subscribe to events
            if (!_inputController) return;
            _inputController.OnLookInputUpdated += OnLookDetected;
            _inputController.OnMoveInputUpdated += OnMoveDetected;
            _inputController.OnAttackInputBegan += OnAttackDetected;
            _inputController.OnSwitchWeaponInput += OnWeaponSwitchDetected;
            _inputController.OnLootPickupInput += OnLootPickupDetected;
            _inputController.OnAbilityEquipped += OnAbilityEquipped;
        }

        /// <summary>
        /// Unsubscribe from events to avoid memory leaks
        /// </summary>
        private void OnDisable()
        {
            // Unsubscribe from events to avoid memory leaks
            if (!_inputController) return;
            _inputController.OnLookInputUpdated -= OnLookDetected;
            _inputController.OnMoveInputUpdated -= OnMoveDetected;
            _inputController.OnAttackInputBegan -= OnAttackDetected;
            _inputController.OnSwitchWeaponInput -= OnWeaponSwitchDetected;
            _inputController.OnLootPickupInput -= OnLootPickupDetected;
            _inputController.OnAbilityEquipped -= OnAbilityEquipped;
        }

        private void Update()
        {
            if (onboardingPanel.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        private void UpdateOnboardingData()
        {
            if (index >= controlsDataSystem.controlsDatabase.Length)
            {
                onboardingPanel.SetActive(false);
                return;
            }
            controlsIcon.sprite = controlsDataSystem.controlsDatabase[index].controlsIcon;
            controlsName.text = controlsDataSystem.controlsDatabase[index].controlsName;
            controlsDescription.text = controlsDataSystem.controlsDatabase[index].controlsDescription;
        }
        
        #region Input Event Handlers
        /// <summary>
        /// Event handler for look input
        /// </summary>
        private void OnLookDetected(Vector2 lookInput)
        {
            index++;
            UpdateOnboardingData();
        }
        /// <summary>
        /// Event handler for move input
        /// </summary>
        private void OnMoveDetected(Vector2 moveInput)
        {
            index++;
            UpdateOnboardingData();
        }
        /// <summary>
        /// Event handler for weapon switch input
        /// </summary>
        private void OnWeaponSwitchDetected(int weaponIndex)
        {
            index++;
            UpdateOnboardingData();
        }
        /// <summary>
        /// Event handler for attack input
        /// </summary>
        private void OnAttackDetected()
        {
            index++;
            UpdateOnboardingData();
        }
        /// <summary>
        /// Event handler for loot pickup input
        /// </summary>
        private void OnLootPickupDetected()
        {
            index++;
            UpdateOnboardingData();
        }
        /// <summary>
        /// Event handler for ability equipped input
        /// </summary>
        private void OnAbilityEquipped()
        {
            index++;
            UpdateOnboardingData();
        }
        #endregion
    }
}
