namespace ThePatient
{
    public class InspectOnly : InspectInteractable
    {
        protected virtual void OnEnable()
        {
            _input.InspectExit += ExitInspect;
        }

        protected virtual void OnDisable()
        {
            _input.InspectExit -= ExitInspect;
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
