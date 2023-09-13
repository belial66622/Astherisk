using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class QuestManager : SingletonBehaviour<QuestManager>, ISaveable
    {
        [field: SerializeField] public List<Quest> questList { get; private set; } = new List<Quest>();
        Queue<QuestStatus> questStatusQ = new Queue<QuestStatus>();

        public event Action<QuestStatus> OnQuestsStarted;
        public event Action<QuestStatus> OnQuestsUpdated;

        public List<QuestStatus> completedQuests = new List<QuestStatus>();

        protected override void Awake()
        {
            base.Awake();

            foreach (Quest quest in questList)
            {
                quest.ResetQuest();
            }
        }

        public void AcceptQuest(Quest quest)
        {
            foreach (QuestStatus newStatus in questStatusQ)
            {
                if (newStatus.GetQuest() == quest)
                {
                    return;
                }
            }

            if (quest.IsCompleted()) return;

            QuestStatus status = new QuestStatus(quest);
            questStatusQ.Enqueue(status);
            GetCurrentQuest().ActivateQuest();
            OnQuestsStarted?.Invoke(GetCurrentQuest());
        }

        public void Trigger()
        {
            OnQuestsUpdated?.Invoke(GetCurrentQuest());
        }

        public void CompleteObjective(Quest quest, QuestObjectiveType objective)
        {
            if (GetCurrentQuest().GetQuest() == quest)
            {
                GetCurrentQuest().AddOrUpdateCompletedObjective(objective);
            }

            OnQuestsUpdated?.Invoke(GetCurrentQuest());

            if (quest.IsCompleted())
            {
                completedQuests.Add(GetCurrentQuest());
                GetCurrentQuest().DeactivateQuest();
                questStatusQ.Dequeue();

                if (questStatusQ.Count > 0)
                    GetCurrentQuest().ActivateQuest();
            }

            OnQuestsUpdated?.Invoke(GetCurrentQuest());
        }

        public QuestStatus GetCurrentQuest()
        {
            if (questStatusQ.Count > 0)
                return questStatusQ.Peek();

            return null;
        }

        public Quest GetQuestName(string name)
        {
            foreach (Quest quest in questList)
            {
                if (quest.name == name)
                {
                    return quest;
                }
            }
            return null;
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();

            foreach (QuestStatus quest in completedQuests)
            {
                state.Add(quest.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;

            if (state == null) return;

            foreach (object objectState in stateList)
            {
                completedQuests.Add(new QuestStatus(objectState));
            }

            foreach (QuestStatus questStatus in completedQuests)
            {
                foreach (Quest quest in questList)
                {
                    if (questStatus.GetQuest() == quest)
                    {
                        quest.InitCompleteQuest();
                    }
                }
            }
        }
    }
}
