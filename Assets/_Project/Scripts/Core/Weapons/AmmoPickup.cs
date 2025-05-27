using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    public class AmmoPickup : MonoBehaviour, ICollectible
    {
        public int Ammo => ammo;

        [SerializeField] private int ammo;

        public void OnPickup(PlayerController controller)
        {
            Destroy(gameObject);
        }

        public void OnDrop() { }
    }
}