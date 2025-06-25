using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class OnboardingSequenceManager : MonoBehaviour
    {
        private static readonly int OpenLeftDoor = Animator.StringToHash("OpenLeftDoor");
        private static readonly int OpenRightDoor = Animator.StringToHash("OpenRightDoor");

        public enum OnboardingStep
        {
            None,
            Room1MoveLook,
            Room1Completed,
            Room2Lore,
            Room3Combat,
            Room4Combat,
            Room5Combat,
            Room6Interaction,
            Room6Completed
        }
        public OnboardingStep CurrentStep => _currentStep;
        private OnboardingStep _currentStep = OnboardingStep.Room1MoveLook;
        private bool _hasMoved = false;
        private bool _hasLooked = false;
        private int _shotsFired = 0;
        private bool _hasReloaded = false;
        private bool _hasPickedUp = false;
        
        private LocalPlayerController _playerController;
        private LocalInputController _inputController;
        
        [SerializeField] private GameObject[] doors;
        [SerializeField] private int targetsPerRoom = 3;
        private int destroyedTargets = 0;
        
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
            /*_inputController.OnSwitchWeaponInput += OnWeaponSwitchDetected;
            _inputController.OnLootPickupInput += OnLootPickupDetected;
            _inputController.OnAbilityEquipped += OnAbilityEquipped;*/
        }

        private void OnDisable()
        {
            if (!_inputController) return;
            _inputController.OnLookInputUpdated -= OnLookDetected;
            _inputController.OnMoveInputUpdated -= OnMoveDetected;
            /*_inputController.OnSwitchWeaponInput -= OnWeaponSwitchDetected;
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
            PlayAnimation(doors[1]);
            _currentStep = OnboardingStep.Room3Combat;
        }
        public void NotifyDummyDestroyedOnShooting()
        {
            destroyedTargets++;

            if (destroyedTargets >= targetsPerRoom)
            {
                switch (_currentStep)
                {
                    case OnboardingStep.Room3Combat:
                        destroyedTargets = 0;
                        PlayAnimation(doors[2]);
                        _currentStep = OnboardingStep.Room4Combat;
                        break;
                    case OnboardingStep.Room4Combat:
                        destroyedTargets = 0;
                        _currentStep = OnboardingStep.Room5Combat;
                        PlayAnimation(doors[3]);
                        break;
                    case OnboardingStep.Room5Combat:
                        destroyedTargets = 0;
                        PlayAnimation(doors[4]);
                        _currentStep = OnboardingStep.Room6Interaction;
                        break;
                }
            }
        }
        public void CheckRoom6Progress()
        {
            PlayAnimation(doors[5]);
            _currentStep = OnboardingStep.Room6Completed;
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
