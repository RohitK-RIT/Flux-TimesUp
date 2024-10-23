﻿using System;
using _Project.Scripts.Core.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Player_Controllers
{
    public class CameraController : CharacterComponent
    {
        /// <summary>
        /// The direction the player is looking in.
        /// </summary>
        [NonSerialized] public Vector2 LookInput;

        [SerializeField] private Transform cinemachineCameraTarget;

        [SerializeField] private Camera mainCamera;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            RotateCamera();
        }

        /// <summary>
        /// This method is called to rotate the character based on the movement input.
        /// </summary>
        private void RotateCamera()
        {
            // if there is an input and camera position is not fixed


            //Don't multiply mouse input by Time.deltaTime;
            // float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            const float deltaTimeMultiplier = 1.0f;

            _cinemachineTargetYaw += LookInput.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += LookInput.y * deltaTimeMultiplier;

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, -90f, 90f);

            // Cinemachine will follow this target
            cinemachineCameraTarget.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);

            var screenCenter = new Vector3(0.5f, 0.5f, 0f);
            if (mainCamera)
            {
                PlayerController.MovementController.AimTransform.position = Physics.Raycast(mainCamera.ViewportPointToRay(screenCenter), out var hit, 1000f)
                    ? hit.point
                    : mainCamera.transform.position + mainCamera.transform.forward * 50f;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}