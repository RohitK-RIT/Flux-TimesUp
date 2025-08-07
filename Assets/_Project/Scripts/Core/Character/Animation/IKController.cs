using _Project.Scripts.Core.Character.Hand_Controller;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Scripts.Core.Character.Animation
{
    public class IKController : CharacterComponent
    {
        /// <summary>
        /// Reference to the body IK rig
        /// </summary>
        [SerializeField] private Rig bodyIKRig;

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

        private HandController _handController;

        protected override void Awake()
        {
            base.Awake();

            _rigBuilder = GetComponentInChildren<RigBuilder>();
            _handIKConstraints = gunIKRig.GetComponentsInChildren<TwoBoneIKConstraint>();

            _handController = GetComponent<HandController>();
        }

        private void OnEnable()
        {
            if (_handController)
            {
                _handController.OnItemSwitched += UpdateIKPoints;
            }

            RefreshRig();
        }

        private void OnDisable()
        {
            if (_handController)
            {
                _handController.OnItemSwitched -= UpdateIKPoints;
            }
        }

        /// <summary>
        /// Method to assign and update the IK points as per the current weapon
        /// </summary>
        private void UpdateIKPoints()
        {
            var item = _handController.CurrentItem;
            if (!gunIKRig || !gunAimingIKRig || !meleeIKRig || _handIKConstraints.Length == 0)
            {
                Debug.LogError("");
                return;
            }

            switch (item)
            {
                case MeleeWeapon:
                    bodyIKRig.weight = 0f;
                    gunIKRig.weight = 0f;
                    gunAimingIKRig.weight = 0f;
                    meleeIKRig.weight = 1f;
                    break;
                case RangedWeapon:
                    bodyIKRig.weight = 1f;
                    gunIKRig.weight = 1f;
                    gunAimingIKRig.weight = 1f;
                    meleeIKRig.weight = 0f;

                    item.transform.parent.rotation = Quaternion.identity;

                    // Assign transforms to each Two Bone IK Constraint
                    foreach (var constraint in _handIKConstraints)
                    {
                        // Example: Dynamically fetch transforms based on naming conventions or hierarchy paths
                        var constraintName = constraint.gameObject.name; // Name of the GameObject with the constraint

                        // Fetch source, target, and hint transforms based on the prefab structure
                        var targetObject = item.transform.Find($"IK Points/{constraintName}_target");
                        var hintObject = item.transform.Find($"IK Points/{constraintName}_hint");

                        if (!hintObject || !targetObject)
                        {
                            Debug.LogWarning($"Transforms for constraint {constraintName} could not be found in the prefab!");
                            continue;
                        }

                        // Assign the transforms to the constraint
                        constraint.data.target = targetObject;
                        constraint.data.hint = hintObject;
                    }

                    break;
                default:
                    bodyIKRig.weight = 0f;
                    gunIKRig.weight = 0f;
                    gunAimingIKRig.weight = 0f;
                    meleeIKRig.weight = 0f;
                    break;
            }

            RefreshRig();
        }

        //Method to Refresh the Rig after updating the IK points as per the current weapon
        private void RefreshRig()
        {
            // Refresh the rig to apply changes
            _rigBuilder?.Build();
        }
    }
}