using System;
using System.Collections;
using _Project.Scripts.Core.Backend.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Grenade
{
    public class Grenade : MonoBehaviour
    {
        /// <summary>
        /// Grenade Explosion VFX prefab.
        /// </summary>
        [SerializeField] private GameObject explosionVFXPrefab;

        /// <summary>
        /// Rigidbody reference for physics implementation on grenade. 
        /// </summary>
        private Rigidbody grenadeRb;

        public event Action OnExploded;
        private void Awake()
        {
            grenadeRb = GetComponent<Rigidbody>();
            grenadeRb.isKinematic = true;
        }

        /// <summary>
        /// Function to throw grenade. 
        /// </summary>
        /// <param name="forceDirection"></param>
        /// <param name="grenadeAbility"></param>
        public void ThrowGrenade(Vector3 forceDirection, GrenadeAbility grenadeAbility)
        {
            //Throw Grenade Functionality
            grenadeRb.isKinematic = false;
            grenadeRb.AddForce(forceDirection * grenadeAbility.Stats.Range, ForceMode.Impulse);
            // Start grenade explosion timer
            StartCoroutine(GrenadeExplosion(grenadeAbility));
        }

        /// <summary>
        /// Grenade explosion Functionality
        /// </summary>
        private IEnumerator GrenadeExplosion(GrenadeAbility grenadeAbility)
        {
            yield return new WaitForSeconds(1f); // Delay before explosion

            // Instantiate explosion VFX
            var explosionVFX = Instantiate(explosionVFXPrefab, transform.position, transform.rotation);
            OnExploded?.Invoke();
            Destroy(explosionVFX, 1f); // Destroy explosion VFX after 1 seconds

            // Check for enemies in the attack range
            var enemiesColliders = Physics.OverlapSphere(transform.position, grenadeAbility.Stats.Radius);

            foreach (var nearbyEnemy in enemiesColliders)
            {
                var rb = nearbyEnemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(grenadeAbility.Stats.Force, transform.position, grenadeAbility.Stats.Radius);
                }

                // Deal damage to the enemies in the attack range
                var damageable = nearbyEnemy.gameObject.GetComponent<IDamageable>();
                var damageInfo = new IDamageable.DamageInfo(grenadeAbility.Stats.Damage, grenadeAbility);
                damageable?.TakeDamage(damageInfo);
            }

            // Destroy grenade object
            Destroy(gameObject);
        }
    }
}