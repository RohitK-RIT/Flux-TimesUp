using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.Types.Boss
{
    public class PlayerSpeedHandler : MonoBehaviour
    {
        public static PlayerSpeedHandler Instance;
        
        // Stores the player's original movement speed
        private float _originalSpeed;
        
        private PlayerController _player;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            _player = GetComponent<PlayerController>(); 
            _originalSpeed = _player.Stats.movementSpeed;
        }

        public void ReduceSpeed(float duration) {
            Debug.Log("boss is slowing player movement");
            StartCoroutine(SlowPlayer(duration));
        }

        IEnumerator SlowPlayer(float duration) {
            _player.Stats.movementSpeed = 0.5f;
            yield return new WaitForSeconds(duration);
            _player.Stats.movementSpeed = _originalSpeed;
        }
    }
}
