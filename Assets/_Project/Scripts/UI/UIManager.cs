using System.Collections.Generic;
using _Project.Scripts.Core.Backend;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class UIManager : BaseSystem<UIManager>
    {
        protected override bool IsPersistent => false;
        [SerializeField] private PageID startingPage = PageID.MainMenu;

        // Dictionary to store all UI pages
        private Dictionary<PageID, UIPage> _pages;

        // Reference to the current page
        private UIPage _currentPage;

        private void Start()
        {
            // Initialize the dictionary
            _pages = new Dictionary<PageID, UIPage>();

            // Find all UI pages in the scene
            var allPages = FindObjectsOfType<UIPage>(true);
            foreach (var page in allPages)
            {
                page.Initialize();
                _pages.Add(page.ID, page); // Add the page to the dictionary
                if (page.ID == startingPage)
                    continue;

                page.Hide(); // Hide the page
                _currentPage = page;
            }
        }

        // Method to switch between pages
        public void SwitchPage(PageID id)
        {
            if (!_pages.TryGetValue(id, out var page))
            {
                Debug.LogError($"UI Page {id} not found!");
                return;
            }

            if (_currentPage.ID != startingPage)
                _currentPage?.Hide();

            _currentPage = page;
            _currentPage.Show();
        }

        // Update method to update the current page
        private void Update()
        {
            _currentPage?.UpdatePage();
        }
    }
}