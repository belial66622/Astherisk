using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class ShadowBoxPlayerDetector : MonoBehaviour
    {
        [SerializeField] float colliderMargin = .9f;
        Renderer rnd;
        Collider colliderGO;
        private void Start()
        {
            rnd = GetComponent<Renderer>();
            colliderGO = GetComponent<Collider>();
            rnd.enabled = false;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var distance = other.transform.position - colliderGO.bounds.center;
                var extent = colliderGO.bounds.extents;
                var newDistance = new Vector3(distance.x, 0, distance.z);
                var newExtent = new Vector3(extent.x, 0, extent.z);
                if(newDistance.sqrMagnitude < newExtent.sqrMagnitude * colliderMargin)
                {
                    rnd.enabled = true;
                    Debug.Log("Enter " + colliderGO.name);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                rnd.enabled = false;
                Debug.Log("Exit " + colliderGO.name);
            }
        }
    }
}
