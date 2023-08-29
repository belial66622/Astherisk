using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class Trigger : MonoBehaviour , ITrigger
    {
        [SerializeField] protected string name;
        public virtual void DoSomething()
        {

            TriggerEvent.instance.Enter(name);
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
