using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    [System.Serializable]
    public struct STrigger
    {
       public string Name;
       public GameObject Trigger;
       public STriggerDo TriggerDo;        
    }
}
