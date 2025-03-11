using _Project.Scripts.Core.Character.Hand_Controller;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.Core.Weapons.Melee;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Animation
{
    public class AnimationController : CharacterComponent
    {
        private static readonly int Horizontal = Animator.StringToHash("DirectionX");
        private static readonly int Vertical = Animator.StringToHash("DirectionZ");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int MeleeAttack = Animator.StringToHash("Melee Attack");
        private static readonly int MeleeAttackSpeed = Animator.StringToHash("Melee Attack Speed");

        [SerializeField] private Animator animator;

        private InputController _inputController;
        private HandController _handController;
        private bool _hasMeleeWeapon;

        private void Awake()
        {
            _inputController = GetComponent<InputController>();
            _handController = GetComponent<HandController>();
        }

        public override void Initialize(PlayerController playerController)
        {
            base.Initialize(playerController);

            animator.SetFloat(Speed, playerController.Stats.movementSpeed);
        }

        private void OnEnable()
        {
            // Subscribe to events
            if (_inputController)
            {
                _inputController.OnMoveInputUpdated += OnMoveDetected;
                _inputController.OnAttackInputBegan += OnAttackBegin;
                _inputController.OnAttackInputEnded += OnAttackEnd;
            }

            if (_handController)
            {
                _handController.OnWeaponSwitched += HandSwitched;
            }
        }

        /// <summary>
        /// Unsubscribe from events to avoid memory leaks
        /// </summary>
        private void OnDisable()
        {
            // Unsubscribe from events to avoid memory leaks
            if (_inputController)
            {
                _inputController.OnMoveInputUpdated -= OnMoveDetected;
                _inputController.OnAttackInputBegan -= OnAttackBegin;
                _inputController.OnAttackInputEnded -= OnAttackEnd;
            }

            if (_handController)
            {
                _handController.OnWeaponSwitched -= HandSwitched;
            }
        }

        private void OnMoveDetected(Vector2 moveInput)
        {
            //Set Movement Blend Tree Parameters
            animator.SetFloat(Horizontal, moveInput.x);
            animator.SetFloat(Vertical, moveInput.y);
        }

        private void OnAttackBegin()
        {
            animator.SetBool(MeleeAttack, _hasMeleeWeapon);
        }

        private void OnAttackEnd()
        {
            animator.SetBool(MeleeAttack, false);
        }

        private void HandSwitched()
        {
            if (_handController.CurrentWeapon is MeleeWeapon meleeWeapon)
            {
                _hasMeleeWeapon = true;
                animator.SetFloat(MeleeAttackSpeed, meleeWeapon.Stats.AttackSpeed);
            }
            else
            {
                _hasMeleeWeapon = false;
            }
        }
    }
}