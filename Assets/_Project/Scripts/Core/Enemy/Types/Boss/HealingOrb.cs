using System.Collections;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.Types.Boss
{
    public class HealingOrb : MonoBehaviour, IDamageable
    {
        // Amount of health restored per second
        private readonly int _healAmount = 1;
        
        // Reference to the BossController
        private BossController _boss;
        
        // Current health of the orb
        [SerializeField] private float currentHealth;
        
        // Maximum health of the orb
        private readonly float _maxHealth = 300f;
    

        void Start() {
            currentHealth = _maxHealth;
            _boss = FindObjectOfType<BossController>();
            StartCoroutine(HealBoss());
        }

        // Coroutine to heal the boss over time
        IEnumerator HealBoss() {
            while (_boss != null) {
                
                // Increase boss's health
                _boss.EnemyController.currentHealth += _healAmount;
                
                // Wait for 1 second before healing again
                yield return new WaitForSeconds(1f);
            }
        }

        // Function to apply damage to the boss
        public void TakeDamage(Weapon weapon, float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, _maxHealth);

            if (currentHealth <= 0)
                gameObject.SetActive(false);
        }
    }
}
