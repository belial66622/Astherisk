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
            TriggerManager.instance._activateblocker += EnableCollider; 
        }

        public override void DoSomething()
        {
            if (!TriggerManager.instance.CheckActive(_eventName))
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
