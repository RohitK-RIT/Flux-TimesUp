using System;
using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Core.Character.Hand_Controller;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Onboarding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Backend
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] lootPrefabs;

        private void OnEnable()
        {
            PlayerController.OnDeath += OnPlayerDeath;
        }
        
        private void OnDisable()
        {
            PlayerController.OnDeath -= OnPlayerDeath;
        }

        /// <summary>
        /// Function to spawn loot on Player Death.
        /// </summary>
        /// <param name="killingplayer">the attacker</param>
        /// <param name="playerkilled">the dead player</param>
        /// <param name="itemKilledBy">item killed by</param>
        private void OnPlayerDeath(PlayerController killingplayer, PlayerController playerkilled, IHandItem itemKilledBy)
        {
            if (killingplayer != LevelSceneController.Instance.Player)
                return;
            
            //Debug.Log("Room: " + playerkilled.transform.parent.name);
            DropLoot(playerkilled.transform.position, playerkilled.transform.parent);
        }

        /// <summary>
        /// Function to spawn loot on Enemy Death.
        /// </summary>
        /// <param name="lootDropPosition"></param>
        /// <param name="currentRoom">Room in which this item will be spawned.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void DropLoot(Vector3 lootDropPosition, Transform currentRoom)
        {
            var onboarding = FindObjectOfType<OnboardingManager>();
            if (onboarding != null)
            {
                onboarding.OnLootDroppedAfterTeleport(); 
            }
            var dropType = Random.Range(0, 2);
            switch (dropType)
            {
                case 0:
                    // Spawn ammo
                    Instantiate(lootPrefabs[0], lootDropPosition, Quaternion.identity, currentRoom);
                    break;
                case 1:
                    // Spawn Random Abilities
                    var abilityType = Random.Range(0, 4) switch
                    {
                        0 => AbilityType.Heal,
                        1 => AbilityType.Shield,
                        2 => AbilityType.Grenades,
                        3 => AbilityType.Teleport,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    SpawnRandomAbilities(abilityType, lootDropPosition, currentRoom);
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
        /// <param name="currentRoom">Room in which this item will be spawned.</param>
        private void SpawnRandomAbilities(AbilityType abilityType, Vector3 lootDropPosition, Transform currentRoom)
        {
            // Get the ability pickup prefab
            var abilityPrefab = AbilityDataSystem.Instance.GetAbilityPickupPrefab(abilityType);
            // If the prefab is null, return
            if (!abilityPrefab)
                return;

            // Instantiate the ability pickup prefab
            Instantiate(abilityPrefab, lootDropPosition, Quaternion.identity, currentRoom);
        }
    }
}

