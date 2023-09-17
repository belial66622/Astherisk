using UnityEngine;

namespace ThePatient
{
    public class InspectPickupObject : InspectPickup 
    {
        [SerializeField] private string pickupAudio;
        [SerializeField]EEventData _event;
        [SerializeField] DialogOnly _dialog;
        protected override void OnEnable()
        {
            base.OnEnable();
            OnInspectExit += Pickup;
            _input.InspectExit += DestroyInspect;
        }

        protected override void OnDisable()
        {
            OnInspectExit -= Pickup;
            _input.InspectExit -= DestroyInspect;
        }

        public override void Pickup(string audio)
        {
            audio = pickupAudio;
            if(_dialog ==null)
            {
                base.Pickup(audio);
                return;
            }

            transform.GetChild(0).gameObject.SetActive(false);
            TriggerManager.Instance.OnEnter(_event);
            if (_dialog != null)
                _dialog.Interact();
        }

        public void EndDialog()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            base.Pickup(pickupAudio);
        }
    }
}
