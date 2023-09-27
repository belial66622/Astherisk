using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TriggerDialog : Trigger
    {
        // Start is called before the first frame update
        [SerializeField]DialogOnly dialog;
        protected override void OnEnable()
        {
            _needCollision= true;
        }

        protected override void Start()
        {
        
        }

        // Update is called once per frame
        protected override void Update()
        {
        
        }

        public override void DoSomething()
        {
            if (_active)
            {
                base.DoSomething();
                if (_eventName == EEventData.phase1)
                {
                    AudioManager.Instance.PlayBGM("LevelBGM2");

                        }
                dialog.Interact();
            }
        }


        public void AfterDialog()
        { 
            _needCollision = false;
        }
    }
}
