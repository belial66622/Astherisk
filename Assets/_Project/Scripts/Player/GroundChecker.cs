using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] Transform _groundChecker;
        [SerializeField] float _groundDistance = .1f;
        [SerializeField] LayerMask groundLayers;

        [field: SerializeField] public bool IsGrounded { get; private set; }

        private void Update()
        {
            IsGrounded = Physics.CheckSphere(_groundChecker.position, _groundDistance, groundLayers);
            
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundChecker.position, _groundDistance);
        }

    }
}
