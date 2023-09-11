using UnityEngine;

namespace ThePatient
{
    public class QuestObjectiveObject : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] QuestObjectiveType objective;

        public QuestObjectiveType GetObjectiveType()
        {
            return objective;
        }

        public Quest GetQuest()
        {
            return quest;
        }
        public void CompleteObjective()
        {
            QuestManager.Instance.CompleteObjective(quest, objective);
        }
    }
}
