using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class TestInspect : Interactable
    {
        void OnEnable()
        {
            _input.InspectExit += StopInspect;
        }
        private void OnDisable()
        {
            _input.InspectExit -= StopInspect;
        }

        public override void Interact()
        {
            Inspect();
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
