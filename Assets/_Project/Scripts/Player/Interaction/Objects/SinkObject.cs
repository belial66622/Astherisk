using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient;
using UnityEngine;

namespace ThePatient
{
    public class SinkObject : Interactable
    {
        [Header("Reference")]
        [SerializeField] ParticleSystem sinkWater;

        public override bool Interact()
        {
            var emission = sinkWater.emission;
            emission.enabled = !sinkWater.emission.enabled;


            if (gameObject.TryGetComponent<QuestObjectiveObject>
                (out QuestObjectiveObject questObjectiveObject))
            {
                if (questObjectiveObject.GetQuest().IsActive)
                {
                    if(emission.enabled)
                        emission.enabled = false;

                    questObjectiveObject.CompleteObjective();
                    gameObject.GetComponent<Collider>().enabled = false;
                }
            }
            return false;
        }

        public override void OnFinishInteractEvent()
        {
            EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(new InteractionTextEventArgs(false, ""));
        }

        public override void OnInteractEvent(string name)
        {
            EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(
                new InteractionTextEventArgs(true, "[ E ]\nInteract With Sink"));
        }

    }
}