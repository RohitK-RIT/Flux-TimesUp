using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Backend.Scene_Control;
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
        /// The friendly layer name for the player.
        /// </summary>
        public override string FriendlyLayerName => "Player";

        /// <summary>
        /// The enemy layer name for the player.
        /// </summary>
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

        /// <summary>
        /// The center of the viewport.
        /// </summary>
        private static readonly Vector3 ViewportCenter = new(0.5f, 0.5f, 0f);

        /// <summary>
        /// The camera used for the player.
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// The property that gets or sets the current pickable item.
        /// </summary>
        private IPickable CurrentPickable
        {
            get => _currentPickable;
            set
            {
                _currentPickable?.OnHoverExit();
                _currentPickable = value;
                _currentPickable?.OnHoverEnter(this);
            }
        }

        /// <summary>
        /// The current pickable item the player is interacting with.
        /// </summary>
        private IPickable _currentPickable;

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

            // Set the camera to the main camera
            _camera = LevelSceneController.Instance.Camera;
        }

        private void Update()
        {
            UpdatePickable();
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

        private void UpdatePickable()
        {
            if (Physics.Raycast(_camera.ViewportPointToRay(ViewportCenter),
                    out var hit,
                    8f,
                    LayerMask.GetMask("Pickup")))
            {
                if (hit.collider.TryGetComponent<IPickable>(out var pickable))
                    CurrentPickable = pickable;
            }
            else
            {
                CurrentPickable = null;
            }
        }

        private void PickUpItem()
        {
            if (CurrentPickable == null) return;
            CurrentPickable.OnPickup(this);
            CurrentPickable = null;
        }
    }
}