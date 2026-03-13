using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class CrosshairTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject crosshair;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (crosshair != null)
                {
                    crosshair.SetActive(true);
                }
            }
        }
    }
}
