using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public interface Icheck 
    {
        void OnEnter();
        void OnExit();
        void OnStay();
    }
}
