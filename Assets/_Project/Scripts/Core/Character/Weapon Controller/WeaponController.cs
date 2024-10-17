using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Weapon_Controller
{
    public sealed class WeaponController : MonoBehaviour
    {
        [SerializeField] private Weapon currentWeapon;
        // [SerializeField] private LoadoutConfig loadoutConfig;
        public Weapon CurrentWeapon => currentWeapon;

        private int _currentWeaponIndex = 0;

        private void Start()
        {
            // LoadWeapon(loadoutConfig.PrimaryWeaponID);
        }

        private void LoadWeapon(string weaponID)
        {
            // Load the weapon
        }

        public void BeginAttack()
        {
            CurrentWeapon.BeginAttack();
        }

        public void EndAttack()
        {
            CurrentWeapon.EndAttack();
        }
    }
}