using UnityEngine;

namespace _Project.Scripts.UI
{
    public class OnboardingCollisionDetection : MonoBehaviour
    {
        [SerializeField] private GameObject objectToHide;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                objectToHide.SetActive(false);
            }
        }
    }
}
