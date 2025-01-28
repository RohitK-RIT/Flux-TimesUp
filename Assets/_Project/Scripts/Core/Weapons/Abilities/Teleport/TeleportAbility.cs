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

        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            UseTeleport();
            Used = true;
        }

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

            //SetShieldVisual(true);
            IsAbilityActive = true;
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
            //SetShieldVisual(false);
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

        /// <summary>
        /// Sets the visual representation of the shield.
        /// </summary>
        /// <param name="value">A boolean value indicating whether to activate or deactivate the shield visual.</param>
        /*private void SetShieldVisual(bool value)
        {
            _shieldVisual.SetActive(value);
        }*/
    }
}