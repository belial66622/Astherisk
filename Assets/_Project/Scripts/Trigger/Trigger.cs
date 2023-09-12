using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class Trigger : MonoBehaviour , ITrigger, INoCollision
    {
        [SerializeField] protected EEventData _eventName;
        protected bool _active => TriggerManager.instance.CheckActive(_eventName);
        [SerializeField] bool _needCollision;
        public bool CheckCollision() => _needCollision;

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }


        public virtual void DoSomething()
        {
            if (_active);
            {
                TriggerManager.instance.OnEnter(_eventName);

            }
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
        
        }

        // Update is called once per frame
        protected virtual void Update()
        {
        
        }

    }
}
