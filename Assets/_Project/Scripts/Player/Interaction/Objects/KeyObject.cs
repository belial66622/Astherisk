using UnityEngine;

namespace ThePatient
{
    public class KeyObject : InspectPickup
    {
        [Header("Trigger Parameter")]
        [SerializeField] EEventData tutorial;

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

        public override void Pickup(string pickupAudio)
        {
            base.Pickup("KeyPickup");
            TriggerManager.Instance.OnEnter(tutorial);
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

    }

}
