using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Onboarding
{
    public class NarrativeManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text narrativeText;
        [SerializeField] private string[] narrativeSequence;
        [SerializeField] private Button continueButton;
        
        [SerializeField] private float typingSpeed = 0.04f;
        
        private int index = 0;
        
        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EndCheck();
        }

        public void OnContinue()
        {
            index++;
            if (index < narrativeSequence.Length)
            {
                EndCheck();
            }
            else
            {
                EndNarrative();
            }
        }
        private void EndCheck()
        {
            if (index < narrativeSequence.Length )
            {
                narrativeText.text = narrativeSequence[index];
                continueButton.gameObject.SetActive(false);
                StartCoroutine(DisplayLine());
            }   
        }
        private void EndNarrative()
        {
            // Now lock the cursor for gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private IEnumerator DisplayLine()
        {
            narrativeText.ForceMeshUpdate();
            var totalVisibleCharacters = narrativeText.textInfo.characterCount;
            var counter = 0;
            while (true)
            {
                var visibleCount = counter % (totalVisibleCharacters + 1);
                narrativeText.maxVisibleCharacters = visibleCount;
                if (visibleCount >= totalVisibleCharacters)
                {
                    break;
                }

                counter++;
                yield return new WaitForSeconds(typingSpeed);
            }
            continueButton.gameObject.SetActive(true);
        }
    }
}
