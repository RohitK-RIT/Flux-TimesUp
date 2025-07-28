using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    [CreateAssetMenu(menuName = "Audio/Ability Audio Config")]
    public class AbilityAudioConfig : ScriptableObject
    {
        [Header("Volume")]
        [SerializeField, Range(0f, 1f)]
        private float volume = 1f;
        
        [Header("Clips")]
        [SerializeField] private AudioClip abilityClip;

        // Overall volume multiplier for this audio config.
        public float Volume => volume;
        
        // Clip played when reloading the weapon.
        public AudioClip AbilityClip => abilityClip;
    }
}