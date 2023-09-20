using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TriggerBlock : Trigger
    {
        [SerializeField] Collider _blocker;

        protected override void Start()
        {
            TriggerManager.Instance._activeSendSignal += EnableCollider; 
        }

        public override void DoSomething()
        {
            if (!TriggerManager.Instance.CheckActive(_eventName))
            {
                _blocker.enabled = false;
            }
        }

        public void EnableCollider(EEventData signal)
        { 
            if(_eventName == signal) 
            {
                _blocker.enabled = true;
            }
        }
    }
}
