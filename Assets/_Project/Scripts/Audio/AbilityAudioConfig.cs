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
        [SerializeField] private AudioClip granadeClip;
        [SerializeField] private AudioClip healClip;
        [SerializeField] private AudioClip shieldClip;
        [SerializeField] private AudioClip teleportClip;
        [SerializeField] private AudioClip tsmFreezwClip;

        // Overall volume multiplier for this audio config. </summary>
        public float Volume => volume;

        // Clip played when firing the weapon. </summary>
        public AudioClip GranadeClip => granadeClip;

        // Clip played when trying to fire an empty weapon. </summary>
        public AudioClip HealClip => healClip;

        // Clip played when reloading the weapon. </summary>
        public AudioClip ShieldClip => shieldClip;
        
        // Clip played when reloading the weapon. </summary>
        public AudioClip TeleportClip => teleportClip;
        
        // Clip played when reloading the weapon. </summary>
        public AudioClip TSMFreezwClip => tsmFreezwClip;
    }
}