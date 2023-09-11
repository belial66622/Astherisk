namespace ThePatient
{
    public class InspectPickupObject : InspectInteractable
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

        public override bool Interact()
        {
            Inspect();
            return false;
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

        public override object CaptureState()
        {
            return null;
        }

        public override void RestoreState(object state)
        {

        }
    }
}
