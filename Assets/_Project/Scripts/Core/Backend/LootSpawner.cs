using System;
using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Weapons.Abilities;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Backend
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] lootPrefabs;

        /// <summary>
        /// Singleton instance of LootSpawner.
        /// </summary>
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public static LootSpawner Instance { get; private set; }

        /// <summary>
        /// Function to spawn loot on Enemy Death.
        /// </summary>
        /// <param name="lootDropPosition"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void LootDrop(Vector3 lootDropPosition)
        {
            var dropType = Random.Range(0, 2);
            switch (dropType)
            {
                case 0:
                    // Spawn ammo
                    Instantiate(lootPrefabs[0], lootDropPosition, Quaternion.identity);
                    break;
                case 1:
                    // Spawn Random Abilities
                    var abilityType = Random.Range(0, 4) switch
                    {
                        0 => AbilityType.Heal,
                        1 => AbilityType.Shield,
                        2 => AbilityType.Attack,
                        3 => AbilityType.Teleport,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    SpawnRandomAbilities(abilityType, lootDropPosition);
                    break;
                /*case 2:
                    // Spawn Random Weapons
                    var weaponType = Random.Range(0, 10);
                    break;*/
            }
            
        }
        
        /// <summary>
        /// Function to spawn random abilities.
        /// </summary>
        /// <param name="abilityType">The type of ability to spawn.</param>
        /// <param name="lootDropPosition">The position to spawn the ability.</param>
        private void SpawnRandomAbilities(AbilityType abilityType, Vector3 lootDropPosition)
        {
            // Get the ability pickup prefab
            var abilityPrefab = AbilityDataSystem.Instance.GetAbilityPickupPrefab(abilityType);
            // If the prefab is null, return
            if (!abilityPrefab)
                return;

            // Instantiate the ability pickup prefab
            Instantiate(abilityPrefab, lootDropPosition, Quaternion.identity);
        }
    }
}

