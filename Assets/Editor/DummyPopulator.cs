using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace ThePatient
{
    public class DummyPopulator : MonoBehaviour
    {
#if UNITY_EDITOR
        public Transform child;
        public Vector3 positionOffset;    

        public List<Transform> children = new List<Transform>();
        public List<Transform> instanceChildren = new List<Transform>();

        [ContextMenu("Populate")]
        public void Populate()
        {
            if(children.Count > 0) { return;  }
            foreach (Transform t in transform) 
            { 
                children.Add(t);
            }
        }

        [ContextMenu("Update Position")]
        public void UpdatePosition()
        {
            if(instanceChildren.Count == 0) { return; }
            foreach (Transform t in instanceChildren)
            {
                t.localPosition = positionOffset;
            }
        }

        [ContextMenu("Add Child")]
        public void AddTransform() 
        {
            Transform t3 = null;
            instanceChildren.Clear();
            foreach (Transform t in children)
            {
                if(t.childCount == 0)
                {
                    var t2 = PrefabUtility.InstantiatePrefab(child);
                    t3 = (Transform)t2;
                    t3.parent = t;
                    t3.transform.localPosition = Vector3.zero + positionOffset;
                    t3.name = $"point light";
                }

                if(t3 != null) 
                {
                    instanceChildren.Add(t3);
                }
                else
                {
                    instanceChildren.Add(t.GetChild(0));
                }
            }

        }

        [ContextMenu("Delete Child")]
        public void DeleteChild()
        {
            foreach (Transform t in instanceChildren)
            {
                t.AddComponent<DummyPopulator>().DestroyGO();
            }
        }

        private void DestroyGO()
        {
            DestroyImmediate(gameObject);
        }
#endif
    }
}
