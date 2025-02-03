using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Grenade
{
    /// <summary>
    /// Represents the grenade ability for the player.
    /// </summary>
    public class GrenadeAbility : Ability
    {
        // Property to reference Grenade Ability stats.
        public GrenadeAbilityStats Stats => stats;
        
        /// <summary>
        /// The stats for the heal ability.
        /// </summary>
        [SerializeField] private GrenadeAbilityStats stats;
        public override AbilityType Type => AbilityType.Attack;

        [SerializeField] private Grenade grenade;
        
        /// <summary>
        /// Function to use grenade ability.
        /// </summary>
        private void UseGrenadeAbility()
        {
            if (IsAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            IsAbilityActive = true;

            var grenadeInstance = Instantiate(grenade, transform.position, Quaternion.identity);
            
            var forceDirection = CurrentPlayerController.MovementController.Body.forward
                                 + Vector3.up * 0.5f; // Throw the grenade in the forward direction
            
            grenadeInstance.ThrowGrenade(forceDirection, this);
            
            CurrentPlayerController.StartCoroutine(DeactivateAbility(stats.Cooldown));
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
            UseGrenadeAbility();
            Used = true;
            yield break;
        }
    }
}
