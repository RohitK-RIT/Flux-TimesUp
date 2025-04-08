using System;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    public abstract class FireModeData : ScriptableObject
    {
        public abstract FireModes FireModes { get; }
    }

    [Serializable]
    public sealed class SingleFireModeData : FireModeData
    {
        public override FireModes FireModes => FireModes.Single;
    }

    [Serializable]
    public class AutoFireModeData : FireModeData
    {
        public override FireModes FireModes => FireModes.Auto;

        public float FireRate => fireRate;

        [SerializeField] protected float fireRate;
    }

    [Serializable]
    public sealed class BurstFireModeData : AutoFireModeData
    {
        public override FireModes FireModes => FireModes.Burst;

        [SerializeField] private float burstAmount;
        [SerializeField] private float burstDuration;
    }
}