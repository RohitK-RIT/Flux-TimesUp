using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Enemy.EnemySpawner;
using UnityEngine;

namespace _Project.Scripts.Gameplay.PCG
{
    /// <summary>
    /// Defines the types of rooms available in the dungeon.
    /// </summary>
    public enum RoomType
    {
        Start,
        Exploration,
        Boss
    }

    /// <summary>
    /// Represents a room within the dungeon, containing exits and other properties such as Size, RoomType, etc.
    /// </summary>
    public class Room : MonoBehaviour
    {
        public Exit[] Exits => exits;
        [SerializeField] public Vector3 size;
        [SerializeField] public RoomType roomType;
        [SerializeField] private Exit[] exits;

        private EnemyDeathListener _enemyDeathListener;
        private RoomWaveController _roomWaveController;
        

        /// <summary>
        /// Initializes the room by mapping local exit positions to world positions.
        /// </summary>
        private void Awake()
        {
            _roomWaveController = GetComponent<RoomWaveController>();
            foreach (var exit in Exits)
            {
                exit.worldPosition = transform.position + exit.localPosition;
                Debug.Log($"Exit at {exit.localPosition} mapped to world position {exit.worldPosition}");
            }
            
            if (roomType == RoomType.Exploration)
            {
                //_enemyDeathListener = new EnemyDeathListener(gameObject);
            }
        }

        private void OnEnable()
        {
            if(_enemyDeathListener != null)
                _enemyDeathListener.OnAllEnemiesDead += OnAllEnemiesDead;
        }
        
        private void OnDisable()
        {
            if(_enemyDeathListener != null)
                _enemyDeathListener.OnAllEnemiesDead -= OnAllEnemiesDead;
        }
        
        private void OnAllEnemiesDead()
        {
            // Debug.Log("All enemies dead");
            // _roomWaveController.CanSpawnEnemies();
        }
        
        
    }
}