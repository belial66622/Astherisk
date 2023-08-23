namespace ThePatient
{
    public struct InteractionTextEventArgs
    {
        public bool isActive;
        public string message;
        public InteractionTextEventArgs(bool isActive, string light)
        {
            this.isActive = isActive;
            this.message = light;
        }
    }


}
