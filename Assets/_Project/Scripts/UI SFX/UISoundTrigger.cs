using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.UI_SFX 
{
    public class UISoundTrigger : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler
    {
        [Header("Types of Interaction")]
        [SerializeField] private InteractionSoundType soundType = InteractionSoundType.Unspecified;
        [SerializeField] private InteractionSoundOn playType = InteractionSoundOn.PointerDown;

        [Header("Manager")]
        [SerializeField] private UIInteractionSoundManager soundManager;

        private void Reset()
        {
            soundManager = FindFirstObjectByType<UIInteractionSoundManager>();
        }
        private void Start()
        {
            if (soundManager == null)
                Debug.LogError("UI Interaction Sound Manager has not been set. " +
                               "Either search manually or click Reset while not in play mode.", this);
        }

        public void PlaySound()
        {
            soundManager.PlaySound(soundType, this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (playType == InteractionSoundOn.PointerDown && soundManager != null)
                soundManager.PlaySound(soundType, this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (playType == InteractionSoundOn.PointerUp && soundManager != null)
                soundManager.PlaySound(soundType, this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (playType == InteractionSoundOn.PointerEnter && soundManager != null)
                soundManager.PlaySound(soundType, this);
        }
    }
}

