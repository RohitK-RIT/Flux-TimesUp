using _Project.Scripts.Core.Player_Controllers;

namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface ICollectible : IPickable
    {
        /// <summary>
        /// Function called when the item is collected.
        /// </summary>
        /// <param name="playerController">the controller that is trying to pick the item</param>
        public void OnCollected(PlayerController playerController)
        {
            OnPickup(playerController);
        }
    }
}