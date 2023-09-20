using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThePatient
{
    public partial class CameraController : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] InputReader _input;
        [SerializeField] Transform player;
        [SerializeField] Transform _orientation;
        [SerializeField] Transform flashlightTransform;
        [SerializeField] CinemachineVirtualCamera _virtualCamera;

        [Header("Camera Look Settings")]
        [SerializeField] float _gamepadMultiplier = 20f;
        [SerializeField, Range(.1f, 10)] float _lookSpeed;

        [Header("Interaction Setting")]
        [SerializeField] float interactRange = 5f;

        [Header("Ray Settings")]
        [SerializeField] float interactRayRadius = .4f;
        [SerializeField] float interactRayRange = 1f;
 
        CinemachinePOV _pov;
        Camera cam;
        [SerializeField] IInteractable interactable;
        float distance;
        private void OnEnable()
        {
            cam = Camera.main;
            _pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _input.Interact += OnInteract;
            new TickUpdateSystem(15).TickUpdate += CameraController_TickUpdate;

            _lookSpeed = ControlSettingManager.Instance.Mouse_Sensivity;
            if (_lookSpeed <= 0f) _lookSpeed = .1f;
        }

        

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _input.Interact -= OnInteract;
        }
        public void CameraLook(Vector2 lookInput, bool isDeviceMouse)
        {
            //Get the device multiplier
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime * _gamepadMultiplier;

            //input based on device currently active
            float mouseX = lookInput.x * deviceMultiplier;
            float mouseY = lookInput.y * deviceMultiplier;

            //Find current look rotation
            Vector3 rot = transform.localRotation.eulerAngles;
            float desiredX = mouseX + rot.y;

            //Adjust the camera speed
            _pov.m_VerticalAxis.m_MaxSpeed = _lookSpeed;
            _pov.m_HorizontalAxis.m_MaxSpeed = _lookSpeed;

            //Perform the rotations
            _pov.m_VerticalAxis.m_InputAxisValue = mouseY;
            _pov.m_HorizontalAxis.m_InputAxisValue = mouseX;
            _orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        }

        private void OnInteract()
        {
            if(interactable != null && _input.IsInteracting)
            {
                interactable.OnHold = true;
            }
            if(interactable != null && !_input.IsInteracting)
            {
                interactable.OnHold = false;
                interactable.OnFinishInteractEvent();
                if (!interactable.Interact())
                {
                    interactable = null;
                }
            }
        }
        private void CameraController_TickUpdate(int tick)
        {
            interactable = HandleRaycast();
            if(interactable != null)
            {
                distance = (interactable.GetTransform().position - player.transform.position).sqrMagnitude;
                if(distance > interactRange * interactRange)
                {
                    interactable.OnFinishInteractEvent();
                    interactable.OnHold = false;
                    interactable = null;
                }
            }
        }

        private void Update()
        {
            CameraFollowPlayer();
        }

        void CameraFollowPlayer()
        {
            transform.position = player.transform.position;
        }

        private IInteractable HandleRaycast()
        {
            if (interactable != null && interactable.IsInspecting) return interactable;

            Transform origin = flashlightTransform.gameObject.activeSelf ? flashlightTransform : cam.transform;

            if(Physics.SphereCast(origin.position, interactRayRadius, origin.forward, out RaycastHit hit, interactRayRange))
            {
                if(hit.transform != null && hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    distance = (hit.point - player.transform.position).sqrMagnitude;
                    if(distance <= interactRange * interactRange)
                    {
                        interactable.OnInteractEvent();
                        return interactable;
                    }
                }
                else
                {
                    if(this.interactable != null)
                    {
                        this.interactable.OnFinishInteractEvent();
                        this.interactable.OnHold = false;
                    }
                }
            }
            else
            {
                if(interactable != null)
                {
                    interactable.OnFinishInteractEvent();
                    interactable.OnHold = false;
                    return null;
                }
            }
            return null;
        }
    }
}
