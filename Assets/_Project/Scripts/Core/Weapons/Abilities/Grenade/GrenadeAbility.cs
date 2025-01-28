using System.Collections;
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
        
        public override AbilityType Type => AbilityType.Attack;
        
        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            UseGrenade();
            Used = true;
        }
        /// <summary>
        /// Function to use the attack ability using grenade.
        /// </summary>
        private void UseGrenade()
        {
            if (IsAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }

            IsAbilityActive = true;
            Debug.Log("Player is using the heal ability!!");
            //CurrentPlayerController.Heal(stats.HealValue);
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
