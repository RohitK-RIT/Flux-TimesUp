using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Teleport
{
    /// <summary>
    /// Represents the teleport ability for the player.
    /// </summary>
    public class TeleportAbility : Ability
    {
        public override AbilityType Type => AbilityType.Teleport;

        /// <summary>
        /// The stats for the Teleport ability.
        /// </summary>
        [SerializeField] private TeleportAbilityStats stats;

        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            Teleport();
            Used = true;
        }
        
        /// <summary>
        /// Activates the teleport ability.
        /// </summary>
        private void Teleport()
        {
            if (IsAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            IsAbilityActive = true;
            
            // Teleport the player to the target position
            Vector3 targetPosition = CurrentPlayerController.transform.position + (-CurrentPlayerController.MovementController.Body.forward) * stats.Distance;
            
            // Perform a raycast to check that teleport does not happen through room walls. 
            RaycastHit hit;
            if (Physics.Raycast(CurrentPlayerController.transform.position, -CurrentPlayerController.MovementController.Body.forward, out hit, stats.Distance))
            {
                return; // Prevent teleportation
            }

            CurrentPlayerController.transform.position = targetPosition;
            CurrentPlayerController.StartCoroutine(DeactivateAbility(0));
        }
        
        /// <summary>
        /// Coroutine to deactivate the ability after a certain time.
        /// </summary>
        /// <param name="time">The duration after which the ability should be deactivated.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator DeactivateAbility(float time)
        {
            yield return new WaitForSeconds(time);
            Debug.Log("Ability deactivated!!");
            CurrentPlayerController.StartCoroutine(StartCooldown(stats.Cooldown));
        }

        /// <summary>
        /// Overrides the OnAttack method to provide custom attack behavior for the shield ability.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine.</returns>
        protected override IEnumerator OnAttack()
        {
            yield break;
        }
    }
}