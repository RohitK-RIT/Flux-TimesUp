using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class VoiceOverTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject tv;
        private AudioSource voiceoverAudio;
        [SerializeField] private OnboardingSequenceManager onboardingSequenceManager;
        
        private bool hasFinishedOnce = false;
        private bool playerInside = false;
        private Coroutine voiceoverCoroutine;

        private void Start()
        {
            voiceoverAudio = tv.GetComponent<AudioSource>();
            if (voiceoverAudio == null)
            {
                Debug.LogError("AudioSource component not found.");
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInside = true;
                if (voiceoverAudio != null && !voiceoverAudio.isPlaying)
                {
                    voiceoverAudio.Play();
                    if (!hasFinishedOnce && voiceoverCoroutine == null)
                    {
                        voiceoverCoroutine = StartCoroutine(WaitForFirstPlayback());
                    }
                }
                else
                {
                    Debug.LogWarning("Voiceover audio is already playing or not found.");
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInside = false;
                if (voiceoverAudio != null && voiceoverAudio.isPlaying)
                {
                    voiceoverAudio.Stop();
                }
                else
                {
                    Debug.LogWarning("Voiceover audio is not playing or not found.");
                }
            }
        }
        private IEnumerator WaitForFirstPlayback()
        {
            yield return new WaitForSeconds(voiceoverAudio.clip.length);

            if (playerInside && !hasFinishedOnce)
            {
                hasFinishedOnce = true;
                if (onboardingSequenceManager.CurrentStep == OnboardingSequenceManager.OnboardingStep.Room2Lore)
                {
                    onboardingSequenceManager.CheckRoom2Progress();
                }
                if(onboardingSequenceManager.CurrentStep == OnboardingSequenceManager.OnboardingStep.Room6Interaction)
                {
                    onboardingSequenceManager.CheckRoom6Progress();
                }
                voiceoverAudio.loop = true;
                voiceoverAudio.Play();
            }

            voiceoverCoroutine = null;
        }
    }
}
