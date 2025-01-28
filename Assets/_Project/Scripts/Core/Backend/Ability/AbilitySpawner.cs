using System;
using System.Threading.Tasks;
using _Project.Scripts.Core.Weapons.Abilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Backend.Ability
{
    /// <summary>
    /// Spawns random abilities in the room, on the ground.
    /// </summary>
    public static class AbilitySpawner
    {
        /// <summary>
        /// Layer mask for the ground.
        /// </summary>
        private static readonly LayerMask GroundLayer = LayerMask.GetMask("Ground");

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Spawns random abilities in the room.
        /// </summary>
        /// <param name="parent">parent transform to spawn the ability in</param>
        /// <param name="size">size of the area to spawn ability in</param>
        public static async void SpawnRandomAbilities(Transform parent, Vector3 size)
        {
            try
            {
                // Get the room size and position
                var roomPosition = parent.transform.position;
                // Randomly determine the number of abilities to spawn
                var numberOfAbilitiesToSpawn = Random.Range(2, 5);

                // Spawn the abilities
                for (var i = 0; i < numberOfAbilitiesToSpawn;)
                {
                    // Randomly determine the ability type
                    var abilityType = Random.Range(0, 4) switch
                    {
                        0 => AbilityType.Shield,
                        1 => AbilityType.Teleport,
                        2 => AbilityType.Heal,
                        3 => AbilityType.Attack,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    // Calculate a random spawn position within the room
                    var roomSpawnPosition = new Vector3(Random.Range(-size.x / 2, size.x / 2), 1f, Random.Range(-size.z / 2, size.z / 2));
                    var spawnPosition = roomPosition + roomSpawnPosition;

                    if (IsValidSpawnPosition(spawnPosition))
                    {
                        SpawnAbility(abilityType, parent.transform, spawnPosition);
                        i++; // Increment the counter
                    }
                    else
                    {
                        // If the spawn position is not valid, try again next frame
                        await Task.Yield();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Checks if the spawn position is valid.
        /// </summary>
        /// <param name="position">position to check if walkable</param>
        /// <returns>if the given position is over the ground layer</returns>
        private static bool IsValidSpawnPosition(Vector3 position)
        { 
            // Raycast and check if the position is walkable
            return Physics.Raycast(position, Vector3.down, out _, 5f, GroundLayer);
        }

        /// <summary>
        /// Spawns the ability in the room.
        /// </summary>
        /// <param name="type">type of ability to spawn</param>
        /// <param name="parent">parent to spawn the ability in</param>
        /// <param name="position">position at which the ability should spawn</param>
        private static void SpawnAbility(AbilityType type, Transform parent, Vector3 position)
        {
            // Get the ability pickup prefab
            var abilityPrefab = AbilityDataSystem.Instance.GetAbilityPickupPrefab(type);
            // If the prefab is null, return
            if (!abilityPrefab)
                return;

            // Instantiate the ability pickup prefab
            GameObject.Instantiate(abilityPrefab, position, Quaternion.identity, parent);
        }
    }
}