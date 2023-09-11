using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    [System.Serializable]
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<QuestObjective> completedObjectives = new List<QuestObjective>();

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;
            quest = QuestManager.Instance.GetQuestName(state.questName);
            completedObjectives = state.questObjective;
        }

        public void ActivateQuest()
        {
            quest.ActivateQuest();
            Debug.Log("active " + quest.name);
        }

        public void DeactivateQuest()
        {
            quest.DeactivateQuest();
            Debug.Log("deactive " + quest.name);
        }

        public void ResetQuest()
        {
            quest.ResetQuest();
            Debug.Log("reset " + quest.name);
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

        public void AddOrUpdateCompletedObjective(QuestObjectiveType objective)
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

        [System.Serializable]
        class QuestStatusRecord
        {
            public string questName;
            public List<QuestObjective> questObjective = new List<QuestObjective>();
        }

        public object CaptureState()
        {
            QuestStatusRecord record = new QuestStatusRecord();
            record.questName = quest.name;
            foreach(QuestObjective objective in completedObjectives)
            {
                record.questObjective.Add(objective);
            }
            return record;
        }

    }
}
