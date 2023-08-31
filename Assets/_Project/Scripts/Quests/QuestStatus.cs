using System.Collections.Generic;

namespace ThePatient
{
    [System.Serializable]
    public class QuestStatus
    {
        public Quest quest;
        public List<QuestObjective> completedObjectives = new List<QuestObjective>();

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
            quest.Setup();
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public List<QuestObjective> GetCompletedObjectives()
        {
            return completedObjectives;
        }

        public int GetCompletedObjectives(QuestObjective objective)
        {
            if(completedObjectives.Contains(objective))
            {
                return objective.GetCompletedObjective();
            }
            return 0;
        }

        public void AddOrUpdateCompletedObjective(string objective)
        {
            foreach(QuestObjective obj in completedObjectives)
            {
                if(obj == quest.GetQuestObjective(objective))
                {
                    obj.CompleteObjective();
                    return;
                }
            }
            if(quest.GetQuestObjective(objective) != null)
            {
                quest.GetQuestObjective(objective).CompleteObjective();
                completedObjectives.Add(quest.GetQuestObjective(objective));
            }

        }

    }
}
