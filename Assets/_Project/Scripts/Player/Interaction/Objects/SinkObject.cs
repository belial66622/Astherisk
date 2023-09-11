using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ThePatient
{
    public class SinkObject : Interactable
    {
        [Header("Reference")]
        [SerializeField] ParticleSystem sinkWater;
        EmissionModule emission;

        protected override void Start()
        {
            gameObject.GetComponent<Collider>().enabled = false;
            QuestManager.Instance.OnQuestsStarted += Instance_OnQuestsStarted;
        }

        private void OnDisable()
        {
            QuestManager.Instance.OnQuestsStarted -= Instance_OnQuestsStarted;
        }

        private void Instance_OnQuestsStarted(QuestStatus status)
        {
            EnableInteraction(status);
        }

        public override bool Interact()
        {
            emission = sinkWater.emission;
            emission.enabled = !sinkWater.emission.enabled;

            CompleteObjective(QuestManager.Instance.GetCurrentQuest());
            return false;
        }

        private void CompleteObjective(QuestStatus status)
        {
            if (gameObject.TryGetComponent<QuestObjectiveObject>
                            (out QuestObjectiveObject questObjectiveObject))
            {
                if (status == null) return;

                if (questObjectiveObject.GetQuest() == status.GetQuest()
                && questObjectiveObject.GetQuest().IsActive)
                {
                    if (emission.enabled)
                        emission.enabled = false;

                    questObjectiveObject.CompleteObjective();
                    gameObject.GetComponent<Collider>().enabled = false;
                }
            }
        }

        void EnableInteraction(QuestStatus status)
        {
            if (gameObject.TryGetComponent<QuestObjectiveObject>
                            (out QuestObjectiveObject questObjectiveObject))
            {
                if (status == null) return;

                if(questObjectiveObject.GetQuest() == status.GetQuest()
                && questObjectiveObject.GetQuest().IsActive)
                {
                    gameObject.GetComponent<Collider>().enabled = true;
                }
            }
        }

        public override void OnFinishInteractEvent()
        {
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(new InteractionIconEventArgs(false, InteractionType.Default));
        }

        public override void OnInteractEvent()
        {
            EventAggregate<InteractionIconEventArgs>.Instance.TriggerEvent(
                new InteractionIconEventArgs(true, InteractionType.Interact));
        }

        public override object CaptureState()
        {
            return null;
        }

        public override void RestoreState(object state)
        {

        }
    }
}