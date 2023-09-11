using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    [System.Serializable]
    public struct STriggerDo 
    {
        public List<GameObject> Activate;
        public List<GameObject> Deactivate;
    }
}
