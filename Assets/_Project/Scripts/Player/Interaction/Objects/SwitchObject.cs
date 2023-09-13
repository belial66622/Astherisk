using System.Collections;
using System.Collections.Generic;
using ThePatient;
using UnityEngine;

namespace ThePatient
{
    public class SwitchObject : BaseInteractable
    {
        [SerializeField] List<GameObject> objectsToToggle = new List<GameObject>();

        List<Renderer> emissions = new List<Renderer>();
        List<Light> lights = new List<Light>();

        bool isOn;
        protected override void Start()
        {
            foreach (var item in objectsToToggle)
            {
                var emissionObj = item.GetComponentsInChildren<Renderer>();
                foreach (var emission in emissionObj)
                {
                    emissions.Add(emission);
                }
                var lightsObj = item.GetComponentsInChildren<Light>();
                foreach (var light in lightsObj)
                {
                    lights.Add(light);
                }
            }

            isOn = lights[0].enabled;
        }

        public override bool Interact()
        {
            if (isOn)
            {
                foreach (var emission in emissions)
                {
                    emission.material.DisableKeyword("_EMISSION");
                }
                foreach (var light in lights)
                {
                    light.enabled = false;
                }
            }
            else
            {
                foreach (var emission in emissions)
                {
                    emission.material.EnableKeyword("_EMISSION");
                }
                foreach (var light in lights)
                {
                    light.enabled = true;
                }
            }
            isOn = !isOn;
            return false;
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