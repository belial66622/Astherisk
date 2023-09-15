using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ThePatient
{
    public class SinkObject : BaseInteractable
    {
        [Header("Reference")]
        [SerializeField] ParticleSystem sinkWater;

        EmissionModule emission;

        bool isOn = true;
        public override bool Interact()
        {
            emission = sinkWater.emission;
            isOn = !sinkWater.emission.enabled;
            emission.enabled = isOn;

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
                    if (isOn)
                    {
                        isOn = false;
                        emission.enabled = isOn;
                    }

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

        public override object CaptureState()
        {
            return isOn;
        }

        public override void RestoreState(object state)
        {
            isOn = (bool)state;
            emission = sinkWater.emission;
            emission.enabled = isOn;
        }
    }
}