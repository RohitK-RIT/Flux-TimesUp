using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Core.Enemy.EnemySpawner;
using _Project.Scripts.Core.Weapons.Ranged;
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
        public GameObject[] LootSpawnPoints => lootSpawnPoints;

        [SerializeField] private GameObject entryPoint;
        [SerializeField] private GameObject exitPoint;
        [SerializeField] private GameObject[] lootSpawnPoints;
        [SerializeField] private RoomEra roomEra; 
        [SerializeField] private GameObject portal;
        [SerializeField] private Collider[] combatArenaColliders;
        
        private bool _clearRoomCheck = false;
        
        private RoomWaveController _roomWaveController;
        
        private AudioSource _roomAudioSource;
        private AudioPlayer _audioPlayer;

        private void Awake()
        {
            _roomWaveController = GetComponent<RoomWaveController>();
            _roomAudioSource = GetComponent<AudioSource>();
            _audioPlayer = GetComponent<AudioPlayer>();
        }
        
        private void Start()
        {
            HidePortal();
            _audioPlayer.PlayRoomClip(_roomAudioSource, roomEra);
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
        public void ShowPortal()
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
