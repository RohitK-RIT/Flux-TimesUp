using _Project.Scripts.Core.Character;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    public class PlayerAimController : CharacterComponent
    {
        [SerializeField] private Transform cinemachineCameraTarget;
        [SerializeField] private float ignoreWallsDist = 4f;

        private readonly Vector3 _raycastOrigin = new(0.5f, 0.5f, 0f);

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            UpdateAimTransform();
        }

        private void UpdateAimTransform()
        {
            var aimPosition = Vector3.zero;
            var ray = _mainCamera.ViewportPointToRay(_raycastOrigin);

            if (Physics.Raycast(ray, out var hit, 1000f, PlayerController.OpponentLayer, QueryTriggerInteraction.Ignore))
            {
                aimPosition = hit.point;
            }
            // If no enemy is found, check for other objects, but ignore very close hits (like doors)
            else if (Physics.Raycast(ray, out hit, 1000f, ~PlayerController.FriendlyLayer, QueryTriggerInteraction.Ignore))
            {
                var hitDistance = Vector3.Distance(transform.position, hit.point);

                // Ignore hits that are too close (e.g., doors, walls in tight spaces)
                if (hitDistance > ignoreWallsDist)
                {
                    aimPosition = hit.point;
                }
                else
                {
                    // If the hit is too close, just aim forward at a default distance
                    aimPosition = _mainCamera.transform.position + _mainCamera.transform.forward * 50f;
                }
            }
            // Default fallback if nothing is hit
            else
            {
                aimPosition = _mainCamera.transform.position + _mainCamera.transform.forward * 50f;
            }

            PlayerController.MovementController.AimTransform.position = aimPosition;
        }
    }
}