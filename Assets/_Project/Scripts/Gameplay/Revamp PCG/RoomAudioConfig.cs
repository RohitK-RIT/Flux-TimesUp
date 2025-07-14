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
        [SerializeField] private AudioClip medievalClip;
        [SerializeField] private AudioClip wildWestClip;
        [SerializeField] private AudioClip worldWarClip;
        [SerializeField] private AudioClip futuristicClip;
        [SerializeField] private AudioClip teleportClip;
        [SerializeField] private AudioClip bossRoomClip;

        // Overall volume multiplier for this audio config.
        public float Volume => volume;

        // Clip played for medieval rooms.
        public AudioClip MedievalClip => medievalClip;

        // Clip played for wild west rooms.
        public AudioClip WildWestClip => wildWestClip;

        // Clip played for world war rooms.
        public AudioClip WorldWarClip => worldWarClip;
        
        // Clip played for Sci-fi rooms.
        public AudioClip FuturisticClip => futuristicClip;
        
        // Clip played when player teleports to the next room
        public AudioClip TeleportClip => teleportClip;
        
        // Clip played for Boss Room.
        public AudioClip BossRoomClip => bossRoomClip;
        
    }
}