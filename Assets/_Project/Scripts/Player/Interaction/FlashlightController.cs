using System;
using UnityEngine;

namespace ThePatient
{
    public class FlashlightController : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] InputReader _input;
        [SerializeField] Transform flashlightTransform;
        [SerializeField] float rotateSmooth = 10f;
 
        float mouseX = 0;
        float mouseY = 0;

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
            flashlightTransform.gameObject.SetActive(!flashlightTransform.gameObject.activeSelf);
        }
        private void FlashlightLook(Vector2 lookInput, bool isDeviceMouse)
        {
            float _gamepadMultiplier = 20f;
            //Get the device multiplier
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime * _gamepadMultiplier;

            //input based on device currently active
            float flashlightrotateMultiplier = 1.5f;
            mouseX += lookInput.x * deviceMultiplier * flashlightrotateMultiplier;
            mouseY -= lookInput.y * deviceMultiplier * flashlightrotateMultiplier;

            mouseX = Mathf.Clamp(mouseX, -40, 40);

            mouseY = Mathf.Clamp(mouseY, -20, 20);

            var targetRotation = Quaternion.Euler(Vector3.up * mouseX) * Quaternion.Euler(Vector3.right * mouseY);

            flashlightTransform.localRotation = Quaternion.Lerp(flashlightTransform.localRotation, targetRotation, rotateSmooth * Time.deltaTime);
        }

    }
}
