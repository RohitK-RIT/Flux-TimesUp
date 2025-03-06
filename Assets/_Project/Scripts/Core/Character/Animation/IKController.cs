using _Project.Scripts.Core.Character.Weapon_Controller;
using _Project.Scripts.Core.Weapons.Melee;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Scripts.Core.Character.Animation
{
    public class IKController : CharacterComponent
    {
        /// <summary>
        /// Reference to the gun hand rig
        /// </summary>
        [SerializeField] private Rig gunIKRig;

        /// <summary>
        /// Reference to the gun aiming rig
        /// </summary>
        [SerializeField] private Rig gunAimingIKRig;

        /// <summary>
        /// Reference to the melee hand rig
        /// </summary>
        [SerializeField] private Rig meleeIKRig;

        /// <summary>
        /// Reference to the RigBuilder component
        /// </summary>
        private RigBuilder _rigBuilder;

        /// <summary>
        /// Array of Two Bone IK Constraints
        /// </summary>
        private TwoBoneIKConstraint[] _handIKConstraints;

        private WeaponController _weaponController;

        private void Awake()
        {
            _rigBuilder = GetComponentInChildren<RigBuilder>();
            _handIKConstraints = gunIKRig.GetComponentsInChildren<TwoBoneIKConstraint>();

            _weaponController = GetComponent<WeaponController>();
        }

        private void OnEnable()
        {
            if (_weaponController)
            {
                _weaponController.OnWeaponSwitched += UpdateIKPoints;
            }
        }

        private void OnDisable()
        {
            if (_weaponController)
            {
                _weaponController.OnWeaponSwitched -= UpdateIKPoints;
            }
        }

        /// <summary>
        /// Method to assign and update the IK points as per the current weapon
        /// </summary>
        private void UpdateIKPoints()
        {
            var weapon = _weaponController.CurrentWeapon;
            if (!gunIKRig || !weapon)
            {
                Debug.LogError("Rig root or prefab is not assigned!");
                return;
            }

            // Fetch all Two Bone IK Constraints under the rig root
            if (_handIKConstraints.Length == 0)
            {
                Debug.LogError("No Two Bone IK Constraints found under the rig root!");
                return;
            }

            if (weapon is MeleeWeapon)
            {
                gunIKRig.weight = 0f;
                gunAimingIKRig.weight = 0f;
                meleeIKRig.weight = 1f;
            }
            else
            {
                gunIKRig.weight = 1f;
                gunAimingIKRig.weight = 1f;
                meleeIKRig.weight = 0f;
                
                // Assign transforms to each Two Bone IK Constraint
                foreach (var constraint in _handIKConstraints)
                {
                    // Example: Dynamically fetch transforms based on naming conventions or hierarchy paths
                    var constraintName = constraint.gameObject.name; // Name of the GameObject with the constraint

                    // Fetch source, target, and hint transforms based on the prefab structure
                    var targetObject = weapon.transform.Find($"IK Points/{constraintName}_target");
                    var hintObject = weapon.transform.Find($"IK Points/{constraintName}_hint");

                    if (!hintObject || !targetObject)
                    {
                        Debug.LogWarning($"Transforms for constraint {constraintName} could not be found in the prefab!");
                        continue;
                    }

                    // Assign the transforms to the constraint
                    constraint.data.target = targetObject;
                    constraint.data.hint = hintObject;
                }
            }

            RefreshRig();
        }

        //Method to Refresh the Rig after updating the IK points as per the current weapon
        private void RefreshRig()
        {
            // Refresh the rig to apply changes
            _rigBuilder?.Build();

            Debug.Log("All Two Bone IK Constraints assigned successfully!");
        }
    }
}