using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Grenade
{
    /// <summary>
    /// Represents the grenade ability for the player.
    /// </summary>
    public class GrenadeAbility : Ability
    {
        /// <summary>
        /// The stats for the heal ability.
        /// </summary>
        [SerializeField] private GrenadeAbilityStats stats;
        
        /// <summary>
        /// Grenade Explosion VFX prefab.
        /// </summary>
        [SerializeField] private GameObject explosionVFXPrefab;
        
        public override AbilityType Type => AbilityType.Attack;
        
        Rigidbody grenadeRb;

        public override void OnEquip()
        {
            base.OnEquip();
            grenadeRb = GetComponent<Rigidbody>();
            grenadeRb.isKinematic = true;
            transform.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Function to use the attack ability using grenade.
        /// </summary>
        private void Grenade()
        {
            if (IsAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            IsAbilityActive = true;
            
            //Throw Grenade Functionality
            grenadeRb.isKinematic = false;
            var forceDirection = CurrentPlayerController.MovementController.Body.forward
                                 + Vector3.up * 0.5f; // Throw the grenade in the forward direction
            grenadeRb.AddForce(forceDirection * stats.Range, ForceMode.Impulse);
            
            // Start grenade explosion timer
            StartCoroutine(GrenadeExplosion(CurrentPlayerController.WeaponController.CurrentWeapon));
            CurrentPlayerController.StartCoroutine(DeactivateAbility(0));
        }
        
        /// <summary>
        /// Grenade explosion Functionality
        /// </summary>
        private IEnumerator GrenadeExplosion(Weapon grenade)
        {
            yield return new WaitForSeconds(1f); // Delay before explosion
            
            // Instantiate explosion VFX
            var explosionVFX = Instantiate(explosionVFXPrefab, grenade.transform.position, grenade.transform.rotation);
            Destroy(explosionVFX, 1f); // Destroy explosion VFX after 1 seconds
            
            // Check for enemies in the attack range
            var enemiesColliders = Physics.OverlapSphere(grenade.transform.position, stats.Radius);
            
            foreach(var nearbyEnemy in enemiesColliders)
            {
                var rb = nearbyEnemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(stats.Force, grenade.transform.position, stats.Radius);
                }
                
                // Deal damage to the enemies in the attack range
                var playerController = nearbyEnemy.gameObject.GetComponent<PlayerController>();
                playerController?.TakeDamage(this, stats.Damage);
            }

            // Destroy grenade object
            grenade.gameObject.SetActive(false);
        }

        /// <summary>
        /// Coroutine to deactivate the ability after a certain time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator DeactivateAbility(float time)
        {
            yield return new WaitForSeconds(time);
            Debug.Log("Ability deactivated!!");
            CurrentPlayerController.StartCoroutine(StartCooldown(stats.Cooldown));
        }

        protected override IEnumerator OnAttack()
        {
            Grenade();
            //Used = true;
            yield break;
        }
    }
}
