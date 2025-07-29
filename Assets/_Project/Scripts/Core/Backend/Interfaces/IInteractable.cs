using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface IInteractable : IPickable
    {
        public GameObject gameObject { get; }
        public Transform transform { get; }

        /// <summary>
        /// Display name of the interactable item.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Function called when the item is hovered over.
        /// </summary>
        /// <param name="controller">the controller that is trying to pick the item</param>
        public void OnHoverEnter(PlayerController controller);

        /// <summary>
        /// Function called when the item is no longer hovered over.
        /// </summary>
        public void OnHoverExit();
    }
}