using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public enum InputType
    {
        Look,
        Move,
        Attack,
        SwitchWeapon,
        LootPickup,
        AbilityEquip
    }
    public class ControlsDataSystem : MonoBehaviour
    { 
        [SerializeField] internal ControlsData[] controlsDatabase;
    }
}
