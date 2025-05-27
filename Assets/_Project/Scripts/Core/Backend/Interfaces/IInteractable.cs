using _Project.Scripts.Core.Player_Controllers;

namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface IInteractable : IPickable
    {
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