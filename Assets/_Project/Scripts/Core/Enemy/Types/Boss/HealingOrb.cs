using System;
using System.Collections;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Weapons;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Core.Enemy.Types.Boss
{
    public class HealingOrb : MonoBehaviour, IDamageable
    {
        // Amount of health restored per second
        private readonly int _healAmount = 10;
        
        // Reference to the BossController
        private BossController _boss;
        
        // Current health of the orb
        [SerializeField] private float currentHealth;
        
        // Maximum health of the orb
        private readonly float _maxHealth = 300f;
        
        private IDamageable.DamageInfo _damageInfo;
    

        void OnEnable() {
            currentHealth = _maxHealth;
            _boss = FindObjectOfType<BossController>();
            ActivateVFX();
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
        public void TakeDamage(IDamageable.DamageInfo damageInfo)
        {
            if (gameObject.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(_damageInfo);
            currentHealth -= damageInfo.Damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, _maxHealth);

            if (currentHealth <= 0)
                gameObject.SetActive(false);
        }
        
        void ActivateVFX()
        {
            Vector3 spawnPosition = _boss.transform.position - new Vector3(0,1.0f, 0); // Adjust Y by 0.5 units down
            Instantiate(_boss.healBossVFX, spawnPosition, Quaternion.identity);
            _boss.healBossVFX.transform.SetParent(_boss.transform); // Keep VFX attached to the boss
        }

        // Deactivates the current VFX
        void DeactivateVFX()
        {
            Destroy(_boss.healBossVFX);
        }

        private void OnDisable()
        {
            DeactivateVFX();
        }
    }
}
