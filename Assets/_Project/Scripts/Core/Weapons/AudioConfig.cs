using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    [CreateAssetMenu(menuName = "Audio/Audio Config")]
    public class AudioConfig : ScriptableObject
    {
        [Header("Volume")]
        [SerializeField, Range(0f, 1f)]
        private float volume = 1f;

        [Header("Clips")]
        [SerializeField] private AudioClip fireClip;
        [SerializeField] private AudioClip emptyClip;
        [SerializeField] private AudioClip reloadClip;

        // Overall volume multiplier for this audio config. </summary>
        public float Volume => volume;

        // Clip played when firing the weapon. </summary>
        public AudioClip FireClip => fireClip;

        // Clip played when trying to fire an empty weapon. </summary>
        public AudioClip EmptyClip => emptyClip;

        // Clip played when reloading the weapon. </summary>
        public AudioClip ReloadClip => reloadClip;
    }
}
