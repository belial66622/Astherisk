using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest quest;

        public void GiveQuest()
        {
            QuestManager.Instance.AcceptQuest(quest);
        }
    }
}
