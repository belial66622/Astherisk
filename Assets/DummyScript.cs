using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class DummyScript : MonoBehaviour
    {
        [ContextMenu("Enable All Child")]
        void EnableChild()
        {
            foreach(Transform t  in transform)
            {
                t.gameObject.SetActive(true);
            }
        }

        [ContextMenu("Disable Cast Shadow")]
        void DisbleChildShadow()
        {
            foreach (Transform t in transform)
            {
                if(t.TryGetComponent<Renderer>(out Renderer renderer))
                {
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }
        }

    }
}
