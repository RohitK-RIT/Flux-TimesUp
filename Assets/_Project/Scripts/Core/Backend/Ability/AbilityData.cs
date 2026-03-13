using System;
using UnityEngine;
using _Project.Scripts.Core.Weapons.Abilities;

namespace _Project.Scripts.Core.Backend.Ability
{
    [Serializable]
    public class AbilityData
    {
        /// <summary>
        /// The name of the ability.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// The type of the ability.
        /// </summary>
        public AbilityType Type => type;

        /// <summary>
        /// The description of the ability.
        /// </summary>
        public string Description => description;

        /// <summary>
        /// The icon of the ability.
        /// </summary>
        public Sprite Icon => icon;

        /// <summary>
        /// The ability prefab.
        /// </summary>
        public Weapons.Abilities.Ability AbilityPrefab => abilityPrefab;
        
        /// <summary>
        /// The ability pickup prefab.
        /// </summary>
        public AbilityPickup AbilityPickup => abilityPickup;

        [SerializeField] private string name;
        [SerializeField] private AbilityType type;
        [SerializeField, Multiline] private string description;
        [SerializeField] private Sprite icon;

        [Space] [SerializeField] private Weapons.Abilities.Ability abilityPrefab;
        [SerializeField] private AbilityPickup abilityPickup;
    }
}