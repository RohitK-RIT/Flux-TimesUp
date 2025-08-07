using System.Collections;
using _Project.Scripts.Core.Backend.Helper;
using _Project.Scripts.Core.Enemy.GroupEnemyBehavior;
using UnityEngine;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons.Ranged;

namespace _Project.Scripts.Core.Weapons.Abilities.Shield
{
    /// <summary>
    /// Represents the shield ability for the player.
    /// </summary>
    public class ShieldAbility : Ability
    {
        public override AbilityType Type => AbilityType.Shield;

        /// <summary>
        /// The stats for the shield ability.
        /// </summary>
        [SerializeField] private ShieldAbilityStats stats;

        /// <summary>
        /// The visual representation of the shield.
        /// </summary>
        [SerializeField] private GameObject shieldVisualPrefab;

        /// <summary>
        /// GameObject to represent the shield.
        /// </summary>
        private GameObject _shieldVisual;
        private AudioSource _abilityAudioSource;
        private AudioPlayer _audioPlayer;

        private void Awake()
        {
            _abilityAudioSource = GetComponent<AudioSource>();
            _audioPlayer = GetComponent<AudioPlayer>();
        }

        public override void OnPickup(PlayerController owner)
        {
            base.OnPickup(owner);

            // Instantiate the shield visual and set the shield visual as a child of the player.
            _shieldVisual = Instantiate(shieldVisualPrefab, owner.transform);
            _shieldVisual.gameObject.SetLayerRecursively( Owner.FriendlyLayerName);
            SetShieldVisual(false);
        }

        public override void OnDrop()
        {
            base.OnDrop();

            // Destroy the shield visual.
            Destroy(_shieldVisual);
        }

        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            DataCollectionEvents.AbilityUsed();
            _audioPlayer.PlayAbilityClip(_abilityAudioSource);
            Shield();
            Used = true;
        }

        /// <summary>
        /// Activates the shield ability.
        /// </summary>
        private void Shield()
        {
            if (isAbilityActive || IsCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }

            SetShieldVisual(true);
            isAbilityActive = true;
            Owner.StartCoroutine(DeactivateAbility(stats.Duration));
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
            isAbilityActive = false;
            SetShieldVisual(false);
        }

        /// <summary>
        /// Sets the visual representation of the shield.
        /// </summary>
        /// <param name="value">A boolean value indicating whether to activate or deactivate the shield visual.</param>
        private void SetShieldVisual(bool value)
        {
            _shieldVisual.SetActive(value);
        }
    }
}