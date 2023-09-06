namespace ThePatient
{
    public class InspectObject : InspectInteractable
    {
        private void OnEnable()
        {
            _input.InspectExit += ExitInspect;
        }

        private void OnDisable()
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
            EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(
                new InteractionTextEventArgs(false, ""));
        }

        public override void OnInteractEvent(string name)
        {
            EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(
                new InteractionTextEventArgs(true, $"[ E ]\nInspect {name}"));
        }
    }
}
