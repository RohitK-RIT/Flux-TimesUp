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
        /// The stats for the shield ability.
        /// </summary>
        [SerializeField] private TeleportAbilityStats stats;

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            UseTeleport();
            Used = true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Activates the teleport ability.
        /// </summary>
        private void UseTeleport()
        {
            if (IsAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            IsAbilityActive = true;
            
            // Teleport the player to the target position
            Vector3 targetPosition = GetTeleportTargetPosition();
            CurrentPlayerController.transform.position = targetPosition;
            
            Debug.Log("Player teleported to: " + targetPosition);
            
            CurrentPlayerController.StartCoroutine(DeactivateAbility(0));
        }
        
        /// <summary>
        /// Function to determine the target position for the teleport ability.
        /// </summary>
        private Vector3 GetTeleportTargetPosition()
        {
            Vector3 targetPosition = CurrentPlayerController.transform.position + CurrentPlayerController.MovementController.AimTransform.forward * stats.Distance;
            return targetPosition;
        }

        // ReSharper disable Unity.PerformanceAnalysis
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