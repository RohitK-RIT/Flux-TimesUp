using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Heal
{
    /// <summary>
    /// Represents the heal ability for the player.
    /// </summary>
    public class HealAbility : Ability
    {
        public override AbilityType Type => AbilityType.Heal;

        /// <summary>
        /// The stats for the heal ability.
        /// </summary>
        [SerializeField] private HealAbilityStats stats;

        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            Heal();
            Used = true;
        }

        /// <summary>
        /// Function to use the heal ability.
        /// </summary>
        private void Heal()
        {
            if (IsAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }

            IsAbilityActive = true;
            //Heal the player
            Debug.Log("Player is using the heal ability!!");
            CurrentPlayerController.Heal(stats.HealValue);
            CurrentPlayerController.StartCoroutine(DeactivateAbility(0));
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
            yield break;
        }
    }
}