using UnityEngine;

namespace ThePatient
{
    public class KeyObject : InspectInteractable
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

        public override void Pickup()
        {
            base.Pickup();
            AudioManager.Instance.PlaySFX("KeyPickup");
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
