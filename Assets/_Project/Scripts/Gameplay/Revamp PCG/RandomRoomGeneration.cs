using System.Collections.Generic;
using _Project.Scripts.Core.Backend.Scene_Control;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Revamp_PCG
{
    public class RandomRoomGeneration : MonoBehaviour
    {
        [SerializeField] private DungeonRoom[] poolOfRoomPrefabs;
        [SerializeField] private DungeonRoom bossRoom;
        //private List<DungeonRoom> _spawnedRooms;
        private DungeonRoom _currentRoom;

        private List<GameObject> _enemiesInRoom;
        
        /*private void Awake()
        {
            //_spawnedRooms = new List<DungeonRoom>();
            //InitializeRoomGeneration();
        }*/

        private void Start()
        {
            InitializeRoomGeneration();
            //TSM will resume if player has crossed that entrance collider and will pause if room is cleared
            //check if TSM is 100 => spawn boss room
        }

        private void Update()
        {
            //if the player enters the portal, generate a new room
            if (_currentRoom.CheckIfPlayerEntersPortal())
            {
                InitializeRoomGeneration();
            }
        }

        private void InitializeRoomGeneration()
        {
            //if there is a current room that exists, destroy it
            if(_currentRoom != null)
            {
                Destroy(_currentRoom.gameObject);
            }
            
            var randomRoomIndex = Random.Range(0, poolOfRoomPrefabs.Length);
            var spawnedRoom = InstantiateRoom(poolOfRoomPrefabs[randomRoomIndex]);
            //_spawnedRooms.Add(spawnedRoom);
            _currentRoom = spawnedRoom;
            
            //Instantiate Player 
            LevelSceneController.Instance.InstantiatePlayerAtEntrance(_currentRoom.EntryPoint.transform.position);
            
            //TODO: all enemies killed => spawn loot
        }
        private DungeonRoom InstantiateRoom(DungeonRoom roomToSpawn)
        {
            return Instantiate(roomToSpawn, transform.position, Quaternion.identity, transform);
        }
    }
}
