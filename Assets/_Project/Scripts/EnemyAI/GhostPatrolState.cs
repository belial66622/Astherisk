using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostPatrolState : IState
{
    private readonly List<Vector3> _patrolPoint;
    private readonly Ghost _ghost;
    private readonly Animator _animator;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly float _waitTime;
    private int _wayPointPos;
    float _wait,_defaultWait;


    public GhostPatrolState(Ghost ghost, List<Vector3> patrolPoint, Animator animator, NavMeshAgent navMeshAgent, float waitTime)
    {
        _ghost = ghost;
        _patrolPoint = patrolPoint;
        _animator = animator;
        _navMeshAgent = navMeshAgent;
        _waitTime = waitTime;
    }

    public void OnEnter()
    {
        _ghost.CanScream(true);
        _navMeshAgent.speed = 1.5f;
        AudioManager.Instance.PlayBGM("LevelBGM");
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_patrolPoint[_wayPointPos]);
        _defaultWait = _waitTime;
        _wait = _waitTime;
        _animator.SetFloat("Speed", 1);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetFloat("Speed", 0);
    }

    public void Tick()
    {
        Debug.Log("Patrol");
        if (destination())
        {
            _animator.SetFloat("Speed", 0);
            _wait -= Time.deltaTime;
            if (_wait <= 0)
            {
                NextWaypoint();
                _animator.SetFloat("Speed", 1);
            }
        }
    }


    bool destination()
    {
        if (!_navMeshAgent.pathPending)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                {

                    //_navMeshAgent.SetDestination(_wayPointPos[_wayPointIndex]);
                    return true;
                }
            }
        }
        return false;
    }

    void NextWaypoint()
    {
        if (_patrolPoint.Count == _wayPointPos + 1)
        {
            _wayPointPos = 0;
            _navMeshAgent.SetDestination(_patrolPoint[_wayPointPos]);
            _wait = _defaultWait;
            return;
        }
            _wayPointPos++;
        _navMeshAgent.SetDestination(_patrolPoint[_wayPointPos]);
        _wait = _defaultWait;
    }
}
