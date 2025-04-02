using _Project.Scripts.Core.Player_Controllers;

namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface IPickable
    {
        /// <summary>
        /// Function called when the item is picked up.
        /// </summary>
        /// <param name="playerController">the controller that is trying to pick the item</param>
        public void OnPickup(PlayerController playerController);

        /// <summary>
        /// Function called when the item is dropped.
        /// </summary>
        public void OnDrop();

        /// <summary>
        /// Function called when the item is hovered over.
        /// </summary>
        /// <param name="playerController">the controller that is trying to pick the item</param>
        public void OnHoverEnter(PlayerController playerController);

        /// <summary>
        /// Function called when the item is no longer hovered over.
        /// </summary>
        public void OnHoverExit();
    }
}