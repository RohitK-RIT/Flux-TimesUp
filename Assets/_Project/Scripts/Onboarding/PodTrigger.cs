using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class PodTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject portal;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!portal)
                    return;
                portal.SetActive(true);
            }
        }
    }
}
