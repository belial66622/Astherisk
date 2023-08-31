using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }
        public List<QuestStatus> questStatuses = new List<QuestStatus>();

        public event Action OnQuestsUpdated;

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void AcceptQuest(Quest quest)
        {
            foreach (QuestStatus newStatus in questStatuses)
            {
                if(newStatus.quest == quest)
                {
                    return;
                }
            }
            QuestStatus status = new QuestStatus(quest);
            questStatuses.Add(status);
            OnQuestsUpdated?.Invoke();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            foreach(QuestStatus status in questStatuses)
            {
                if(status.quest == quest)
                {
                    status.AddOrUpdateCompletedObjective(objective);
                }
            }
            OnQuestsUpdated?.Invoke();
        }

        public QuestStatus GetQuestStatus()
        {
            return questStatuses[0];
        }

    }
}
