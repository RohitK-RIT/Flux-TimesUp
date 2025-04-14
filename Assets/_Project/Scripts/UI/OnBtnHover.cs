using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace _Project.Scripts.UI
{
    public class OnBtnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool isHovered = false;
        [SerializeField] private Animator animator;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            animator.SetTrigger("Hover");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animator.SetTrigger("Exit"); // Optional, if you want a reverse animation
        }
    }
}
