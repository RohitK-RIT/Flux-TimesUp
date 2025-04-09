using _Project.Scripts.Core.Character.Hand_Controller;
using _Project.Scripts.Core.Player_Controllers;

namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(DamageInfo damageInfo);

        public struct DamageInfo
        {
            public readonly float Damage;
            public readonly IHandItem Item;
            public readonly PlayerController Attacker;

            public DamageInfo(float damage, IHandItem item)
            {
                Damage = damage;
                Item = item;
                Attacker = item?.CurrentPlayerController;
            }
        }
    }
}