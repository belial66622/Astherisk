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

        [Header("Interaction Setting")]
        [SerializeField] float interactRange = 5f;

        [Header("Ray Settings")]
        [SerializeField] float interactRayRadius = .4f;
        [SerializeField] float interactRayRange = 1f;
        [SerializeField] LayerMask interactLayer;
 
        Camera cam;
        Interactable interactable;
        private void OnEnable()
        {
            cam = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _input.Interact += OnInteract;
        }
        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _input.Interact -= OnInteract;
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
                interactable.Interact();
            }
        }

        private void Update()
        {
            transform.position = player.transform.position;
            HandleRaycast();
        }

        private void HandleRaycast()
        {
            if(Physics.SphereCast(cam.transform.position, interactRayRadius, cam.transform.forward, out RaycastHit hit, interactRayRange, interactLayer))
            {
                if(hit.transform != null && hit.transform.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    float distance = (hit.point - player.transform.position).magnitude;
                    if(distance <= interactRange)
                    {
                        this.interactable = interactable;
                        interactable.OnInteractEvent(interactable.ToString());
                    }
                    else
                    {
                        interactable.OnFinishInteractEvent();
                        interactable.OnHold = false;
                        this.interactable = null;
                    }
                }
            }else
            {
                if (interactable != null)
                {
                    interactable.OnFinishInteractEvent();
                    interactable.OnHold = false;
                    this.interactable = null;
                }
            }
        }
    }
}
