using System;
using System.Linq;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.Core.Weapons.Abilities.Shield;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    /// <summary>
    /// This class is responsible for handling the player's input.
    /// </summary>
    [RequireComponent(typeof(LocalInputController), typeof(PlayerAimController))]
    public sealed class LocalPlayerController : PlayerController
    {
        /// <summary>
        /// The current pickup item the player has.
        /// </summary>
        public IPickup CurrentPickup { get; private set; }

        public override string FriendlyLayerName => "Player";
        public override string OpponentLayerName => "Enemy";

        /// <summary>
        /// Component that handles player input.
        /// </summary>
        private LocalInputController _localInputController;

        /// <summary>
        /// Component that handles the player's camera.
        /// </summary>
        private PlayerAimController _playerAimController;

        // This will go in player info eventually.
        [SerializeField] private float aimSensitivity = 1f;

        protected override void Awake()
        {
            base.Awake();

            // Get the required components
            _localInputController = GetComponent<LocalInputController>();
            _playerAimController = GetComponent<PlayerAimController>();
        }

        protected override void Start()
        {
            base.Start();

            // Initialize the input controller and camera controller
            _localInputController.Initialize(this);
            _playerAimController.Initialize(this);
        }

        private void Update()
        {
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out var hit, 8f,
                    LayerMask.GetMask("Pickup")))
            {
                if (hit.collider.TryGetComponent<IPickup>(out var pickupItem))
                {
                    CurrentPickup = pickupItem;
                    CurrentPickup.OnHoverEnter();
                }
            }
            else if (CurrentPickup != null)
            {
                CurrentPickup.OnHoverExit();
                var abilitiesInRange = Physics.OverlapSphere(transform.position, 7f, LayerMask.GetMask("Pickup"));
                foreach (var ability in abilitiesInRange)
                {
                    if (ability.TryGetComponent<IPickup>(out var pickupItem))
                    {
                        CurrentPickup = pickupItem;
                        CurrentPickup.OnHoverExit();
                    }
                }

                CurrentPickup = null;
            }
        }

        private void OnEnable()
        {
            // Subscribe to input events
            _localInputController.OnMoveInputUpdated += SetMoveInput;

            _localInputController.OnAttackInputBegan += BeginAttack;
            _localInputController.OnAttackInputEnded += EndAttack;

            _localInputController.OnAbilityEquipped += AbilityEquipped;

            _localInputController.OnSwitchWeaponInput += SwitchWeapon;
            _localInputController.OnReloadInput += Reload;

            _localInputController.OnLootPickupInput += PickUpItem;
        }

        private void OnDisable()
        {
            // Unsubscribe from input events
            _localInputController.OnMoveInputUpdated -= SetMoveInput;

            _localInputController.OnAttackInputBegan -= BeginAttack;
            _localInputController.OnAttackInputEnded -= EndAttack;

            _localInputController.OnAbilityEquipped -= AbilityEquipped;

            _localInputController.OnSwitchWeaponInput -= SwitchWeapon;
            _localInputController.OnReloadInput -= Reload;

            _localInputController.OnLootPickupInput -= PickUpItem;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ICollectible>(out var collectible))
                collectible.OnCollected(this);
        }

        /// <summary>
        /// Function to equip the player's ability.
        /// </summary>
        private void AbilityEquipped()
        {
            HandController.OnAbilityEquipped();
        }

        /// <summary>
        /// Overrides the TakeDamage method to include shield ability check.
        /// </summary>
        /// <param name="damageInfo"></param>
        /// <returns>if the player is dead</returns>
        public override void TakeDamage(IDamageable.DamageInfo damageInfo)
        {
            // Check if the attacker is not null, (which means that TSM is killing the player)
            if (damageInfo.Attacker)
            {
                // Check if the shield ability is active, if so, return false
                var shield = HandController.CurrentAbility as ShieldAbility;
                if (shield && shield.isAbilityActive)
                    return;
            }

            // If the shield ability is not active, take damage
            base.TakeDamage(damageInfo);
        }

        private void PickUpItem()
        {
            if (CurrentPickup == null) return;
            CurrentPickup.OnPickup();
            CurrentPickup = null;
        }
    }
}