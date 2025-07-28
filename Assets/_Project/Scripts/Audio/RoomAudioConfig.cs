using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Weapons
{
    [CreateAssetMenu(menuName = "Audio/Room Audio Config")]
    public class RoomAudioConfig: ScriptableObject
    {
        [Header("Volume")]
        [SerializeField, Range(0f, 1f)]
        private float volume = 1f;

        [Header("Clips")]
        [SerializeField] private AudioClip teleportClip;
        [SerializeField] private AudioClip roomClip;

        // Overall volume multiplier for this audio config.
        public float Volume => volume;
        
        // Clip played when player teleports to the next room
        public AudioClip TeleportClip => teleportClip;
        
        // Clip played for Rooms.
        public AudioClip RoomClip => roomClip;
        
    }
}