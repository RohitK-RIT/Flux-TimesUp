using _Project.Scripts.Core.Character.Weapon_Controller;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Scripts.Core.Character.IK_Points
{
    public class IKPoints : MonoBehaviour
    {
        [SerializeField]
        private GameObject rigRoot; // Reference to the root of the rig
        private WeaponController _weaponController;

        void Awake()
        {
            _weaponController = GetComponent<WeaponController>();
        }
        
        void Start()
        {
            UpdateIKPoints();
        }
        
        //Method to assign and update the IK points as per the current weapon
        internal void UpdateIKPoints()
        {
            
            if (!rigRoot || !_weaponController)
            {
                Debug.LogError("Rig root or prefab is not assigned!");
                return;
            }
            
            // Fetch all Two Bone IK Constraints under the rig root
            TwoBoneIKConstraint[] ikConstraints = rigRoot.GetComponentsInChildren<TwoBoneIKConstraint>();
            
            if (ikConstraints.Length == 0)
            {
                Debug.LogError("No Two Bone IK Constraints found under the rig root!");
                return;
            }
            
            // Assign transforms to each Two Bone IK Constraint
            foreach (var ikConstraint in ikConstraints)
            {
                // Example: Dynamically fetch transforms based on naming conventions or hierarchy paths
                string constraintName = ikConstraint.gameObject.name; // Name of the GameObject with the constraint
            
                // Fetch source, target, and hint transforms based on the prefab structure
                Transform targetObject = _weaponController.CurrentWeapon.transform.Find($"IK Points/{constraintName}_target");
                Transform hintObject = _weaponController.CurrentWeapon.transform.Find($"IK Points/{constraintName}_hint");
                
                if (!hintObject || !targetObject)
                {
                    Debug.LogWarning($"Transforms for constraint {constraintName} could not be found in the prefab!");
                    continue;
                }
                
                // Assign the transforms to the constraint
                ikConstraint.data.target = targetObject;
                ikConstraint.data.hint = hintObject;
            }

            RefreshRig();
        }

        //Method to Refresh the Rig after updating the IK points as per the current weapon
        private void RefreshRig()
        {
            // Refresh the rig to apply changes
            RigBuilder rigBuilder = rigRoot.GetComponentInParent<RigBuilder>();
            if (rigBuilder)
            {
                rigBuilder.Build();
            }
            
            Debug.Log("All Two Bone IK Constraints assigned successfully!");
        }
    }
}
