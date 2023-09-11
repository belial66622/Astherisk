namespace ThePatient
{
    public struct InteractionLockPuzzleEventArgs
    {
        public bool isActive;
        public InteractionLockPuzzleEventArgs(bool isActive)
        {
            this.isActive = isActive;
        }
    }
}
