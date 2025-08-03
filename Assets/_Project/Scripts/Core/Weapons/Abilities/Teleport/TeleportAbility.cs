using System.Collections;
using _Project.Scripts.Core.Weapons.Ranged;
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
        
        private AudioSource _abilityAudioSource;
        private AudioPlayer _audioPlayer;

        private void Awake()
        {
            _abilityAudioSource = GetComponent<AudioSource>();
            _audioPlayer = GetComponent<AudioPlayer>();
        }
        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            _audioPlayer.PlayAbilityClip(_abilityAudioSource);
            Teleport();
            Used = true;
        }

        /// <summary>
        /// Activates the teleport ability.
        /// </summary>
        private void Teleport()
        {
            if (isAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }

            isAbilityActive = true;
            // Teleport the player to the target position
            Vector3 targetPosition = Owner.transform.position + (-Owner.MovementController.Body.forward) * stats.Distance;

            // Perform a raycast to check that teleport does not happen through room walls. 
            RaycastHit hit;
            if (Physics.Raycast(Owner.transform.position, -Owner.MovementController.Body.forward, out hit, stats.Distance))
            {
                return; // Prevent teleportation
            }

            Owner.transform.position = targetPosition;
            Owner.StartCoroutine(DeactivateAbility(0));
            Owner.StartCoroutine(StartCooldown(stats.Cooldown));
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
        }
    }
}