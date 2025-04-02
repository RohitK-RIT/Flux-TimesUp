using _Project.Scripts.Core.Player_Controllers;

namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface ICollectible
    {
        public void OnCollected(PlayerController playerController);
    }
}