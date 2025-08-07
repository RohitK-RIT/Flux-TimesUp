using _Project.Scripts.Core.Character.Hand_Controller;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Character.Animation
{
    public class AnimationController : CharacterComponent
    {
        private static readonly int Horizontal = Animator.StringToHash("DirectionX");
        private static readonly int Vertical = Animator.StringToHash("DirectionZ");
        private static readonly int Speed = Animator.StringToHash("Move Speed");
        private static readonly int MeleeAttack = Animator.StringToHash("Melee Attack");
        private static readonly int MeleeAttackSpeed = Animator.StringToHash("Melee Attack Speed");
        private static readonly int MeleeAttackCombo = Animator.StringToHash("Melee Attack Combo");

        private const string MeleeLayer = "Melee Layer";

        [SerializeField] private Animator animator;

        private InputController _inputController;
        private HandController _handController;
        private bool _hasMeleeWeapon;

        private Vector2 _targetMoveInput;
        private Vector2 _animatorMoveInput;

        private int _meleeLayerIndex;

        protected override void Awake()
        {
            base.Awake();

            _inputController = GetComponent<InputController>();
            _handController = GetComponent<HandController>();

            if (animator)
            {
                _meleeLayerIndex = animator.GetLayerIndex(MeleeLayer);
                animator.SetLayerWeight(_meleeLayerIndex, 0f);
            }
        }

        private void Start()
        {
            animator.SetFloat(Speed, PlayerController.Stats.movementSpeed);
        }

        private void OnEnable()
        {
            // Subscribe to events
            if (_inputController)
            {
                _inputController.OnMoveInputUpdated += OnMoveDetected;
            }

            if (_handController)
            {
                _handController.OnItemSwitched += OnItemSwitched;
                _handController.OnItemPicked += OnItemPicked;
                _handController.OnItemDropped += OnItemDropped;
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
            }

            if (_handController)
            {
                _handController.OnItemSwitched -= OnItemSwitched;
                _handController.OnItemPicked -= OnItemPicked;
                _handController.OnItemDropped -= OnItemDropped;
            }
        }

        private void OnMeleeAttackBegin()
        {
            animator.SetFloat(MeleeAttackCombo, Random.Range(0, 3));
            animator.SetTrigger(MeleeAttack);
        }

        private void OnItemSwitched()
        {
            if (_handController.CurrentItem is MeleeWeapon meleeWeapon)
            {
                animator.SetLayerWeight(_meleeLayerIndex, 1f);
            }
            else
            {
                animator.SetLayerWeight(_meleeLayerIndex, 0f);
            }
        }

        private void OnItemPicked(IHandItem item)
        {
            switch (item)
            {
                case MeleeWeapon meleeWeapon:
                    animator.SetFloat(MeleeAttackSpeed, meleeWeapon.Stats.AttackSpeed * 2.2f);
                    meleeWeapon.OnAttackBegin += OnMeleeAttackBegin;
                    break;
                case RangedWeapon rangedWeapon:
                    break;
            }
        }

        private void OnItemDropped(IHandItem item)
        {
            switch (item)
            {
                case MeleeWeapon meleeWeapon:
                    meleeWeapon.OnAttackBegin -= OnMeleeAttackBegin;
                    break;
                case RangedWeapon rangedWeapon:
                    break;
            }
        }

        private void OnMoveDetected(Vector2 moveInput)
        {
            _targetMoveInput = moveInput;
        }

        private void Update()
        {
            _animatorMoveInput = Vector2.Lerp(_animatorMoveInput, _targetMoveInput, Time.deltaTime * 12f);
            if (Vector2.Distance(_animatorMoveInput, _targetMoveInput) < 0.01f)
                _animatorMoveInput = _targetMoveInput;

            //Set Movement Blend Tree Parameters
            animator.SetFloat(Horizontal, _animatorMoveInput.x);
            animator.SetFloat(Vertical, _animatorMoveInput.y);
        }
    }
}