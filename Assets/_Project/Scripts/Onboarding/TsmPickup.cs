using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class TsmPickup : MonoBehaviour
    {
        [SerializeField] private GameObject tsm;
        [SerializeField] private string playerTag = "Player";

        private bool _isPlayerInRange = false;

        private void Update()
        {
            if (_isPlayerInRange && Input.GetKeyDown(KeyCode.F))
            {
                Pickup();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                _isPlayerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                _isPlayerInRange = false;
            }
        }

        private void Pickup()
        {
            Debug.Log("Picked up TSM!");
            if (tsm != null)
                tsm.SetActive(true);
            gameObject.SetActive(false); 
        }
       
    }
}
