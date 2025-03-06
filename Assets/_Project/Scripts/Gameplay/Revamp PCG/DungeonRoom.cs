using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Core.Enemy.EnemySpawner;
using _Project.Scripts.Gameplay.PCG;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Revamp_PCG
{
    /// <summary>
    /// Represents the era of the room, which can be used to determine the theme and aesthetics of the room.
    /// </summary>
    public enum RoomEra
    {
        WorldWar,
        Medieval,
        Futuristic,
        WildWest
    }
    public class DungeonRoom : MonoBehaviour
    {
        public GameObject EntryPoint => entryPoint;
        public GameObject ExitPoint => exitPoint;
        public GameObject[] LootSpawnPoints => lootSpawnPoints;
        public RoomEra RoomEra => roomEra;
        public GameObject Portal => portal;
        public Collider[] CombatArenaColliders => combatArenaColliders;
        
        [SerializeField] private GameObject entryPoint;
        [SerializeField] private GameObject exitPoint;
        [SerializeField] private GameObject[] lootSpawnPoints;
        [SerializeField] private RoomEra roomEra; 
        [SerializeField] private GameObject portal;
        [SerializeField] private Collider[] combatArenaColliders;
        
        private bool _clearRoomCheck = false;
        
        private EnemyDeathListener _enemyDeathListener;
        private RoomWaveController _roomWaveController;

        private void Awake()
        {
            _enemyDeathListener = new EnemyDeathListener(this.gameObject);
            _roomWaveController = GetComponent<RoomWaveController>();
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
        
        private void Start()
        {
            HidePortal();
        }

        private void Update()
        {
            foreach (var enemy in _roomWaveController.EnemiesInRoom)
            {
                if(enemy.activeSelf)
                    return;
            }
            _clearRoomCheck = true;
            //pause TSM
            TimeStabilityMeter.Instance.PauseTimeStabilityMeter = true;
            //TODO: send loot spawn points to loot spawner
            //show portal
            ShowPortal();
        }
        
        // Called when all enemies in the room are dead.
        private void OnAllEnemiesDead()
        {
            /*foreach (var enemy in _roomWaveController.EnemiesInRoom)
            {
                if(enemy.activeSelf)
                    return;
            }
            _clearRoomCheck = true;
            //pause TSM
            TimeStabilityMeter.Instance.PauseTimeStabilityMeter = true;
            //TODO: send loot spawn points to loot spawner
            //show portal
            ShowPortal();*/
        }
        
        /// <summary>
        /// Function to check if the player enters the portal.  
        /// </summary>
        /// <returns></returns>
        public bool CheckIfPlayerEntersPortal()
        {
            var portalCollider = exitPoint.GetComponent<Collider>();
            if (portalCollider.bounds.Contains(LevelSceneController.Instance.Player.transform.position))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Hides the portal object initially.
        /// </summary>
        private void HidePortal()
        {
            if (portal != null)
            {
                portal.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Portal object is not assigned.");
            }
        }
        
        /// <summary>
        /// Shows the portal object when the room is cleared.
        /// </summary>
        private void ShowPortal()
        {
            if (portal != null)
            {
                portal.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Portal object is not assigned.");
            }
        }

        /// <summary>
        /// Function to check if the room is cleared of enemies.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfRoomIsCleared()
        {
            return _clearRoomCheck;
        }
    }
}
