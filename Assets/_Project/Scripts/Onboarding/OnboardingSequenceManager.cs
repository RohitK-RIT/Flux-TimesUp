using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class OnboardingSequenceManager : MonoBehaviour
    {
        private static readonly int OpenLeftDoor = Animator.StringToHash("OpenLeftDoor");
        private static readonly int OpenRightDoor = Animator.StringToHash("OpenRightDoor");

        private enum OnboardingStep
        {
            None,
            Room1MoveLook,
            Room1Completed,
            Room2Lore,
            Room3Combat,
            Room3Completed
        }
        private OnboardingStep _currentStep = OnboardingStep.Room1MoveLook;
        private bool _hasMoved = false;
        private bool _hasLooked = false;
        private int _shotsFired = 0;
        private bool _hasReloaded = false;
        private bool _hasPickedUp = false;
        
        private LocalPlayerController _playerController;
        private LocalInputController _inputController;
        
        [SerializeField] private GameObject[] doors;
    
        private void Awake()
        {
            _inputController = FindObjectOfType<LocalInputController>();
            _playerController = FindObjectOfType<LocalPlayerController>();
            if(!_playerController) return;
        }
        private void OnEnable()
        {
            if (!_inputController) return;
            _inputController.OnLookInputUpdated += OnLookDetected;
            _inputController.OnMoveInputUpdated += OnMoveDetected;
            /*_inputController.OnAttackInputBegan += OnAttackDetected;
            _inputController.OnSwitchWeaponInput += OnWeaponSwitchDetected;
            _inputController.OnLootPickupInput += OnLootPickupDetected;
            _inputController.OnAbilityEquipped += OnAbilityEquipped;*/
        }

        private void OnDisable()
        {
            if (!_inputController) return;
            _inputController.OnLookInputUpdated -= OnLookDetected;
            _inputController.OnMoveInputUpdated -= OnMoveDetected;
            /*_inputController.OnAttackInputBegan -= OnAttackDetected;
            _inputController.OnSwitchWeaponInput -= OnWeaponSwitchDetected;
            _inputController.OnLootPickupInput -= OnLootPickupDetected;
            _inputController.OnAbilityEquipped -= OnAbilityEquipped;*/
        }
        private void OnLookDetected(Vector2 input)
        {
            if (_currentStep == OnboardingStep.Room1MoveLook && input.magnitude > 0.1f)
            {
                _hasLooked = true;
                CheckRoom1Progress();
            }
        }
        private void OnMoveDetected(Vector2 input)
        {
            if (_currentStep == OnboardingStep.Room1MoveLook && input.magnitude > 0.1f)
            {
                _hasMoved = true;
                CheckRoom1Progress();
            }
        }
        private void CheckRoom1Progress()
        {
            if (_hasMoved && _hasLooked)
            {
                Debug.Log("Room 1 Complete");
                _currentStep = OnboardingStep.Room1Completed;
                Invoke(PlayAnimation(doors[0]), 3f);
                _currentStep = OnboardingStep.Room2Lore;
            }
        }
        public void CheckRoom2Progress()
        {
            PlayAnimation(doors[2]);
        }
        private string PlayAnimation(GameObject door)
        {
            var leftDoorAnimator = door.transform.Find("LeftDoor").GetComponent<Animator>();
            var rightDoorAnimator = door.transform.Find("RightDoor").GetComponent<Animator>();
            if (leftDoorAnimator != null && rightDoorAnimator != null)
            {
                leftDoorAnimator.SetTrigger(OpenLeftDoor);
                rightDoorAnimator.SetTrigger(OpenRightDoor);
            }
            else
            {
                Debug.LogWarning("Door Animators are not assigned or missing.");
            }

            return null;
        }
    }
}
