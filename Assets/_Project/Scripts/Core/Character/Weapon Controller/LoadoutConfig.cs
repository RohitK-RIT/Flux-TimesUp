using System;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Weapon_Controller
{
    [Serializable]
    public struct LoadoutConfig
    {
        public string PrimaryWeaponID => primaryWeaponID;
        public string SecondaryWeaponID => secondaryWeaponID;
        public string MeleeWeaponID => meleeWeaponID;
        
        [SerializeField] private string primaryWeaponID, secondaryWeaponID, meleeWeaponID;
    }
}