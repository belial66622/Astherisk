using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThePatient
{
    public class DummyPopulator : MonoBehaviour
    {
        public Transform child;
        public Vector3 positionOffset;    

        public List<Transform> children = new List<Transform>();

        [ContextMenu("Populate")]
        public void Populate()
        {
            if(children.Count > 0) { return;  }
            foreach (Transform t in transform) 
            { 
                children.Add(t);
            }
        }

        [ContextMenu("Add Child")]
        public void AddTransform() 
        {
            foreach (Transform t in children)
            {
                var t2 = PrefabUtility.InstantiatePrefab(child);
                Transform t3 = (Transform)t2;
                t3.parent = t;
                t3.transform.localPosition = Vector3.zero + positionOffset;
                t3.name = $"point light";
            }
        }

        [ContextMenu("Delete Child")]
        public void DeleteChild()
        {
            foreach (Transform t in children)
            {
                t.DetachChildren();
            }
        }
    }
}
