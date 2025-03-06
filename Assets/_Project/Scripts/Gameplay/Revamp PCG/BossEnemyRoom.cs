using _Project.Scripts.Core.Backend.Scene_Control;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Revamp_PCG
{
    public class BossEnemyRoom : MonoBehaviour
    {
        public GameObject EntryPoint => entryPoint;
        [SerializeField] private GameObject entryPoint;

        [SerializeField] private GameObject bossEnemy;

        private void Start()
        {
            LevelSceneController.Instance.BossEnemy = bossEnemy;
        }
    }
}
