using _Project.Scripts.Core.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Enemy HUD class.
    /// </summary>
    public class EnemyHUD : MonoBehaviour
    {
        // References to the UI components
        [SerializeField] public Slider healthBar;
        [SerializeField] public EnemyController enemy;
        private void Start()
        {
            // Initialize the health bar and ammo display with the player's starting values
            UpdateHealthBar();
        }

        private void Update()
        {
            // Update the health bar and ammo display in real-time
            UpdateHealthBar();
        }

        // Updates the health bar based on the player's current and max health
        private void UpdateHealthBar()
        {
            healthBar.value = enemy.CurrentHealth;
            healthBar.maxValue = enemy.Stats.maxHealth;
        }
    }
}