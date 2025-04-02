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
        [SerializeField] private GameObject portal;
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
            if(onboardingManager.OnboardingIndex >= onboardingManager.controlsDataSystem.controlsDatabase.Length)
            {
                narrativeManager.gameObject.SetActive(false);
                onboardingManager.gameObject.SetActive(false);
                return;
            }
            narrativeManager.gameObject.SetActive(false);
            onboardingManager.gameObject.SetActive(true);
        }

        private void OnLookAndMoveComplete()
        {
            onboardingManager.OnLookAndMoveComplete -= OnLookAndMoveComplete;
            levelSceneController.gameObject.SetActive(true);
            MoveToNextOnboardingPhase();
        }

        private void OnWeaponSwitchAndAttackComplete()
        {
            onboardingManager.OnWeaponSwitchAndAttackComplete -= OnWeaponSwitchAndAttackComplete;
            levelSceneController.playerHUD.timeStabilityBar.gameObject.SetActive(true);
            MoveToNextOnboardingPhase();
            portal.SetActive(true);
        }
        
        private void OnPlayerTeleportComplete()
        {
            onboardingManager.OnPlayerTeleportComplete -= OnPlayerTeleportComplete;
            MoveToNextOnboardingPhase();
            TimeStabilityMeter.Instance.PauseTimeStabilityMeter = false;
        }
        
        private void OnPlayerLootControlsComplete()
        {
            onboardingManager.OnPlayerLootControlsComplete -= OnPlayerLootControlsComplete;
            MoveToNextOnboardingPhase();
        }
        
        public void MoveToNextOnboardingPhase()
        {
            narrativeManager.gameObject.SetActive(true);
            onboardingManager.gameObject.SetActive(false);
            narrativeManager.OnContinue();
        }
    }
}
