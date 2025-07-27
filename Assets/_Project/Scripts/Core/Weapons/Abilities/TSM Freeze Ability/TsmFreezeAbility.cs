using System.Collections;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.TSM_Freeze_Ability
{
    /// <summary>
    /// Represents the TSM freeze ability for the player.
    /// </summary>
    public class TsmFreezeAbility : Ability
    {
        public override AbilityType Type => AbilityType.TsmFreeze;
        
        /// <summary>
        /// The stats for the heal ability.
        /// </summary>
        [SerializeField] private TsmFreezeAbilityStats stats;
        
        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            FreezeTsm();
            Used = true;
        }

        /// <summary>
        /// Function to use the freeze TSM ability.
        /// </summary>
        private void FreezeTsm()
        {
            if (isAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            isAbilityActive = true;
            //Freeze Tsm for the player
            Debug.Log("Player is using the TSM Freeze ability!!");
            TimeStabilityMeter.Instance.PauseTimeStabilityMeter = true;
            Owner.StartCoroutine(DeactivateAbility(stats.FreezeDuration));
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
            TimeStabilityMeter.Instance.PauseTimeStabilityMeter = false;
            Debug.Log("Ability deactivated!!");
        }
    }
}
