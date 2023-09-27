using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TriggerType2Object : TriggerType2
    {
        [SerializeField] GameObject[] obj;
        [SerializeField] bool notdisable;
        public override void OnEnter()
        {
            foreach (GameObject a in obj)
            {
                a.SetActive(true);
            }
        }

        public override void OnExit()
        {
            if(!notdisable)
            { foreach (GameObject a in obj)
                {
                    a.SetActive(false);
                }
                box.enabled = false; 
            }
        }
    }
}
