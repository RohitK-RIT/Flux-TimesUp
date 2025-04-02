using _Project.Scripts.Core.Backend.Scene_Control;
using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class PortalTeleportation : MonoBehaviour
    {
        [SerializeField] private Transform destination;
        [SerializeField] private OnboardingManager onboardingManager;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered portal" + destination.position);
                var player = LevelSceneController.Instance.Player;
                CharacterController cc = player.GetComponent<CharacterController>();

                if (cc != null)
                {
                    cc.enabled = false; 
                }

                player.transform.position = destination.position;

                if (cc != null)
                {
                    cc.enabled = true;
                }
                onboardingManager.OnPlayerEnteredPortal();
            }
        }
    }
}
