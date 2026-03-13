using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.UI
{
    public class RotationAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject objectToRotate1;
        [SerializeField] private GameObject objectToRotate2;
        private const float RotationSpeed = 80f;
        private readonly Vector3 rotationAxis = Vector3.forward;
        private bool isHovered = false;
        
        private void Update()
        {
            if (!isHovered) return;
            RotateObject(objectToRotate1);
            RotateObject(objectToRotate2);
        }
        private void RotateObject(GameObject objectToRotate)
        {
            objectToRotate.transform.Rotate(rotationAxis, RotationSpeed * Time.deltaTime);
        }
        // Detect mouse enter
        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
        }

        // Detect mouse exit
        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
        }
    }
}
