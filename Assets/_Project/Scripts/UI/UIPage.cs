using UnityEngine;

namespace _Project.Scripts.UI
{
    public abstract class UIPage : MonoBehaviour
    {
        public abstract PageID ID { get; }
        // Flag to check if the page is active
        private bool _isActive;

        /// <summary>
        /// Method to show the page
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Method to hide the page
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        // Optional: Initialization logic for the page
        public virtual void Initialize()
        {
            // Initialization logic here, if required
        }

        // Optional: Update logic for the page
        public virtual void UpdatePage()
        {
            if (_isActive)
            {
                // Update logic here, if required
            }
        }

        // Method to check if the page is active
        public bool IsActive()
        {
            return _isActive;
        }
    }
}