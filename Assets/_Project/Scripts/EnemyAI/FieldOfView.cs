using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.UIElements;

[SelectionBase]
public class FieldOfView : MonoBehaviour
{
    public Action<Vector3, bool> PlayerPos;
    [SerializeField] protected Transform _sightLocation;

    public Transform sightLocation => _sightLocation;
    #region FOV
    [SerializeField] protected float _radius;
    [Range(0, 360)]
    [SerializeField] protected float _angle;
    [SerializeField] protected Vector3 _playerPos;
    [SerializeField] protected GameObject _playerRef;

    [SerializeField] protected LayerMask _targetMask;
    [SerializeField] protected LayerMask _obstructionMask;
    [SerializeField] protected bool _canSeePlayer, _Countdownstat;
    public bool _playersighted = false, _playerlost = false;
    [SerializeField] protected Transform _player;
    float defaultradius;
    #endregion

    #region Reference
    public bool CanSeePlayer => _canSeePlayer;
    public float Radius => _radius;
    public float Angle => _angle;
    public GameObject PlayerRef => _playerRef;
    #endregion


    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }
    protected virtual void Start()
    {
        defaultradius = _angle;
        StartCoroutine(FOVRoutine());
        //StartCoroutine(setpos());
    }

    #region fov
    public IEnumerator FOVRoutine()
    {
        float delay = .2f;

        WaitForSeconds wait = new WaitForSeconds(delay);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    public void fullradius()
    {
        _angle = 360;
    }

    public void PatrolRadius()
    {
        _angle = defaultradius;
    }
protected virtual void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(_sightLocation.position, _radius, _targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - _sightLocation.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < _angle / 2)
            {
                float distanceToTarget = Vector3.Distance(_sightLocation.position, target.position);
                if (!Physics.Raycast(_sightLocation.position, directionToTarget, distanceToTarget, _obstructionMask))
                {
                    _canSeePlayer = true;
                    PlayerPos?.Invoke(target.position, _canSeePlayer);

                }
                else
                {
                    _canSeePlayer = false;
                    PlayerPos?.Invoke(target.position, _canSeePlayer);
                }
            }
            else
            {
                _canSeePlayer = false;
                PlayerPos?.Invoke(target.position, _canSeePlayer);
            }
        }

        else if (_canSeePlayer)
        {
            _canSeePlayer = false;
            PlayerPos?.Invoke(Vector3.zero, _canSeePlayer);

        }

    }

    #endregion

    
}
