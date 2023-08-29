namespace ThePatient
{
    public struct InteractionInspectEventArgs
    {
        public bool isActive;
        public InteractionInspectEventArgs(bool isActive)
        {
            this.isActive = isActive;
        }
    }
}
