using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities.Shield;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    /// <summary>
    /// This class is responsible for handling the player's input.
    /// </summary>
    [RequireComponent(typeof(LocalInputController))]
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
        private CameraController _cameraController;

        // This will go in player info eventually.
        [SerializeField] private float aimSensitivity = 1f;

        /// <summary>
        /// The wallet ID for the player.
        /// </summary>
        //private string _walletID;

        protected override void Awake()
        {
            base.Awake();

            // Get the required components
            _localInputController = GetComponent<LocalInputController>();
            _cameraController = GetComponent<CameraController>();
        }

        protected override void Start()
        {
            base.Start();

            // Initialize the input controller and camera controller
            _localInputController.Initialize(this);
            _cameraController.Initialize(this);

            // Create a wallet for the player
            //_walletID = CurrencySystem.Instance.CreateWallet();
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
        }

        /// <summary>
        /// Update the player's look direction.
        /// </summary>
        /// <param name="lookInput">look input to the player</param>
        private void SetLookInput(Vector2 lookInput)
        {
            _cameraController.LookInput = lookInput * aimSensitivity;
        }

        /// <summary>
        /// Function to equip the player's ability.
        /// </summary>
        private void AbilityEquipped()
        {
            WeaponController.OnAbilityEquipped();
        }

        /// <summary>
        /// Overrides the TakeDamage method to include shield ability check.
        /// </summary>
        /// <param name="weapon"></param>
        /// <param name="damageDealt"></param>
        /// <returns>if the player is dead</returns>
        public override void TakeDamage(Weapon weapon, float damageDealt)
        {
            // Check if the shield ability is active, if so, return false
            var shield = WeaponController.CurrentAbility as ShieldAbility;
            if (shield && shield.IsActive)
                return;

            // If the shield ability is not active, take damage
            base.TakeDamage(weapon, damageDealt);
        }

        /// <summary>
        /// Called when an enemy is killed.
        /// </summary>
        /// <param name="enemyPlayer"></param>
        protected override void OnKillConfirmed(PlayerController enemyPlayer)
        {
            // Cast the enemyPlayer to an enemy controller
            if (enemyPlayer is EnemyController enemyController)
            {
                // Add coins to the player's wallet
                //CurrencySystem.Instance.AddCoins(_walletID, 10); // 10 coins for now, eventually this will be based on enemy type
            }
        }

        /// <summary>
        /// Called when an enemy is hit.
        /// </summary>
        protected override void OnHitConfirmed(PlayerController enemyPlayer)
        {
            // Empty for now
        }

        /// <summary>
        /// Get the player's coins.
        /// </summary>
        /// <returns></returns>
        public int GetCoins()
        {
            //return CurrencySystem.Instance.GetCoins(_walletID);
            return 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if player has picked up a pickup
            if (other.TryGetComponent(out IPickupItem pickupItem))
            {
                // Call the OnPickup method
                pickupItem.OnItemPickup();
                CurrentPickupItem = pickupItem;
            }
        }
    }
}