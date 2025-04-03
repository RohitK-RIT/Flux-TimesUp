using System;
using System.Collections;
using _Project.Scripts.Core.Backend.Scene_Control;
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

        [SerializeField] public ControlsDataSystem controlsDataSystem;

        public int OnboardingIndex => index;
        private int index = 0;
        private bool onboardingLocked = true;
        private bool waitingForInput = false;
        private bool waitingForLootDrop = false;
        
        public Action OnLookAndMoveComplete;
        public Action OnWeaponSwitchAndAttackComplete;
        public Action OnPlayerTeleportComplete;
        public Action OnPlayerLootControlsComplete;

        private void Awake()
        {
            _inputController = FindObjectOfType<LocalInputController>();
            _playerController = FindObjectOfType<LocalPlayerController>();
            if(!_playerController) return;
        }

        private void Start()
        {
            ShowOnboardingStep();
        }

        private void OnEnable()
        {
            if (!_inputController) return;
            _inputController.OnLookInputUpdated += OnLookDetected;
            _inputController.OnMoveInputUpdated += OnMoveDetected;
            _inputController.OnAttackInputBegan += OnAttackDetected;
            _inputController.OnSwitchWeaponInput += OnWeaponSwitchDetected;
            _inputController.OnLootPickupInput += OnLootPickupDetected;
            _inputController.OnAbilityEquipped += OnAbilityEquipped;
        }

        private void OnDisable()
        {
            if (!_inputController) return;
            _inputController.OnLookInputUpdated -= OnLookDetected;
            _inputController.OnMoveInputUpdated -= OnMoveDetected;
            _inputController.OnAttackInputBegan -= OnAttackDetected;
            _inputController.OnSwitchWeaponInput -= OnWeaponSwitchDetected;
            _inputController.OnLootPickupInput -= OnLootPickupDetected;
            _inputController.OnAbilityEquipped -= OnAbilityEquipped;
        }

        private void ShowOnboardingStep()
        {
            if (index >= controlsDataSystem.controlsDatabase.Length)
            {
                EndOnboarding();
                return;
            }

            onboardingLocked = true;
            waitingForInput = false;

            onboardingPanel.SetActive(true);
            controlsIcon.sprite = controlsDataSystem.controlsDatabase[index].controlsIcon;
            controlsName.text = controlsDataSystem.controlsDatabase[index].controlsName;
            controlsDescription.text = controlsDataSystem.controlsDatabase[index].controlsDescription;

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void OnContinueClicked()
        {
            onboardingLocked = false;
            waitingForInput = true;

            onboardingPanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void AdvanceStep()
        {
            index++;
            if (index == 2)
            {
                OnLookAndMoveComplete?.Invoke();
            }
            if(index == controlsDataSystem.controlsDatabase.Length)
            {
                OnPlayerLootControlsComplete?.Invoke();
            }
            
            ShowOnboardingStep();
        }

        private void EndOnboarding()
        {
            onboardingPanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnPlayerEnteredPortal()
        {
            waitingForLootDrop = true;
            OnPlayerTeleportComplete?.Invoke();
        }
        public void OnLootDroppedAfterTeleport()
        {
            if (!waitingForLootDrop) return;

            waitingForLootDrop = false;
            AdvanceStep();
        }

        #region DelayCoroutines
        private IEnumerator DelayedAdvanceStep(float delayTime)
        {
            yield return new WaitForSecondsRealtime(delayTime); // Use Realtime to ignore Time.timeScale
            AdvanceStep();
        }
        private IEnumerator DelayCoroutine(float delayTime)
        {
            yield return new WaitForSecondsRealtime(delayTime); // Use Realtime to ignore Time.timeScale
        }
        #endregion
        
        #region Input Event Handlers

        private void OnLookDetected(Vector2 lookInput)
        {
            if (!waitingForInput || onboardingLocked || index >= controlsDataSystem.controlsDatabase.Length) return;
            if (controlsDataSystem.controlsDatabase[index].inputType != InputType.Look) return;

            waitingForInput = false;
            StartCoroutine(DelayedAdvanceStep(2));
        }

        private void OnMoveDetected(Vector2 moveInput)
        {
            if (!waitingForInput || onboardingLocked || index >= controlsDataSystem.controlsDatabase.Length) return;
            if (controlsDataSystem.controlsDatabase[index].inputType != InputType.Move) return;

            waitingForInput = false;
            StartCoroutine(DelayedAdvanceStep(3.5f));
        }

        private void OnWeaponSwitchDetected(int weaponIndex)
        {
            if (!waitingForInput || onboardingLocked || index >= controlsDataSystem.controlsDatabase.Length) return;
            if (controlsDataSystem.controlsDatabase[index].inputType != InputType.SwitchWeapon) return;

            waitingForInput = false;
            StartCoroutine(DelayedAdvanceStep(3f));
        }

        private void OnAttackDetected()
        {
            if (!waitingForInput || onboardingLocked || index >= controlsDataSystem.controlsDatabase.Length) return;
            if (controlsDataSystem.controlsDatabase[index].inputType != InputType.Attack) return;

            waitingForInput = false;
            StartCoroutine(DelayCoroutine(3f));
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            OnWeaponSwitchAndAttackComplete?.Invoke();
        }

        private void OnLootPickupDetected()
        {
            if (!waitingForInput || onboardingLocked || index >= controlsDataSystem.controlsDatabase.Length) return;
            if (controlsDataSystem.controlsDatabase[index].inputType != InputType.LootPickup) return;

            waitingForInput = false;
            StartCoroutine(DelayedAdvanceStep(3));
        }

        private void OnAbilityEquipped()
        {
            if (!waitingForInput || onboardingLocked || index >= controlsDataSystem.controlsDatabase.Length) return;
            if (controlsDataSystem.controlsDatabase[index].inputType != InputType.AbilityEquip) return;

            waitingForInput = false;
            StartCoroutine(DelayedAdvanceStep(2));
        }

        #endregion
    }
}
