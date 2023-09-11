using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TestInspect : InspectInteractable
    {
        private void OnEnable()
        {
            //OnInspectExit += Pickup;
            _input.InspectExit += ExitInspect;
        }

        private void OnDisable()
        {
            //OnInspectExit -= Pickup;
            _input.InspectExit -= ExitInspect;
        }

        protected override void Update()
        {
            base.Update();

            //do some logic here
        }

        public override bool Interact()
        {
            Inspect();
            return false;
        }

        public override void OnFinishInteractEvent()
        {
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(new InteractionIconEventArgs(false, InteractionType.Default));
        }

        public override void OnInteractEvent()
        {
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(
                new InteractionIconEventArgs(true, InteractionType.Inspect));
        }

        public override object CaptureState()
        {
            return null;
        }

        public override void RestoreState(object state)
        {

        }
    }
}
