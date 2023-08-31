namespace ThePatient
{
    [System.Serializable]
    public class QuestObjective
    {
        public string objective;
        public int completedObjective;
        public int objectiveCount;
        public bool IsCompleted()
        {
            return completedObjective >= objectiveCount;
        }
        public void ResetCompletedObjective()
        {
            completedObjective = 0;
        }

        public string GetObjective() => objective;
        public int GetObjectiveCount() => objectiveCount;
        public int GetCompletedObjective() => completedObjective;

        public void CompleteObjective()
        {
            completedObjective++;
        }
    }
}
