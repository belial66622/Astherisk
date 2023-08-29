using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TriggerManager : MonoBehaviour
    {
        [SerializeField] List<STrigger> _trigger;
        STrigger _tempTrigger;


        private void OnEnable()
        {
            TriggerEvent.instance.Onenter += Triggerinit;    
        }


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        public void SearchTrigger(string eventname)
        { 
            foreach (STrigger trigger in _trigger)
            {
                if(eventname == trigger.Name)
                {
                    _tempTrigger = trigger;
                    break;
                }
            }

            ActiveDeactivate(_tempTrigger);

        }


        void Triggerinit(string Id)
        { 
            SearchTrigger(Id);
        }

        void ActiveDeactivate(STrigger Temp)
        {
            foreach (GameObject obj in Temp.TriggerDo.Activate)
            { 
                obj.SetActive(true);
            }

            foreach (GameObject obj in Temp.TriggerDo.Deactivate)
            {
                obj.SetActive(false);
            }
        }


    }
}
