using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class OnboardingFlowControl : MonoBehaviour
    {
        [SerializeField] private NarrativeManager narrativeManager;
        [SerializeField] private OnboardingManager onboardingManager;

        private void Start()
        {
            onboardingManager.gameObject.SetActive(false);
            narrativeManager.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            onboardingManager.OnLookAndMoveComplete += OnLookAndMoveComplete;
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
            narrativeManager.OnContinue();
        }
    }
}
