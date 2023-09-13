using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TriggerManager : SingletonBehaviour<TriggerManager>
    {
        [SerializeField] ScriptableTrigger _savedTrigger;
        [SerializeField] STrigger[] _triggerList;
        STrigger _tempTrigger;
        public Action<EEventData> _activateblocker;


        protected override void Awake()
        {
            base.Awake();

            _triggerList = new STrigger[_savedTrigger.GetTrigger().Length];
            for (int i = 0; i < _triggerList.Length; i++)
            {
                _triggerList[i] = _savedTrigger.GetTrigger()[i].Clone();
            }
            
        }
        private void OnEnable()
        {

        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }


        public void OnEnter(EEventData eventname)
        { 
            foreach (STrigger trigger in _triggerList)
            {
                if(eventname == trigger._name)
                {
                    _tempTrigger = trigger;
                    break;
                }
            }

            if (eventname != EEventData.DoNothing)
            {
                EventSetter();
            }
        }


        void Triggerinit(EEventData Id)
        {
            OnEnter(Id);
        }



        public bool CheckActive(EEventData _eventname)
        {
            STrigger _searchedTrigger = System.Array.Find(_triggerList, trigger => trigger._name == _eventname);

            if (_searchedTrigger !=null)
            {
                return _searchedTrigger._isActive;
            }
            
            return false;
        }


        public void EventSetter()
        {
            ActivateEvent();
            DeactivateEvent();
        }

        public void ActivateEvent()
        { if (_tempTrigger._activate != null)
            { for (int i = 0; i < _tempTrigger._activate.Length; i++)
                {
                    EEventData temp = _tempTrigger._activate[i];

                    foreach (STrigger trigger in _triggerList)
                    {
                        if (temp == trigger._name)
                        {
                            trigger.SetActive(true);
                            _activateblocker?.Invoke(temp);
                        }
                    }
                }
            }
        }

        public void DeactivateEvent()
        {
            if (_tempTrigger._deactivate != null)
            {
                for (int i = 0; i < _tempTrigger._deactivate.Length; i++)
                {
                    EEventData temp = _tempTrigger._deactivate[i];

                    foreach (STrigger trigger in _triggerList)
                    {
                        if (temp == trigger._name)
                        {
                            trigger.SetActive(false);

                        }
                    }
                }
            }
        }

    }
}
