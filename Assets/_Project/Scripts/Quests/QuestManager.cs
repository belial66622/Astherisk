using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }
        [field: SerializeField]public List<Quest> questList { get; private set; } = new List<Quest>();
        Queue<QuestStatus> questStatusQ = new Queue<QuestStatus>();

        public event Action OnQuestsUpdated;

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            foreach(Quest quest in questList)
            {
                quest.DeactivateQuest();
            }
        }

        public void AcceptQuest(Quest quest)
        {
            foreach (QuestStatus newStatus in questStatusQ)
            {
                if(newStatus.quest == quest)
                {
                    return;
                }
            }
            QuestStatus status = new QuestStatus(quest);
            questStatusQ.Enqueue(status);
            OnQuestsUpdated?.Invoke();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            foreach(QuestStatus status in questStatusQ)
            {
                if(status.quest == quest)
                {
                    status.AddOrUpdateCompletedObjective(objective);
                }
            }

            if(quest.IsCompleted())
            {
                questStatusQ.Dequeue();
            }
            OnQuestsUpdated?.Invoke();
        }

        public QuestStatus GetQuestStatus()
        {
            if(questStatusQ.Count > 0)
                return questStatusQ.Peek();

            return null;
        }

    }
}
