using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] float speed = 7;
    [SerializeField] bool _puzzleMove;
    [SerializeField] float _radius;
    [SerializeField] LayerMask _targetMask, _obstructionMask;
    float _angle = 45;
    [SerializeField] GameObject puzzle;
    [SerializeField] bool puzzleview = true;
    Vector3 _targetPos;
    public event Action Follow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.A))
        {
            move.y = 1f;
            Debug.Log("a");
        }
        if (Input.GetKey(KeyCode.W))
        {
            move.x = 1f;
            Debug.Log("w");
        }
        if (Input.GetKey(KeyCode.S))
        {
            move.x = -1f;
            Debug.Log("s");
        }
        if (Input.GetKey(KeyCode.D))
        {
            move.y = -1f;
            Debug.Log("d");
        }

        move = move.normalized;

        Vector3 movedir = new Vector3(move.x, 0, move.y);
        transform.position += movedir * Time.deltaTime * speed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Follow?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!puzzleview)
            {
                Debug.Log("ini");
                puzzle.SetActive(true);
                puzzleview = !puzzleview;
                return;
            }

            if (puzzleview)
                Debug.Log("itu");
            {
                puzzle.SetActive(false);
                puzzleview = !puzzleview;
                return;
            }
        }
        //this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }



    void Puzzle()
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
                    _puzzleMove = true;
                    _targetPos = target.position;
                }
                else
                    _puzzleMove = false;

            }
            else
                _puzzleMove = false;
        }

        else if (_puzzleMove)
        {
            _puzzleMove = false;
        }
    }
}
