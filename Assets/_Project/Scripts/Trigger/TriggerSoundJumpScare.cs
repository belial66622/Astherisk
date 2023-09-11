using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ThePatient
{
    public class TriggerSoundJumpScare : Trigger
    {

        [SerializeField]string _Soundname;
        Sound _temp;
        bool _outFromBox;
        // Start is called before the first frame update

        public override void DoSomething()
        {
            if (_active) AudioManager.Instance.PlaySFX(_Soundname);
            base.DoSomething();

        }

       

        protected override void Start()
        {

        }

        protected override void Update()
        {

        }

    }
}
