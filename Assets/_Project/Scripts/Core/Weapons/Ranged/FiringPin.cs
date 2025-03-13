using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    /// <summary>
    /// Interface for fire mode strategies.
    /// </summary>
    public abstract class FiringPin
    {
        /// <summary>
        /// Fire the weapon.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        public abstract IEnumerator Fire(RangedWeaponStats stats, Action fireAction);

        /// <summary>
        /// Get the fire mode strategy based on the fire mode.
        /// </summary>
        /// <param name="mode">fire mode</param>
        /// <returns>strategy based on the fire mode</returns>
        public static FiringPin GetFiringPin(FireModes mode)
        {
            return mode switch
            {
                FireModes.Single => new SingleFirePin(),
                FireModes.Burst => new BurstFirePin(),
                FireModes.Auto => new AutoFirePin(),
                _ => new SingleFirePin()
            };
        }
    }

    /// <summary>
    /// Single fire mode strategy.
    /// </summary>
    public sealed class SingleFirePin : FiringPin
    {
        /// <summary>
        /// Fire the weapon.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        public override IEnumerator Fire(RangedWeaponStats stats, Action fireAction)
        {
            fireAction?.Invoke();
            yield break;
        }
    }

    /// <summary>
    /// Auto fire mode strategy.
    /// </summary>
    public class AutoFirePin : FiringPin
    {
        /// <summary>
        /// Last time the weapon was fired.
        /// </summary>
        private DateTime _lastFireTime = DateTime.MinValue;

        /// <summary>
        /// Fire the weapon.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        public override IEnumerator Fire(RangedWeaponStats stats, Action fireAction)
        {
            // Wait for the attack speed and then fire the bullet.
            yield return new WaitWhile(() => (DateTime.Now - _lastFireTime).Seconds < 1 / stats.AttackSpeed);

            // Keep on firing until the coroutine is stopped.
            while (true)
            {
                yield return InternalFire(stats, fireAction);
                _lastFireTime = DateTime.Now;

                yield return new WaitForSeconds(1 / stats.AttackSpeed);
            }
        }

        /// <summary>
        /// What happens when the weapon is fired.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        protected virtual IEnumerator InternalFire(RangedWeaponStats stats, Action fireAction)
        {
            fireAction?.Invoke();
            yield break;
        }
    }

    /// <summary>
    /// Burst fire mode strategy.
    /// </summary>
    public sealed class BurstFirePin : AutoFirePin
    {
        /// <summary>
        /// What happens when the weapon is fired.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        protected override IEnumerator InternalFire(RangedWeaponStats stats, Action fireAction)
        {
            // Fire the weapon for the amount of bursts.
            for (var burstNumber = 0; burstNumber < stats.BurstAmount; burstNumber++)
            {
                Debug.Log("Firing burst");
                fireAction?.Invoke();
                yield return new WaitForSeconds(stats.BurstDuration / stats.BurstAmount);
            }
        }
    }
}