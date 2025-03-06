using System;
using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Gameplay.PCG;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;
using UnityEngine.Serialization;

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

        private void Awake()
        {
            _enemyDeathListener = new EnemyDeathListener(this.gameObject);
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
            //check for any collisions with the combat arena colliders
            foreach (var combatArenaCollider in combatArenaColliders)
            {
                TimeStabilityMeter.Instance.PauseTimeStabilityMeter = !combatArenaCollider.bounds.Contains(LevelSceneController.Instance.Player.transform.position);
                Debug.Log("Player is in combat area.");
            }
            
        }
        
        // Called when all enemies in the room are dead.
        private void OnAllEnemiesDead()
        {
            _clearRoomCheck = true;
            
            //pause TSM
            TimeStabilityMeter.Instance.PauseTimeStabilityMeter = true;
            
            //send loot spawn points to loot spawner
            ShowPortal();
        }
        
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

        public bool CheckIfRoomIsCleared()
        {
            return _clearRoomCheck;
        }
    }
}
