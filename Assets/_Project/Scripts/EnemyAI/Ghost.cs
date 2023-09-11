using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Ghost : MonoBehaviour
{
    private StateMachine _stateMachine;
    [SerializeField]Waypoint patrolPos;
    [SerializeField] List<Vector3> _waypoint => patrolPos.Waypoints;
    Vector3 _playerPosition,_lastPosition;
    public Vector3 LastPosition => _lastPosition;
    public Vector3 PlayerPosition => _playerPosition;
    bool _canSeePlayer;
    [SerializeField] GameObject _player;
    [SerializeField] float _recognizeTime , _searchTime;
    [SerializeField] FieldOfView _playerPrefab;
    private void Awake()
    {

        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();
        var audioSource = GetComponent<AudioSource>();

        _stateMachine = new StateMachine();

        
        var patrol = new GhostPatrolState(this, _waypoint, animator,navMeshAgent,_searchTime);
        var recognize = new GhostRecognizeState(this, animator,_recognizeTime);
        var search = new GhostSearchState(this, animator,_searchTime, navMeshAgent);
        var chase = new GhostChaseState(this,animator,navMeshAgent,audioSource);


        At(patrol, recognize, HasTarget());
        At(recognize, chase, HasTargetRecognize());
        At(recognize, patrol, HasNoTargetRecognize());
        At(chase, search, HasNoTarget());
        At(search, patrol, HasNoTargetSearch());
        At(search, chase, HasTargetSearch());


        _stateMachine.SetState(patrol);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        
        Func<bool> HasTarget() => () => _canSeePlayer;
        Func<bool> HasNoTarget() => () => !_canSeePlayer;
        Func<bool> HasTargetRecognize() => () => _canSeePlayer && recognize.Chase == true;
        Func<bool> HasNoTargetRecognize() => () => !_canSeePlayer && recognize.Lost== true;
        Func<bool> HasNoTargetSearch() => () => !_canSeePlayer&& search.Patrol == true;
        Func<bool> HasTargetSearch() => () => _canSeePlayer&& search.Chase==true;

    }

    void OnEnable()
    {
        gameObject.GetComponent<FieldOfView>().PlayerPos += SeePlayer;
    }


    private void Update() => _stateMachine.Tick();

    public bool CanSeePlayer => _canSeePlayer;

    void SeePlayer(Vector3 player, bool canSeePlayer)
    {
        if (canSeePlayer == true)
        {
            _playerPosition = player;
            _canSeePlayer = canSeePlayer;
            return;
        }

        _canSeePlayer = canSeePlayer;
        _lastPosition = PlayerPosition;
    }
}
