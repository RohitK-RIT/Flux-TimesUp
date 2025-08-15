using System.Collections;
using _Project.Scripts.Core.Backend;
using _Project.Scripts.Core.Backend.Weapon;
using _Project.Scripts.Core.Loadout;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
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
        
        private LocalPlayerController _playerController;
        private LocalInputController _inputController;
        
        [SerializeField] private LootSpawner lootSpawner;
        
        [SerializeField] private GameObject rangedWeaponPickup;
        [SerializeField] private GameObject meleeWeaponPickup;
        [SerializeField] private GameObject abilityPickup;
        
        [SerializeField] private GameObject[] doors;
        [SerializeField] private int targetsPerRoom = 3;
        private int destroyedTargets = 0;

        private Weapon weaponDrop1;
        private Weapon weaponDrop2;
        
        private void Awake()
        {
            _inputController = FindObjectOfType<LocalInputController>();
            _playerController = FindObjectOfType<LocalPlayerController>();

            if (!lootSpawner)
            {
                lootSpawner = FindObjectOfType<LootSpawner>();
            }
            
            lootSpawner.SpawnWeapon("Rifle1", rangedWeaponPickup.transform.position, rangedWeaponPickup.transform);
            lootSpawner.SpawnWeapon("Sword5", meleeWeaponPickup.transform.position, meleeWeaponPickup.transform);
            lootSpawner.SpawnAbility(AbilityType.Grenades, abilityPickup.transform.position, abilityPickup.transform);
            
            if(!_playerController) return;
        }
        
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _playerController.HandController.CurrentItem != null);

            _playerController.HandController.DropWeapon();

            weaponDrop1 = FindObjectOfType<RangedWeapon>();
            if (weaponDrop1 != null)
                Destroy(weaponDrop1.gameObject);

            _playerController.HandController.DropWeapon();

            weaponDrop2 = FindObjectOfType<MeleeWeapon>();
            if (weaponDrop2 != null)
                Destroy(weaponDrop2.gameObject);
        }
        
        private void OnEnable()
        {
            if (!_inputController) return;
            _inputController.OnLookInputUpdated += OnLookDetected;
            _inputController.OnMoveInputUpdated += OnMoveDetected;
        }

        private void OnDisable()
        {
            if (!_inputController) return;
            _inputController.OnLookInputUpdated -= OnLookDetected;
            _inputController.OnMoveInputUpdated -= OnMoveDetected;
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
                //Invoke(PlayAnimation(doors[0]), 3f);
                Invoke(nameof(PlayRoom1DoorAnimation), 1f);
                _currentStep = OnboardingStep.Room2Lore;
            }
        }
        
        private void PlayRoom1DoorAnimation()
        {
            PlayAnimation(doors[0]);
        }
        
        public void CheckRoom2Progress()
        {
            PlayAnimation(doors[1]);
            _currentStep = OnboardingStep.Room3Combat;
        }
        public void NotifyDummyDestroyedOnShooting()
        {
            Debug.Log($"Dummy destroyed during step: {_currentStep}");
            destroyedTargets++;

            if (destroyedTargets >= targetsPerRoom)
            {
                switch (_currentStep)
                {
                    case OnboardingStep.Room3Combat:
                    {
                        destroyedTargets = 0;
                        PlayAnimation(doors[2]);
                        _currentStep = OnboardingStep.Room4Combat;
                        break;
                    }
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
