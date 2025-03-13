using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities.Shield;
using UnityEngine;
using IPickupItem = _Project.Scripts.Core.Backend.Interfaces.IPickupItem;

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
        public IPickupItem CurrentPickupItem { get; private set; }

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
                if (hit.collider.TryGetComponent<IPickupItem>(out var pickupItem))
                {
                    CurrentPickupItem = pickupItem;
                    CurrentPickupItem.OnItemEnterRange();
                }
            }
            else if (CurrentPickupItem != null)
            {
                CurrentPickupItem.OnItemExitRange();
                var abilitiesInRange = Physics.OverlapSphere(transform.position, 7f, LayerMask.GetMask("Pickup"));
                foreach (var ability in abilitiesInRange)
                {
                    if (ability.TryGetComponent<IPickupItem>(out var pickupItem))
                    {
                        CurrentPickupItem = pickupItem;
                        CurrentPickupItem.OnItemExitRange();
                    }
                }

                CurrentPickupItem = null;
            }
        }

        private void OnEnable()
        {
            // Subscribe to input events
            _localInputController.OnMoveInputUpdated += SetMoveInput;

            _localInputController.OnAttackInputBegan += BeginAttack;
            _localInputController.OnAttackInputEnded += EndAttack;

            _localInputController.OnLookInputUpdated += SetLookInput;

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

            _localInputController.OnLookInputUpdated -= SetLookInput;

            _localInputController.OnAbilityEquipped -= AbilityEquipped;

            _localInputController.OnSwitchWeaponInput -= SwitchWeapon;
            _localInputController.OnReloadInput -= Reload;

            _localInputController.OnLootPickupInput -= PickUpItem;
        }

        /// <summary>
        /// Update the player's look direction.
        /// </summary>
        /// <param name="lookInput">look input to the player</param>
        private void SetLookInput(Vector2 lookInput) { }

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
        /// <param name="damageDealt"></param>
        /// <returns>if the player is dead</returns>
        public override void TakeDamage(IDamageable.DamageInfo damageDealt)
        {
            // Check if the shield ability is active, if so, return false
            var shield = HandController.CurrentAbility as ShieldAbility;
            if (shield && shield.isAbilityActive)
                return;

            // If the shield ability is not active, take damage
            base.TakeDamage(damageDealt);
        }

        private void PickUpItem()
        {
            if (CurrentPickupItem == null) return;
            CurrentPickupItem.OnItemPickup();
            CurrentPickupItem = null;
        }
    }
}