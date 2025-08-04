using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class PromptTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject promptUI;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (promptUI != null)
                {
                    promptUI.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Prompt UI is not assigned in the inspector.");
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (promptUI != null)
                {
                    promptUI.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Prompt UI is not assigned in the inspector.");
                }
            }
        }
    }
}
