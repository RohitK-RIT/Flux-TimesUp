using _Project.Scripts.Core.Backend.Scene_Control;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Onboarding
{
    public class PortalTeleportation : MonoBehaviour
    {
        [SerializeField] private string targetSceneName = "UI";
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SceneSystem.Instance.LoadScene(new SceneLoadRequest(targetSceneName, LoadSceneMode.Single));
            }
        }
    }
}
