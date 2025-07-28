using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Gameplay.Revamp_PCG;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private WeaponAudioConfig weaponAudioConfig;
        [SerializeField] private RoomAudioConfig roomAudioConfig;
        [SerializeField] private AbilityAudioConfig abilityAudioConfig;
        
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

        internal void PlayRoomClip(AudioSource audioSource)
        {
            audioSource.clip = roomAudioConfig.RoomClip;
            audioSource.loop = true;
            audioSource.volume = roomAudioConfig.Volume;
            audioSource.Play();
        }

        internal void PlayPlayerTeleportClip(AudioSource audioSource)
        {
            if (roomAudioConfig.TeleportClip != null)
            {
                audioSource.PlayOneShot(roomAudioConfig.TeleportClip, roomAudioConfig.Volume);
            }
        }

        internal void PlayAbilityClip(AudioSource audioSource)
        {
            if (abilityAudioConfig.AbilityClip != null)
            {
                audioSource.PlayOneShot(abilityAudioConfig.AbilityClip, abilityAudioConfig.Volume);
            }
        }
        
        
    }
}