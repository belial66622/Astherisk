namespace ThePatient
{
    public struct InteractionIconEventArgs
    {
        public bool isActive;
        public InteractionType interactionType;
        public InteractionIconEventArgs(bool isActive, InteractionType interactionType)
        {
            this.isActive = isActive;
            this.interactionType = interactionType;
        }
    }
}
