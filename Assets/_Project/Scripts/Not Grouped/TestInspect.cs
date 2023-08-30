using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TestInspect : Interactable
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
            EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(new InteractionTextEventArgs(false, ""));
        }

        public override void OnInteractEvent(string name)
        {
            EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(
                new InteractionTextEventArgs(true, $"[ E ]\nInspect {name}"));
        }

    }
}
