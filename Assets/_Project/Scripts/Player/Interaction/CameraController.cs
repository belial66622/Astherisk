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
        [SerializeField] Interactable interactable;
        float distance;
        private void OnEnable()
        {
            cam = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _input.Interact += OnInteract;
            new TickUpdateSystem(15).TickUpdate += CameraController_TickUpdate;
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
                interactable.OnFinishInteractEvent();
                interactable = null;
            }
        }
        private void CameraController_TickUpdate(int tick)
        {
            interactable = HandleRaycast();
            if(interactable != null)
            {
                distance = (interactable.transform.position - player.transform.position).sqrMagnitude;
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

        private Interactable HandleRaycast()
        {
            if(Physics.SphereCast(cam.transform.position, interactRayRadius, cam.transform.forward, out RaycastHit hit, interactRayRange))
            {
                if(hit.transform != null && hit.transform.TryGetComponent<Interactable>(out Interactable interactable))
                {
                    distance = (hit.point - player.transform.position).sqrMagnitude;
                    if(distance <= interactRange * interactRange)
                    {
                        interactable.OnInteractEvent(interactable.ToString());
                        return interactable;
                    }
                }
                else
                {
                    if(this.interactable != null)
                    {
                        this.interactable.OnFinishInteractEvent();
                        this.interactable.OnHold = false;
                        return null;
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
