using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioConfig audioConfig;
        
        internal void PlayShootingClip(AudioSource audioSource, bool isReloading = false)
        {
            if (!isReloading && audioConfig.FireClip != null)
            {
                audioSource.PlayOneShot(audioConfig.FireClip, audioConfig.Volume);
            }
        }

        internal void PlayOutOfAmmoClip(AudioSource audioSource)
        {
            if (audioConfig.EmptyClip != null)
            {
                audioSource.PlayOneShot(audioConfig.EmptyClip, audioConfig.Volume);
            }
        }

        internal void PlayReloadClip(AudioSource audioSource)
        {
            if (audioConfig.ReloadClip != null)
            {
                audioSource.PlayOneShot(audioConfig.ReloadClip, audioConfig.Volume);
            }
        }
    }
}