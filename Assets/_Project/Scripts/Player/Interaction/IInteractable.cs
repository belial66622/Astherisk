namespace ThePatient
{
    public interface IInteractable
    {
        void OnInteractEvent(string name);
        void OnFinishInteractEvent();
        bool Interact();
        bool OnHold { get; set; }
        bool IsInspecting { get; set; }
    }
}
