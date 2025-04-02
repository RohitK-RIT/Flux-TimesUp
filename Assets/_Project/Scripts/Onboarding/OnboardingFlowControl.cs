using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class OnboardingFlowControl : MonoBehaviour
    {
        [SerializeField] private NarrativeManager narrativeManager;
        [SerializeField] private OnboardingManager onboardingManager;
        [SerializeField] private LevelSceneController levelSceneController;
        private void Start()
        {
            onboardingManager.gameObject.SetActive(false);
            narrativeManager.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            onboardingManager.OnLookAndMoveComplete += OnLookAndMoveComplete;
            onboardingManager.OnWeaponSwitchAndAttackComplete += OnWeaponSwitchAndAttackComplete;
            onboardingManager.OnPlayerTeleportComplete += OnPlayerTeleportComplete;
            onboardingManager.OnPlayerLootControlsComplete += OnPlayerLootControlsComplete;
        }

        public void OnContinueClickedFromNarrative()
        {
            narrativeManager.gameObject.SetActive(false);
            onboardingManager.gameObject.SetActive(true);
        }

        private void OnLookAndMoveComplete()
        {
            onboardingManager.OnLookAndMoveComplete -= OnLookAndMoveComplete;
            narrativeManager.gameObject.SetActive(true);
            onboardingManager.gameObject.SetActive(false);
            levelSceneController.gameObject.SetActive(true);
            narrativeManager.OnContinue();
        }

        private void OnWeaponSwitchAndAttackComplete()
        {
            onboardingManager.OnWeaponSwitchAndAttackComplete -= OnWeaponSwitchAndAttackComplete;
            narrativeManager.gameObject.SetActive(true);
            onboardingManager.gameObject.SetActive(false);
            levelSceneController.playerHUD.timeStabilityBar.gameObject.SetActive(true);
            narrativeManager.OnContinue();
        }
        
        private void OnPlayerTeleportComplete()
        {
            onboardingManager.OnPlayerTeleportComplete -= OnPlayerTeleportComplete;
            narrativeManager.gameObject.SetActive(true);
            onboardingManager.gameObject.SetActive(false);
            narrativeManager.OnContinue();
        }
        
        private void OnPlayerLootControlsComplete()
        {
            onboardingManager.OnPlayerLootControlsComplete -= OnPlayerLootControlsComplete;
            narrativeManager.gameObject.SetActive(true);
            onboardingManager.gameObject.SetActive(false);
            narrativeManager.OnContinue();
        }
    }
}
