using System.Collections;
using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Revamp_PCG
{
    public class RandomRoomGeneration : MonoBehaviour
    {
        [SerializeField] private DungeonRoom[] poolOfRoomPrefabs;
        [SerializeField] private BossEnemyRoom bossRoom;
        public DungeonRoom CurrentRoom => _currentRoom;
        private DungeonRoom _currentRoom;
        private bool _hasInstantiatedBossRoom = false;

        private void Start()
        {
            InitializeRoomGeneration();
        }

        private void Update()
        {
            //check if the room if cleared of enemies and the TSM is 100
            if (_hasInstantiatedBossRoom == false && _currentRoom.CheckIfRoomIsCleared() &&
                TimeStabilityMeter.Instance.TimeStability >= TimeStabilityMeter.Instance.TotalTimeStability)
            {
                TimeStabilityMeter.Instance.PauseTimeStabilityMeter = true;
                _currentRoom.ShowPortal();

                if (_currentRoom.CheckIfPlayerEntersPortal())
                {
                    Destroy(_currentRoom.gameObject);
                    //spawn boss room
                    InstantiateBossRoom(bossRoom);
                    _hasInstantiatedBossRoom = true;
                }
            }

            //if the player enters the portal, generate a new room
            if (!_hasInstantiatedBossRoom && _currentRoom.CheckIfRoomIsCleared() && _currentRoom.CheckIfPlayerEntersPortal())
            {
                InitializeRoomGeneration();
            }
        }

        private void InitializeRoomGeneration()
        {
            //if there is a current room that exists, destroy it
            if (_currentRoom != null)
            {
                Destroy(_currentRoom.gameObject);
            }

            var randomRoomIndex = Random.Range(0, poolOfRoomPrefabs.Length);
            var spawnedRoom = InstantiateRoom(poolOfRoomPrefabs[randomRoomIndex]);
            _currentRoom = spawnedRoom;

            LevelSceneController.Instance.Player.gameObject.SetActive(false);
            //Instantiate Player in the new room at the entry point
            LevelSceneController.Instance.Player.transform.position = _currentRoom.EntryPoint.transform.position;
            LevelSceneController.Instance.Player.gameObject.SetActive(true);

            //TODO: all enemies killed => spawn loot
        }

        private DungeonRoom InstantiateRoom(DungeonRoom roomToSpawn)
        {
            return Instantiate(roomToSpawn, transform.position, Quaternion.identity, transform);
        }

        private void InstantiateBossRoom(BossEnemyRoom bossRoomToSpawn)
        {
            var bossRoomInstance = Instantiate(bossRoomToSpawn, transform.position, Quaternion.identity, transform);

            //Instantiate Player in the boss room at the entry point
            LevelSceneController.Instance.Player.transform.position = bossRoomInstance.EntryPoint.transform.position;
        }
    }
}