using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform followTarget;
	[SerializeField] private Vector3 offset;
    [SerializeField] private float followSpeed;

    private void FixedUpdate()
    {
        Vector3 targetPosition = followTarget.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Mengatur posisi kamera
        transform.position = smoothPosition;
    }
}
