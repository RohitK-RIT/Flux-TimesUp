using UnityEngine;

namespace _Project.Scripts.Onboarding
{
    public class DoorTrigger : MonoBehaviour
    {
        private static readonly int OpenLeftDoor = Animator.StringToHash("OpenLeftDoor");
        private static readonly int OpenRightDoor = Animator.StringToHash("OpenRightDoor");
        private Animator leftDoorAnimator;
        private Animator rightDoorAnimator;
        private void Start()
        {
            leftDoorAnimator = transform.Find("LeftDoor").GetComponent<Animator>();
            rightDoorAnimator = transform.Find("RightDoor").GetComponent<Animator>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (leftDoorAnimator != null && rightDoorAnimator != null)
                {
                    leftDoorAnimator.SetTrigger(OpenLeftDoor);
                    rightDoorAnimator.SetTrigger(OpenRightDoor);
                }
                else
                {
                    Debug.LogWarning("Door animators not found on the door object.");
                }
            }
        }
    }
}
