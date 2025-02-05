using TMPro;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface IPickupItem
    {
        public void OnItemPickup();
        public void OnTriggerEnter(Collider other);
    }
}