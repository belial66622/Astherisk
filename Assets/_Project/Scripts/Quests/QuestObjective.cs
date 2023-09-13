using System;
using System.Collections.Generic;

namespace ThePatient
{
    [System.Serializable]
    public class QuestObjective
    {
        public QuestObjectiveType objective;
        public int completedObjective;
        public int objectiveCount;

        public event Action OnObjectiveCompleted;
        public bool IsCompleted()
        {
            return completedObjective == objectiveCount;
        }
        public void ResetCompletedObjective()
        {
            completedObjective = 0;
        }

        public QuestObjectiveType GetObjective() => objective;
        public int GetObjectiveCount() => objectiveCount;
        public int GetCompletedObjective() => completedObjective;

        public void CompleteObjective()
        {
            if (!IsCompleted())
                completedObjective++;

            OnObjectiveCompleted?.Invoke();
        }

        public void ForceCompleteObjective()
        {
            completedObjective = objectiveCount;
        }
    }
}
