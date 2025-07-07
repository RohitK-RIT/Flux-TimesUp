using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Config", menuName = "Audio Config")]
public class AudioConfig : ScriptableObject
{
    [Range(0f, 1f)]
    private float _volume = 1f;
    private AudioClip _fireClip;
    private AudioClip _emptyClip;
    private AudioClip _reloadClip;

    private void PlayShootingClip(AudioSource audioSource, bool isReloading)
    {
        if (!isReloading && _fireClip != null)
        {
            audioSource.PlayOneShot(_fireClip, _volume);
        }
    }

    private void PlayOutOfAmmoClip(AudioSource audioSource)
    {
        if (_emptyClip != null)
        {
            audioSource.PlayOneShot(_emptyClip, _volume);
        }
    }

    private void PlayReloadClip(AudioSource audioSource, bool isReloading)
    {
        if (isReloading && _reloadClip != null)
        {
            audioSource.PlayOneShot(_reloadClip, _volume);
        }
    }
}
