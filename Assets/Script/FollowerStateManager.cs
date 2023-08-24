using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


    public class FollowerStateManager : MonoBehaviour
    {
    bool follow;
    [SerializeField] bool onArea;
    [SerializeField]Move _move ;

    void OnEnable()
    {
        _move.Follow += setfollowtrue; 
    }

    void OnDisable()
    {
        _move.Follow -= setfollowtrue;
    }

    #region waypoint
    [SerializeField] NavMeshAgent _ghost;
    [SerializeField] Waypoint _wayPoint;
    int _wayPointIndex=0;
    [SerializeField] List<Vector3> _wayPointPos => _wayPoint.waypoints;
    #endregion

    #region state 
    GhostBaseState _currentState;        
    GhostChaseState _chaseState = new GhostChaseState();       
    GhostPatrolState _patrolState = new GhostPatrolState();  
    GhostSearchState _seachState = new GhostSearchState();
    #endregion

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
        StartCoroutine(Chase());
    }

    #region Chase
    IEnumerator Chase()
    {

        WaitForSeconds wait = new WaitForSeconds(.2f);
        while (true)
        {
            //transform.LookAt(_wayPointPos[_wayPointIndex]);
            yield return wait;
            if (CanSeePlayer && follow)
            {
                _ghost.isStopped = false;
                _ghost.SetDestination(_playerRef.transform.position);

            }
            else
            {
                _ghost.isStopped = true;
                follow = false;
            }
        }
    }
    #endregion

    #region patrol
    IEnumerator setpos()
    {

        WaitForSeconds wait = new WaitForSeconds(1f);
        while (true)
        {
            
            //transform.LookAt(_wayPointPos[_wayPointIndex]);
            yield return wait;
                destination();
        }
    }




    bool destination()
    {
        if (!_ghost.pathPending)
        {
            if (_ghost.remainingDistance <= _ghost.stoppingDistance)
            {
                if (!_ghost.hasPath || _ghost.velocity.sqrMagnitude == 0f)
                {
                    if (_wayPointIndex < _wayPointPos.Count - 1)
                    {
                        _wayPointIndex++;
                    }
                    else
                    {
                        _wayPointIndex = 0;
                    }

                    _ghost.SetDestination(_wayPointPos[_wayPointIndex]);
                    return true;
                }
            }
        }
        return false;
    }

    #endregion

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
                    _playerPos = target.position;
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

    void setfollowtrue()
    {
        if(onArea)
        follow =  !follow;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Move move))
        {
            onArea = true;

        }
        else
            onArea= false;

    }

}
