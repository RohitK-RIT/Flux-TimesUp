using System.Collections;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class ShowControls : MonoBehaviour
    {
        // Time in seconds before disabling the GameObject
        public float disableTime = 5f;

        private void Start()
        {
            Invoke(nameof(DisableAfterDelay), disableTime);
        }

       
        private void DisableAfterDelay()
        {
            // Disable the GameObject
            gameObject.SetActive(false);
        }
    }
}
