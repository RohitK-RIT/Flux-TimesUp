using UnityEngine;

namespace _Project.Scripts.UI
{
    public class CollisionDetection : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
