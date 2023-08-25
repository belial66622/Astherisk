using UnityEngine;
using static UnityEditorInternal.ReorderableList;

namespace ThePatient
{
    public class KeyObject : Interactable
    {
        private void OnEnable()
        {
            OnInspectDestroy += Pickup;
            _input.InspectExit += DestroyInspect;
        }

        private void OnDisable()
        {
            OnInspectDestroy -= Pickup;
            _input.InspectExit -= DestroyInspect;
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
