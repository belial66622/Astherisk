using System;
using UnityEngine;

namespace ThePatient
{
    public class PlayerObjectController : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] InputReader _input;
        [SerializeField] CameraController cameraController;
        [SerializeField] InventoryItem flashlightObject;
        [SerializeField] InventoryItem walkietalkieObject;
        [SerializeField] Transform flashlightTransform;
        [SerializeField] Transform walkietalkieTransform;

        [Header("Flashlight Settings")]
        [SerializeField] float maxRotX = 40f;
        [SerializeField] float maxRotY = 20f;
        [SerializeField] float flashlightrotateMultiplier = 1.5f;
        [SerializeField] float rotateSmooth = 5f;
 
        float mouseX = 0;
        float mouseY = 0;

        IPickupable flashlight;
        IPickupable walkietalkie;

        private void Start()
        {
            flashlight = Inventory.Instance.GetItemFromID(flashlightObject.GetItemID()).GetObject();
            walkietalkie = Inventory.Instance.GetItemFromID(walkietalkieObject.GetItemID()).GetObject();
        }

        private void OnEnable()
        {
            _input.Flashlight += ToggleFlashlight;
            _input.Look += FlashlightLook;
        }


        private void OnDisable()
        {
            _input.Flashlight -= ToggleFlashlight;
            _input.Look -= FlashlightLook;
        }

        private void ToggleFlashlight()
        {
            if (Inventory.Instance.HasItem(flashlight))
            {
                flashlightTransform.gameObject.SetActive(!flashlightTransform.gameObject.activeSelf);
                ToggleWalkieTalkie();
            }
        }
        private void ToggleWalkieTalkie()
        {
            if (Inventory.Instance.HasItem(walkietalkie))
            {
                walkietalkieTransform.gameObject.SetActive(!walkietalkieTransform.gameObject.activeSelf);
            }
        }
        private void FlashlightLook(Vector2 lookInput, bool isDeviceMouse)
        {
            if (!flashlightTransform.gameObject.activeSelf)
            {
                cameraController.CameraLook(lookInput, isDeviceMouse);
                return;
            }

            float _gamepadMultiplier = 10f;
            //Get the device multiplier
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime * _gamepadMultiplier;

            //input based on device currently active
            mouseX += lookInput.x * deviceMultiplier * flashlightrotateMultiplier;
            mouseY -= lookInput.y * deviceMultiplier * flashlightrotateMultiplier;

            //Clamp mouse input rotation
            mouseX = Mathf.Clamp(mouseX, -maxRotX, maxRotX);
            mouseY = Mathf.Clamp(mouseY, -maxRotY, maxRotY);

            //if check if flashlight rotation more than certain treshold
            if(mouseX >= .9f * maxRotX || mouseX <= .9f * -maxRotX || mouseY >= .9f * maxRotY || mouseY <= .9f * -maxRotY)
            {
                //rotate camera if yes
                cameraController.CameraLook(lookInput, isDeviceMouse);
            }
            else
            {
                //set camera rotation to none if not
                cameraController.CameraLook(Vector2.zero, isDeviceMouse);
            }

            //set rotation to localVariable
            var targetRotation = Quaternion.Euler(Vector3.up * mouseX) * Quaternion.Euler(Vector3.right * mouseY);

            //apply rotation to transform local rotation
            flashlightTransform.localRotation = Quaternion.Lerp(flashlightTransform.localRotation, targetRotation, rotateSmooth * Time.deltaTime);
        }

    }
}
