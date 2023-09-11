using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ThePatient
{

    public class PlayerCheck : FieldOfView
    {
        float _distance;
        public Action<float> Pos;
        protected override void OnEnable()
        {

        }

        protected override void OnDisable()
        {

        }


        protected override void Start()
        {
            //StartCoroutine(FOVRoutine());
        }
        
        
        protected override void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(_sightLocation.position, _radius, _targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 directionToTarget = (target.position - _sightLocation.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < _angle / 2)
                {
                    float distanceToTarget = _distance =  Vector3.Distance(_sightLocation.position, target.position);
                    if (!Physics.Raycast(_sightLocation.position, directionToTarget, distanceToTarget, _obstructionMask))
                    {
                        _canSeePlayer = true;
                        Pos?.Invoke(distanceToTarget);
                        return;
                    }

                    _canSeePlayer = false;
                    Pos?.Invoke(distanceToTarget + 5);
                    
                }
                _canSeePlayer = false;
                Pos?.Invoke(_distance + 5);
            }
        }

        

        public void Enter()
        {
            StartCoroutine(FOVRoutine());
        }
        public void Exit()
        {
            StopAllCoroutines();
        }
    }
}
