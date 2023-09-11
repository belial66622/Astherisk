using System.Collections;
using UnityEngine;

public class GhostRecognizeState : IState
{
    private readonly float _recognizeTime;
    private readonly Ghost _ghost;
    private readonly Animator _animator;
    private bool _chase, _lost;
    public bool Chase => _chase;
    public bool Lost =>_lost;
    float _timeSee, _timeNoSee;
    public GhostRecognizeState(Ghost ghost, Animator animator, float recognizeTime)
    {
        _ghost = ghost;
        _animator = animator;
        _recognizeTime = recognizeTime;
    }

    public void OnEnter()
    {
        _lost= false;
        _chase = false;
        _timeSee = _recognizeTime;
        _timeNoSee = _recognizeTime;
        _animator.SetFloat("Speed", 0);
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        Debug.Log("recognize");
        if (_ghost.CanSeePlayer == true)
        {
            _timeSee -= Time.deltaTime;
            if (_timeSee <= 0)
            {
                _chase = true;
                _lost = false;
                Debug.Log("see");
            }

        }

        else if (_ghost.CanSeePlayer == false)
        {
            _timeNoSee-= Time.deltaTime;
            if (_timeNoSee <= 0)
            {
                _chase = false;
                _lost = true;
                Debug.Log("lost");
            }
        }
    }


   

}
