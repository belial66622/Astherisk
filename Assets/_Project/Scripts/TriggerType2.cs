using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TriggerType2 : MonoBehaviour , Icheck
    {

        protected BoxCollider box; 

        protected virtual void Start()
        {
            box = GetComponent<BoxCollider>();
        }
        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public virtual void OnStay()
        {

        }

    }
}
