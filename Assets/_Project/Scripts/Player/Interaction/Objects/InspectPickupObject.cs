using UnityEngine;

namespace ThePatient
{
    public class InspectPickupObject : InspectPickup 
    {
        [SerializeField] private string pickupAudio;
        [SerializeField]EEventData _event;
        protected override void OnEnable()
        {
            base.OnEnable();
            OnInspectExit += Pickup;
            _input.InspectExit += DestroyInspect;
        }

        protected override void OnDisable()
        {
            OnInspectExit -= Pickup;
            _input.InspectExit -= DestroyInspect;
        }

        public override void Pickup(string audio)
        {
            audio = pickupAudio;
                base.Pickup(audio);
            TriggerManager.Instance.OnEnter(_event);
        }

    }
}
