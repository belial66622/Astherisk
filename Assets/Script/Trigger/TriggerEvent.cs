using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TriggerEvent: MonoBehaviour
    {
        public static TriggerEvent instance;

        public event Action<string> Onenter;

        private void Awake()
        {
            instance= this;
        }

        public void Enter(string id)
        { 
            Onenter?.Invoke(id);
        }    

    }
}
