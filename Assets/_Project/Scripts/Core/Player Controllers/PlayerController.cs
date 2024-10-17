using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Character.Weapon_Controller;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    [RequireComponent(typeof(MovementController), typeof(WeaponController))]
    public abstract class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Component that handles movement.
        /// </summary>
        private MovementController _movementController;

        /// <summary>
        /// Component that handles weapons.
        /// </summary>
        private WeaponController _weaponController;

        protected virtual void Awake()
        {
            // Get the CharacterMovement and CharacterWeaponController component attached to the player
            _movementController = GetComponent<MovementController>();
            _weaponController = GetComponent<WeaponController>();
        }

        /// <summary>
        /// Update the player's movement direction.
        /// </summary>
        /// <param name="direction">direction in which player should move</param>
        protected void UpdateMoveDirection(Vector2 direction)
        {
            _movementController.moveDirection = direction;
        }

        /// <summary>
        /// Begin the player's attack.
        /// </summary>
        protected void BeginAttack()
        {
            _weaponController.BeginAttack();
        }
        
        /// <summary>
        /// End the player's attack.
        /// </summary>
        protected void EndAttack()
        {
            _weaponController.EndAttack();
        }
    }
}