using _Project.Scripts.Core.Backend.Scene_Control;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Enemy.EnemySpawner;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Revamp_PCG
{
    public class BossEnemyRoom : MonoBehaviour
    {
        public GameObject EntryPoint => entryPoint;
        [SerializeField] private GameObject entryPoint;

        [SerializeField] private EnemyController bossEnemy;

        private AudioSource _roomAudioSource;
        private AudioPlayer _audioPlayer;

        private void Awake()
        {
            _roomAudioSource = GetComponent<AudioSource>();
            _audioPlayer = GetComponent<AudioPlayer>();
        }
        private void Start()
        {
            LevelSceneController.Instance.BossEnemy = bossEnemy;
            _audioPlayer.PlayRoomClip(_roomAudioSource, null, true);
        }
        
    }
}
