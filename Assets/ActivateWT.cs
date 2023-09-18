using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class ActivateWT : MonoBehaviour
    {
        [SerializeField] EEventData _activate,_deactivate;
        // Start is called before the first frame update
        private void OnEnable()
        {
            TriggerManager.Instance.OnEnter(_deactivate);
        }

        private void OnDisable()
        {
            TriggerManager.Instance.OnEnter(_activate);
        }


    }
}
