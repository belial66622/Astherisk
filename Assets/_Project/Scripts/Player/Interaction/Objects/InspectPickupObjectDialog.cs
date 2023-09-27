﻿using UnityEngine;

namespace ThePatient
{
    public class InspectPickupObjectDialog : InspectPickup 
    {
        [SerializeField] private string pickupAudio;
        [SerializeField]EEventData _event;
        [SerializeField] DialogOnly _dialog;
        int i= 0;
        bool _noDialog;
        bool check => TriggerManager.Instance.CheckActive(_event);
        protected override void OnEnable()
        {
            base.OnEnable();
            OnInspectExit += Pickup;
            _input.InspectExit += DestroyInspect;
            if(TriggerManager.Instance != null)
            TriggerManager.Instance._deactivateSendSignal += DialogDeactivate;
        }

        protected override void OnDisable()
        {
            OnInspectExit -= Pickup;
            TriggerManager.Instance._deactivateSendSignal -= DialogDeactivate;
            _input.InspectExit -= DestroyInspect;
        }

        protected override void Start()
        {
            base.Start();

            TriggerManager.Instance._deactivateSendSignal += DialogDeactivate;
        }


        public override void Pickup(string audio)
        {
            i++;
            audio = pickupAudio;
            if(_dialog ==null)
            {
                base.Pickup(audio);
                return;
            }

            if (i == 2)
            {
                if (!_noDialog)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    TriggerManager.Instance.OnEnter(_event);

                    _dialog.Interact();
                }
                else
                {
                    base.Pickup(pickupAudio);
                }
            }
        }

        public void DialogDeactivate(EEventData data) 
        {
            if (data == _event)
            {
                _noDialog = true;
                if (data == EEventData.NyalaLampu) ;
                AudioManager.Instance.PlayBGM("LevelBGMMatiLampu");
            }
        }

        public void EndDialog()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            base.Pickup(pickupAudio);
        }
    }
}
