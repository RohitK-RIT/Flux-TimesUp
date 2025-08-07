using _Project.Scripts.Core.Backend.Helper;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Character.Animation;
using _Project.Scripts.Core.Character.Hand_Controller;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    /// <summary>
    /// Base class for player controllers.
    /// </summary>
    [RequireComponent(typeof(MovementController), typeof(HandController), typeof(AnimationController))]
    [RequireComponent(typeof(IKController))]
    public abstract class PlayerController : MonoBehaviour, IDamageable
    {
        public delegate void PlayerDeath(PlayerController attacker, PlayerController deadPlayer, IHandItem itemKilledBy);

        /// <summary>
        /// Event that is triggered when a player dies.
        /// </summary>
        public static event PlayerDeath OnDeath;

        /// <summary>
        /// Component that handles movement.
        /// </summary>
        public MovementController MovementController { get; private set; }

        /// <summary>
        /// Property to access the weapon controller.
        /// </summary>
        public HandController HandController { get; private set; }

        /// <summary>
        /// Property to access the animation controller.
        /// </summary>
        public AnimationController AnimationController { get; private set; }

        public IKController IKController { get; private set; }

        /// <summary>
        /// Property to access the char stats.
        /// </summary>
        public CharacterStats Stats => stats;

        /// <summary>
        /// Property to access the player's current health.
        /// </summary>
        public float CurrentHealth => currentHealth;

        /// <summary>
        /// Property to access the friendly layer.
        /// </summary>
        public LayerMask FriendlyLayer => friendlyLayer;

        /// <summary>
        /// Property to access the enemy layer.
        /// </summary>
        public LayerMask OpponentLayer => opponentLayer;

        public abstract string FriendlyLayerName { get; }
        public abstract string OpponentLayerName { get; }

        /// <summary>
        /// Component that handles Character Stats.
        /// </summary>
        [SerializeField] private CharacterStats stats;

        /// <summary>
        /// Layer mask for the friendly.
        /// </summary>
        [SerializeField] private LayerMask friendlyLayer;

        /// <summary>
        /// Layer mask for the enemy.
        /// </summary>
        [SerializeField] private LayerMask opponentLayer;

        /// <summary>
        /// Player's current health.
        /// </summary>
        [SerializeField] internal float currentHealth;

        protected virtual void Awake()
        {
            // Get the MovementController, WeaponController and AnimationController component attached to the player
            MovementController = GetComponent<MovementController>();
            HandController = GetComponent<HandController>();
            AnimationController = GetComponentInChildren<AnimationController>();
            IKController = GetComponent<IKController>();
        }

        protected virtual void Start()
        {
            // Initialize the player's health
            currentHealth = Stats.maxHealth;
            gameObject.SetLayerRecursively(FriendlyLayerName);
        }

        /// <summary>
        /// Update the player's movement direction.
        /// </summary>
        /// <param name="direction">direction in which player should move</param>
        protected void SetMoveInput(Vector2 direction)
        {
            MovementController.MoveInput = direction;
        }

        /// <summary>
        /// Switch the player's weapon.
        /// </summary>
        /// <param name="direction">the number by which the weapon is supposed to switch</param>
        protected virtual void SwitchWeapon(int direction)
        {
            HandController.SwitchWeapon(direction);
        }
        
        /// <summary>
        /// Switch the player's weapon using Hotkey.
        /// </summary>
        /// <param name="slot">the number by which the weapon is supposed to switch</param>
        protected virtual void SwitchWeaponHotKey(int slot)
        {
            HandController.SwitchWeaponUsingHotkey(slot);
        }

        protected virtual void Reload()
        {
            HandController.ReloadWeapon();
        }

        /// <summary>
        /// Begin the player's attack.
        /// </summary>
        protected void BeginAttack()
        {
            HandController.BeginAttack();
        }

        /// <summary>
        /// End the player's attack.
        /// </summary>
        protected void EndAttack()
        {
            HandController.EndAttack();
        }

        /// <summary>
        /// Function to take damage by reducing the stat's value.
        /// </summary>
        /// <param name="damageInfo">damage dealt by the weapon</param>
        public virtual void TakeDamage(IDamageable.DamageInfo damageInfo)
        {
            currentHealth -= damageInfo.Damage;
            currentHealth = Mathf.Clamp(currentHealth, 0f, Stats.maxHealth);

            if (currentHealth <= 0)
                Die(damageInfo.Attacker, damageInfo.Item);
        }

        /// <summary>
        /// Function to heal character's health by increasing the stat's value.
        /// </summary>
        /// <param name="healAmount"></param>
        public void Heal(int healAmount)
        {
            currentHealth += healAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, Stats.maxHealth);
        }

        /// <summary>
        /// Function to handle the character's death.
        /// </summary>
        /// <param name="enemyPlayer"></param>
        /// <param name="itemKilledBy"></param>
        protected virtual void Die(PlayerController enemyPlayer, IHandItem itemKilledBy)
        {
            OnDeath?.Invoke(enemyPlayer, this, itemKilledBy);
        }
    }
}