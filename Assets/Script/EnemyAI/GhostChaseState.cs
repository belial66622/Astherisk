using UnityEngine;
using UnityEngine.AI;

public class GhostChaseState : IState
{
    private readonly Ghost _ghost;
    private readonly Vector3 _playerPos;
    private readonly Animator _animator;
    private readonly NavMeshAgent _navMeshAgent;
    float _checkCondition, _checkConditionDefault= 0.3f;

    public GhostChaseState(Ghost ghost, Animator animator, NavMeshAgent navMeshAgent)
    { 
        _ghost= ghost;
        _animator= animator;
        _navMeshAgent = navMeshAgent;
    }

    public void Tick()
    {
        Debug.Log("chase");
        _checkCondition -= Time.deltaTime;
        if (_checkCondition <= 0)
        { 
            _navMeshAgent.SetDestination(_ghost.PlayerPosition);
            _checkCondition = _checkConditionDefault;
            Debug.Log(_playerPos);
        }
    }

    public void OnEnter()
    {
        _animator.SetFloat("Speed", 1);
        _navMeshAgent.enabled= true;
        _checkCondition= _checkConditionDefault;
    }

    public void OnExit()
    {
        _animator.SetFloat("Speed", 0);
        _navMeshAgent.enabled = false;
    }
}
