using _Project.Scripts.Core.Backend.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class DummyTarget : MonoBehaviour, IDamageable
    {
        [SerializeField] private OnboardingSequenceManager onboardingSequenceManager;
        [SerializeField] private float maxHealth = 30f;
        private float currentHealth;
        private void Awake()
        {
            currentHealth = maxHealth;
            if (onboardingSequenceManager == null)
            {
                onboardingSequenceManager = FindObjectOfType<OnboardingSequenceManager>();
                if (onboardingSequenceManager == null)
                {
                    Debug.LogError("OnboardingSequenceManager not found in scene!");
                }
            }
        }

        public void TakeDamage(IDamageable.DamageInfo damageInfo)
        {
            currentHealth -= damageInfo.Damage;
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {            
            Destroy(gameObject);
            onboardingSequenceManager?.NotifyDummyDestroyedOnShooting();
        }
    }
}
