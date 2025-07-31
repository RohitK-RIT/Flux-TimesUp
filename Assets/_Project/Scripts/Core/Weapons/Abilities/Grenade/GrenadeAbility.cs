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

        public override AbilityType Type => AbilityType.Grenades;

        [SerializeField] private Grenade grenade;

        public override void BeginUse()
        {
            UseGrenadeAbility();
            Used = true;
        }


        /// <summary>
        /// Function to use grenade ability.
        /// </summary>
        private void UseGrenadeAbility()
        {
            if (isAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }

            isAbilityActive = true;

            var grenadeInstance = Instantiate(grenade, transform.position, Quaternion.identity);
            
            var forceDirection = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).direction + Vector3.up * 0.5f; // Get the direction from the camera to the center of the screen
                //CurrentPlayerController.MovementController.Body.forward + Vector3.up * 0.5f; // Throw the grenade in the forward direction
            
            grenadeInstance.ThrowGrenade(forceDirection, this);
            
            Owner.StartCoroutine(DeactivateAbility(stats.Cooldown));
            Owner.StartCoroutine(StartCooldown(stats.Cooldown));
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
        }
    }
}