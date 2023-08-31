using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class GhostChaseState : IState
{
    private readonly Ghost _ghost;
    private readonly Vector3 _playerPos;
    private readonly Animator _animator;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly AudioSource _audioSource;
    SoundUtils sound = new SoundUtils();
    float _checkCondition, _checkConditionDefault= 0.3f;

    public GhostChaseState(Ghost ghost, Animator animator, NavMeshAgent navMeshAgent, AudioSource audioSource)
    { 
        _ghost= ghost;
        _animator= animator;
        _navMeshAgent = navMeshAgent;
        _audioSource = audioSource;
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
        AudioManager.Instance.PlayBGM("Chase");
        sound.CustomBPlaySound(AudioManager.Instance.GetSfx("FemaleScream"), _audioSource);
        _animator.SetFloat("Speed", 1);
        _navMeshAgent.enabled= true;
        _checkCondition= _checkConditionDefault;
    }

    public void OnExit()
    {
        AudioManager.Instance.PlayBGM("LevelBGM");
        _navMeshAgent.enabled = false;
    }
}
