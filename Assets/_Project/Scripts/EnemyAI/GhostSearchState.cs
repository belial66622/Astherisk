using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GhostSearchState : IState
{
    private readonly float _searchTime;
    private readonly Ghost _ghost;
    private readonly Animator _animator;
    private readonly NavMeshAgent _navMeshAgent;
    float _timeSee, _timeNoSee;
    bool _chase, _patrol;
    public bool Chase => _chase;
    public bool Patrol => _patrol;
    public GhostSearchState(Ghost ghost, Animator animator, float searchTime,NavMeshAgent navMeshAgent)
    {
        _ghost = ghost;
        _animator = animator;
        _searchTime = searchTime;
        _navMeshAgent= navMeshAgent;
    }


    public void OnEnter()
    {
        _timeNoSee = _searchTime;
        _navMeshAgent.enabled = true;
        _patrol= false;
        _chase= false;
        _navMeshAgent.SetDestination(_ghost.LastPosition);

    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetFloat("Speed", 1);
        _animator.SetBool("IsNari", false);
    }

    public void Tick()
    {
        if (destination())
        {
            if (!_animator.GetBool("IsNari"))
            {
                _animator.SetTrigger("Nari");
                _animator.SetBool("IsNari", true);
            }
            Debug.Log("Search");
        if (_ghost.CanSeePlayer == true)
        {

                _chase = true;


        }

        else if (_ghost.CanSeePlayer == false)
        {
            _timeNoSee -= Time.deltaTime;
            if (_timeNoSee <= 0)
            {
                _patrol = true;
            }
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
                    return true;
                }
            }
        }
        return false;
    }
}
