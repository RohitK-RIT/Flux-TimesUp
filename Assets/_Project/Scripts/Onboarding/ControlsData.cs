using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Onboarding
{
    [Serializable]
    public class ControlsData
    {
        [SerializeField] internal Sprite controlsIcon;
        [SerializeField] internal string controlsName;
        [SerializeField] internal string controlsDescription;
    }
}
