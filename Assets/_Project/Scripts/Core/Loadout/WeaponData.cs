using System;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

[Serializable]
public class WeaponData
{
    // The weapon prefab that will be instantiated in the game
    [SerializeField] internal Weapon weaponPrefab;
    
    // The weapon icon that will be instantiated
    [SerializeField] internal Sprite icon;
    
    // An array of weapon stats 
    [SerializeField] internal WeaponStats weaponStats;
}
