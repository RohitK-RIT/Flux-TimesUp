using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    [RequireComponent(typeof(PlayerController))]
    public class CharacterComponent : MonoBehaviour
    {
        protected PlayerController PlayerController { get; private set; }

        protected virtual void Awake()
        {
            PlayerController = GetComponent<PlayerController>();
        }
    }
}