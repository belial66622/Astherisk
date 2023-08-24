using System.Collections.Generic;
using UnityEngine;

public class Waypoint:MonoBehaviour
{
    [SerializeField]List<Vector3> _waypoint;

    public List<Vector3> waypoints => _waypoint;

    private void Start()
    {
        foreach (Transform child in transform) 
        { 
            _waypoint.Add(child.transform.position);
        }
    }
}
