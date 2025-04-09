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
                var player = LevelSceneController.Instance.Player;
                player.MovementController.SetPosition(destination.position);
                onboardingManager.OnPlayerEnteredPortal();
            }
        }
    }
}
