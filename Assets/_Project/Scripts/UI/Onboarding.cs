using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Core.Player_Controllers;
using TMPro;
using UnityEngine;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.Core.Weapons.Ranged;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// This class is responsible for displaying onboarding prompts to the player.
    /// </summary>
    public class Onboarding : MonoBehaviour
    {
        [SerializeField] private string[] onboardingPrompts;
        [SerializeField] private TextMeshProUGUI onboardingText;
        [SerializeField] private GameObject onboardingLevelOne;
        [SerializeField] private GameObject onboardingLevelTwo;
        [SerializeField] private GameObject onboardingLevelThree;
        private LevelSceneController _levelSceneController;
        private LocalPlayerController _playerController;
        private int currentPromptIndex = 0;
        
        private LocalInputController _inputController;

        private void Awake()
        {
            // Find and assign the player controller (assuming only one player in the scene)
            _inputController = FindObjectOfType<LocalInputController>();
            _levelSceneController = FindObjectOfType<LevelSceneController>();
            _playerController = FindObjectOfType<LocalPlayerController>();
            if (!_levelSceneController) return;
        }
        
        private void Start()
        {
            // Display the first prompt
            UpdateOnboardingPrompt(); // Display first prompt to look around.
        }

        private void Update()
        {
            if(_levelSceneController.WinPage.activeInHierarchy || _levelSceneController.LoosePage.activeInHierarchy)
                gameObject.SetActive(false); // Hide onboarding UI when onboarding is complete
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
            _inputController.OnAbilityEquipped += OnAbilityEquipped;
            _inputController.OnReloadInput += OnReloadDetected;
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
            _inputController.OnAbilityEquipped -= OnAbilityEquipped;
        }
        /// <summary>
        /// Updates the onboarding prompt text
        /// </summary>
        private void UpdateOnboardingPrompt()
        {
            if (currentPromptIndex < onboardingPrompts.Length)
            {
                onboardingText.text = onboardingPrompts[currentPromptIndex];
            }
        }
        /// <summary>
        /// Advances to the next onboarding prompt
        /// </summary>
        private void NextPrompt()
        {
            currentPromptIndex++;
            if (currentPromptIndex < onboardingPrompts.Length)
            {
                UpdateOnboardingPrompt();
            }
            else
            {
                onboardingText.text = ""; // Clear text when onboarding is complete
            }
        }
        /// <summary>
        /// Event handler for look input
        /// </summary>
        private void OnLookDetected(Vector2 lookInput)
        {
            if (currentPromptIndex != 0) return;
            NextPrompt(); // move prompt
        }
        /// <summary>
        /// Event handler for move input
        /// </summary>
        private void OnMoveDetected(Vector2 moveInput)
        {
            if (currentPromptIndex != 1) return;
            if(!onboardingLevelOne.activeInHierarchy)
                NextPrompt(); // attack prompt
        }

        /// <summary>
        /// Event handler for attack input
        /// </summary>
        private void OnAttackDetected()
        {
            if (currentPromptIndex != 2) return;
            //if(!onboardingLevelTwo.activeInHierarchy)
            NextPrompt(); // weapon switch prompt
        }
        /// <summary>
        /// Event handler for weapon switch input
        /// </summary>
        private void OnWeaponSwitchDetected(int weaponIndex)
        {
            if (currentPromptIndex != 3) return;
            NextPrompt(); // ability prompt
        }
        /// <summary>
        /// Event handler for ability equipped input
        /// </summary>
        private void OnAbilityEquipped()
        {
            if (currentPromptIndex != 4) return;
            NextPrompt(); // reload prompt
        }
        /// <summary>
        /// Event handler for reload input
        /// </summary>
        private void OnReloadDetected()
        {
            if (currentPromptIndex != 5) return;
            if(_playerController.HandController.CurrentWeapon is RangedWeapon)
                NextPrompt(); // defeat all enemies prompt
        }
    }
}
