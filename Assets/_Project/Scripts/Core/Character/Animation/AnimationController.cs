using System.Collections;
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
        private static readonly int ReloadSpeed = Animator.StringToHash("Reload Speed");
        private static readonly int Reloading = Animator.StringToHash("Reloading");

        private const string MeleeLayer = "Melee Layer";
        private const string GunLayer = "Gun Layer";

        [SerializeField] private Animator animator;

        private InputController _inputController;
        private HandController _handController;

        private int _meleeLayerIndex;
        private int _gunLayerIndex;

        private Vector2 _targetMoveInput;
        private Vector2 _animatorMoveInput;

        private float _gunReloadTime;

        protected override void Awake()
        {
            base.Awake();

            _inputController = GetComponent<InputController>();
            _handController = GetComponent<HandController>();

            if (animator)
            {
                _meleeLayerIndex = animator.GetLayerIndex(MeleeLayer);
                _gunLayerIndex = animator.GetLayerIndex(GunLayer);
                animator.SetLayerWeight(_meleeLayerIndex, 0f);
                animator.SetLayerWeight(_gunLayerIndex, 0f);
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
            switch (_handController.CurrentItem)
            {
                case MeleeWeapon:
                    animator.SetLayerWeight(_meleeLayerIndex, 1f);
                    animator.SetLayerWeight(_gunLayerIndex, 0f);
                    break;
                case RangedWeapon:
                    animator.SetLayerWeight(_meleeLayerIndex, 0f);
                    animator.SetLayerWeight(_gunLayerIndex, 1f);
                    break;
                default:
                    animator.SetLayerWeight(_meleeLayerIndex, 0f);
                    animator.SetLayerWeight(_gunLayerIndex, 0f);
                    break;
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
                    _gunReloadTime = rangedWeapon.Stats.ReloadTime;
                    rangedWeapon.OnReloadBegin += OnReloadBegin;
                    rangedWeapon.OnReloadEnd += OnReloadEnd;
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
                    rangedWeapon.OnReloadBegin -= OnReloadBegin;
                    rangedWeapon.OnReloadEnd -= OnReloadEnd;
                    break;
            }
        }

        private void OnReloadBegin()
        {
            animator.SetBool(Reloading, true);
            StartCoroutine(WaitForReloadBegin());
        }

        private IEnumerator WaitForReloadBegin()
        {
            // Wait for the reload animation to start
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(_gunLayerIndex).IsName("Reloading"));

            var currentAnimState = animator.GetCurrentAnimatorStateInfo(_gunLayerIndex);
            Debug.Log($"Reloading animation started: {currentAnimState.IsName("Reloading")} {currentAnimState.length}");
            animator.SetFloat(ReloadSpeed, currentAnimState.length / _gunReloadTime);
        }

        private void OnReloadEnd()
        {
            animator.SetBool(Reloading, false);
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