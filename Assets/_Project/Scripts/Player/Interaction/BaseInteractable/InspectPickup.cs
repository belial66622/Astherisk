using System.ComponentModel;
using UnityEngine;

namespace ThePatient
{
    public class InspectPickup : InspectInteractable, IPickupable
    {
        protected virtual void OnEnable()
        {
            OnInspectExit += Pickup;
            _input.InspectExit += DestroyInspect;
        }

        protected virtual void OnDisable()
        {
            OnInspectExit -= Pickup;
            _input.InspectExit -= DestroyInspect;
        }

        public override bool Interact()
        {
            Inspect();
            return true;
        }

        public override void OnFinishInteractEvent()
        {
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(
                new InteractionIconEventArgs(false, InteractionType.Default));
        }

        public override void OnInteractEvent()
        {
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(
                new InteractionIconEventArgs(true, InteractionType.Pickup));
        }
        public virtual void Pickup([DefaultValue("Key Pickup")] string pickupAudio)
        {
            AudioManager.Instance.PlaySFX(pickupAudio);
            Inventory.Instance.AddItem(this);
            gameObject.SetActive(false);
            Debug.Log("Pickup " + this.ToString());
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
