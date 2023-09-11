using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    [CreateAssetMenu]
    public class ScriptableTrigger : ScriptableObject
    {
        [SerializeField] STrigger[]_trigger;

        public STrigger[] GetTrigger()
        {
            STrigger[] _tempTriggerArray = new STrigger[_trigger.Length];
            System.Array.Copy(_trigger, _tempTriggerArray, _trigger.Length);
            return _tempTriggerArray;
        }
    }
}
