namespace ThePatient
{
    public interface IInteractable
    {
        void OnInteractEvent(string name);
        void OnFinishInteractEvent();
        void Interact();
        bool OnHold { get; set; }
    }
}
