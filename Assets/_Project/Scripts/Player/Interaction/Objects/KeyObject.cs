using UnityEngine;

namespace ThePatient
{
    public class KeyObject : Interactable
    {
        private void Update()
        {
        }

        public override void Interact()
        {
            Pickup();
            gameObject.SetActive(false);
        }

        public override void OnFinishInteractEvent()
        {
            EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(new InteractionTextEventArgs(false, ""));
        }

        public override void OnInteractEvent(string name)
        {
            EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(new InteractionTextEventArgs(true, "Pickup Door Key"));
        }

    }


}
