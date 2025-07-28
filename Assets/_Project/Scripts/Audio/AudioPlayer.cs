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

        internal void PlayGrenadeClip(AudioSource audioSource)
        {
            if (abilityAudioConfig.GranadeClip != null)
            {
                audioSource.PlayOneShot(abilityAudioConfig.GranadeClip, abilityAudioConfig.Volume);
            }
        }
        
        internal void PlayHealClip(AudioSource audioSource)
        {
            if (abilityAudioConfig.HealClip != null)
            {
                audioSource.PlayOneShot(abilityAudioConfig.HealClip, abilityAudioConfig.Volume);
            }
        }
        
        internal void PlayShieldClip(AudioSource audioSource)
        {
            if (abilityAudioConfig.ShieldClip != null)
            {
                audioSource.PlayOneShot(abilityAudioConfig.ShieldClip, abilityAudioConfig.Volume);
            }
        }
        
        internal void PlayTeleportClip(AudioSource audioSource)
        {
            if (abilityAudioConfig.TeleportClip != null)
            {
                audioSource.PlayOneShot(abilityAudioConfig.TeleportClip, abilityAudioConfig.Volume);
            }
        }
        
        internal void PlayTSMClip(AudioSource audioSource)
        {
            if (abilityAudioConfig.TSMFreezwClip != null)
            {
                audioSource.PlayOneShot(abilityAudioConfig.TSMFreezwClip, abilityAudioConfig.Volume);
            }
        }
    }
}