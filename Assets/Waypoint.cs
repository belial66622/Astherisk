using System.Collections.Generic;
using UnityEngine;

public class Waypoint:MonoBehaviour
{
    [SerializeField]List<Vector3> _waypoint;

    public List<Vector3> Waypoints => _waypoint;

    private void Awake()
    {
        foreach (Transform child in transform) 
        { 
            _waypoint.Add(child.transform.position);
        }
    }
}
