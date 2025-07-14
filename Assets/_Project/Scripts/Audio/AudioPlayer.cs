using _Project.Scripts.Gameplay.Revamp_PCG;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private WeaponAudioConfig weaponAudioConfig;
        [SerializeField] private RoomAudioConfig roomAudioConfig;
        
        internal void PlayAttackClip(AudioSource audioSource, bool isReloading = false)
        {
            if (!isReloading && weaponAudioConfig.FireClip != null)
            {
                audioSource.PlayOneShot(weaponAudioConfig.FireClip, weaponAudioConfig.Volume);
            }
        }

        internal void PlayOutOfAmmoClip(AudioSource audioSource)
        {
            if (weaponAudioConfig.EmptyClip != null)
            {
                audioSource.PlayOneShot(weaponAudioConfig.EmptyClip, weaponAudioConfig.Volume);
            }
        }

        internal void PlayReloadClip(AudioSource audioSource)
        {
            if (weaponAudioConfig.ReloadClip != null)
            {
                audioSource.PlayOneShot(weaponAudioConfig.ReloadClip, weaponAudioConfig.Volume);
            }
        }

        internal void PlayRoomClip(AudioSource audioSource, RoomEra? roomEraType = null, bool isBossRoom = false)
        {
            AudioClip selectedClip = GetClipByRoomEra(roomEraType, isBossRoom);
            if (selectedClip == null) return;

            audioSource.clip = selectedClip;
            audioSource.loop = true;
            audioSource.volume = roomAudioConfig.Volume;
            audioSource.Play();
        }

        private AudioClip GetClipByRoomEra(RoomEra? roomEraType = null, bool isBossRoom = false)
        {
            if (isBossRoom == true && roomAudioConfig.BossRoomClip != null)
            {
                return roomAudioConfig.BossRoomClip;
            }
            
            switch (roomEraType)
            {
                case RoomEra.Medieval when roomAudioConfig.MedievalClip != null:
                    return roomAudioConfig.MedievalClip;
                case RoomEra.WildWest when roomAudioConfig.WildWestClip != null:
                    return roomAudioConfig.WildWestClip;
                case RoomEra.WorldWar when roomAudioConfig.WorldWarClip != null:
                    return roomAudioConfig.WorldWarClip;
                case RoomEra.Futuristic when roomAudioConfig.FuturisticClip != null:
                    return roomAudioConfig.FuturisticClip;
            }

            return null;
        }

        internal void PlayPlayerTeleportClip(AudioSource audioSource)
        {
            if (roomAudioConfig.TeleportClip != null)
            {
                audioSource.PlayOneShot(roomAudioConfig.TeleportClip, roomAudioConfig.Volume);
            }
        }
    }
}