using UnityEngine;

namespace _Project.Scripts.UI
{
    public class LookAtPlayer : MonoBehaviour
    {
        private Camera mainCamera;

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            LookAtCamera();
        }

        private void LookAtCamera()
        {
            //if the player is in the range of this pickup item, rotate the instruction to pick up the ability.
            var lookAtPosition = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);

            transform.LookAt(lookAtPosition);

            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y - 180, 0f);
        }
    }
}