using System;
using TMPro;
using UnityEngine;

namespace ThePatient
{
    public class QuestUITest : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;

        QuestStatus status;

        private void Start()
        {
            QuestManager.Instance.OnQuestsUpdated += Instance_OnQuestsUpdated;
        }

        private void Instance_OnQuestsUpdated()
        {
            text.text = "";
            status = QuestManager.Instance.GetCurrentQuest();
            if (status != null)
            {
                foreach (QuestObjective objective in status.GetQuest().GetObjectives())
                {
                    text.text += $"{objective.GetObjective()} \t{objective.GetCompletedObjective()} / {status.GetQuest().GetQuestObjectiveCount(objective)}\n";
                }
            }
        }
    }
}
