using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
    public class ControllerAI : MonoBehaviour
    {
    [SerializeField] private FieldOfView _fov;

    void OnEnable()
    {

        _fov.PlayerPos += Chase;
    }

    #region waypoint
    [SerializeField] NavMeshAgent _ghost;
    [SerializeField] Waypoint _wayPoint;
    int _wayPointIndex=0;
    [SerializeField] List<Vector3> _wayPointPos => _wayPoint.Waypoints;
    #endregion



    private void Start()
    {
        //StartCoroutine(setpos());
    }


    void Update() 
    { 
        
    
    }
    #region Chase

    void Chase(Vector3 _playerpos, bool _canSeePlayer)
    {
            if (_canSeePlayer)
            {
                _ghost.isStopped = false;
                _ghost.SetDestination(_playerpos);
                AudioManager.Instance.PlayBGM("Chase");

            }
            else
                _ghost.isStopped = true;
        
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

    
}
