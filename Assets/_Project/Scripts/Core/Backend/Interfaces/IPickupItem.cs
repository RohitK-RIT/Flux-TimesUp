
namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface IPickupItem
    {
        public void OnItemPickup();
        public void OnItemEnterRange();
        public void OnItemExitRange();
    }
}