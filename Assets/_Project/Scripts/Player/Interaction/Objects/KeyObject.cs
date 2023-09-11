using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace ThePatient
{
    public class KeyObject : InspectInteractable
    {
        private void OnEnable()
        {
            OnInspectExit += Pickup;
            _input.InspectExit += DestroyInspect;
        }

        private void OnDisable()
        {
            OnInspectExit -= Pickup;
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
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(new InteractionIconEventArgs(false, InteractionType.Default));
        }

        public override void OnInteractEvent()
        {
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(
                new InteractionIconEventArgs(true, InteractionType.Pickup));
        }

        public override object CaptureState()
        {
            if (Inventory.Instance.HasItem(this))
            {
                return false;
            }
            return true;
        }

        public override void RestoreState(object state)
        {

        }
    }

}
