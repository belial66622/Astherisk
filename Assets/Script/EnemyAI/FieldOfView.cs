using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System;

[SelectionBase]
    public class FieldOfView : MonoBehaviour
    {
    public Action<Vector3,bool> PlayerPos;

    #region FOV
    [SerializeField] float _radius;
    [Range(0, 360)]
    [SerializeField] float _angle;
    [SerializeField] Vector3 _playerPos;
    [SerializeField] GameObject _playerRef;

    [SerializeField] LayerMask _targetMask;
    [SerializeField] LayerMask _obstructionMask;

    [SerializeField] bool _canSeePlayer;

    #endregion

    #region Reference
    public bool CanSeePlayer => _canSeePlayer;
    public float Radius => _radius;
    public float Angle => _angle;
    public GameObject PlayerRef => _playerRef;
    #endregion

    private void Start()
    {

        StartCoroutine(FOVRoutine());
        //StartCoroutine(setpos());

    }

    #region fov
    IEnumerator FOVRoutine()
    {
        float delay = .2f;

        WaitForSeconds wait = new WaitForSeconds(delay);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                {
                    _canSeePlayer = true;
                    PlayerPos?.Invoke(target.position,_canSeePlayer);
                }
                else
                    _canSeePlayer = false;
                

            }
            else
                _canSeePlayer = false;
        }

        else if (_canSeePlayer)
        {
            _canSeePlayer = false;
        }

    }

    #endregion
}
