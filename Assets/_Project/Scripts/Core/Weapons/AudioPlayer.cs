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

        internal void PlayRoomClip(AudioSource audioSource, RoomEra roomEraType)
        {
            switch (roomEraType)
            {
                case RoomEra.Medieval when roomAudioConfig.MedievalClip != null:
                    audioSource.PlayOneShot(roomAudioConfig.MedievalClip, roomAudioConfig.Volume);
                    break;
                case RoomEra.WildWest when roomAudioConfig.WildWestClip != null:
                    audioSource.PlayOneShot(roomAudioConfig.WildWestClip, roomAudioConfig.Volume);
                    break;
                case RoomEra.WorldWar when roomAudioConfig.WorldWarClip != null:
                    audioSource.PlayOneShot(roomAudioConfig.WorldWarClip, roomAudioConfig.Volume);
                    break;
                case RoomEra.Futuristic when roomAudioConfig.FuturisticClip != null:
                    audioSource.PlayOneShot(roomAudioConfig.FuturisticClip, roomAudioConfig.Volume);
                    break;
            }
        }
    }
}