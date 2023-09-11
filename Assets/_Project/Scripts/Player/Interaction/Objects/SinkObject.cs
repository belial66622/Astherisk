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
        public override bool Interact()
        {
            emission = sinkWater.emission;
            emission.enabled = !sinkWater.emission.enabled;

            CompleteObjective();
            return false;
        }

        private void CompleteObjective()
        {
            if (gameObject.TryGetComponent<QuestObjectiveObject>
                            (out QuestObjectiveObject questObjectiveObject))
            {
                if (questObjectiveObject.GetQuest().IsActive)
                {
                    if (emission.enabled)
                        emission.enabled = false;

                    questObjectiveObject.CompleteObjective();
                    gameObject.GetComponent<Collider>().enabled = false;
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

    }
}