using UnityEngine;

namespace ThePatient
{
    public class QuestObjectiveObject : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string objective;

        public void CompleteObjective()
        {
            QuestManager.Instance.CompleteObjective(quest, objective);
        }
    }
}
