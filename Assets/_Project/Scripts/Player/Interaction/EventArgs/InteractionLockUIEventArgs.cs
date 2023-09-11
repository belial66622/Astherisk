namespace ThePatient
{
    public struct InteractionLockUIEventArgs
    {
        public bool isActive;
        public float valueFraction;

        public InteractionLockUIEventArgs(bool isActive, float valueFraction)
        {
            this.isActive = isActive;
            this.valueFraction = valueFraction;
        }
    }


}
