using System;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    public class AmmoPickup : MonoBehaviour, ICollectible
    {
        public static event Action<PlayerController, int> OnAmmoCollected;

        [SerializeField] private int ammo;

        public void OnCollected(PlayerController playerController)
        {
            OnAmmoCollected?.Invoke(playerController, ammo);
            Destroy(gameObject);
        }
    }
}