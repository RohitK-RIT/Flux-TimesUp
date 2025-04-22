using _Project.Scripts.Core.Player_Controllers;

namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface IPickable
    {
        /// <summary>
        /// Function called when the item is picked up.
        /// </summary>
        /// <param name="controller">the controller that is trying to pick the item</param>
        public void OnPickup(PlayerController controller);

        /// <summary>
        /// Function called when the item is dropped.
        /// </summary>
        public void OnDrop();
    }
}