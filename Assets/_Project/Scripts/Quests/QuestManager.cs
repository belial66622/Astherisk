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
                quest.ResetQuest();
            }
        }

        public void AcceptQuest(Quest quest)
        {
            foreach (QuestStatus newStatus in questStatusQ)
            {
                if (newStatus.quest == quest)
                {
                    return;
                }
            }

            if(quest.IsCompleted()) return;

            QuestStatus status = new QuestStatus(quest);
            questStatusQ.Enqueue(status);
            GetCurrentQuest().ActivateQuest();
            OnQuestsUpdated?.Invoke();
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            if(GetCurrentQuest().quest == quest)
            {
                GetCurrentQuest().AddOrUpdateCompletedObjective(objective);
            }
            OnQuestsUpdated?.Invoke();

            if(quest.IsCompleted())
            {
                GetCurrentQuest().DeactivateQuest();
                questStatusQ.Dequeue();

                if(questStatusQ.Count > 0)
                    GetCurrentQuest().ActivateQuest();
            }

            OnQuestsUpdated?.Invoke();
        }

        public QuestStatus GetCurrentQuest()
        {
            if(questStatusQ.Count > 0)
                return questStatusQ.Peek();

            return null;
        }

    }
}
