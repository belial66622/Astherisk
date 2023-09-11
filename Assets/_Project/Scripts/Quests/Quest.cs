using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
    public class Quest : ScriptableObject
    {
        [field: SerializeField]public bool IsActive { get; private set; } = false;
        public List<QuestObjective> objectives = new List<QuestObjective>();
        
        public void ActivateQuest()
        {
            IsActive = true;
        }
        public void DeactivateQuest()
        {
            IsActive = false;
        }

        public void ResetQuest()
        {
            DeactivateQuest();
            foreach (QuestObjective objective in objectives)
            {
                objective.ResetCompletedObjective();
            }
        }

        public int GetObjectivesCount()
        {
            return objectives.Count;
        }

        public int GetQuestObjectiveCount(QuestObjective objective)
        {
            if(objectives.Contains(objective))
            {
                return objective.GetObjectiveCount();
            }
            return 0;
        }
        public List<QuestObjective> GetObjectives()
        {
            return objectives;
        }
        public QuestObjective GetQuestObjective(string objective)
        {
            foreach(QuestObjective obj in objectives)
            {
                if(obj.GetObjective() == objective)
                {
                    return obj;
                }
            }
            
            return null;
        }
        public bool IsCompleted()
        {
            foreach (QuestObjective objective in objectives)
            {
                if (!objective.IsCompleted())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
