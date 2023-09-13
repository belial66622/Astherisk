using System;
using TMPro;
using UnityEngine;

namespace ThePatient
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;

        QuestStatus status;

        private void Start()
        {
            QuestManager.Instance.OnQuestsUpdated += Instance_OnQuestsUpdated;
            QuestManager.Instance.OnQuestsStarted += Instance_OnQuestsUpdated;
        }

        private void OnDisable()
        {
            QuestManager.Instance.OnQuestsUpdated -= Instance_OnQuestsUpdated;
            QuestManager.Instance.OnQuestsStarted -= Instance_OnQuestsUpdated;
        }

        private void Instance_OnQuestsUpdated(QuestStatus status)
        {
            text.text = "No Task.";
            if (status != null)
            {
                text.text = "";
                foreach (QuestObjective objective in status.GetQuest().GetObjectives())
                {
                    text.text += $"{objective.GetObjective()} \t{objective.GetCompletedObjective()} / {status.GetQuest().GetQuestObjectiveCount(objective)}\n";
                }
            }
        }
    }
}
