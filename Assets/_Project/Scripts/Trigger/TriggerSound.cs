using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ThePatient
{
    public class TriggerSound : Trigger, Icheck
    {

        PlayerCheck _checkPlayer;
        float _volume;
        [SerializeField] string _soundName;
        Sound _tempSound;
        [SerializeField] float _maxDistance;
        AudioSource TempSound;
        [Header("Use multiple check for this trigger")]
        [SerializeField]EEventData[] _checkmultiple;

        protected override void OnEnable()
        {
            _checkPlayer.Pos += Distance;
        }

        protected override void OnDisable()
        {
            _checkPlayer.Pos -= Distance;
            if(_soundName!=null) StopLooping(_soundName) ;

        }

        public override void DoSomething()
        { 
        
        }

        protected void Awake()
        {
            _checkPlayer = GetComponent<PlayerCheck>();
        }
        public void Distance(float distance)
        {

            if (TriggerManager.Instance.CheckMultiple(_checkmultiple))
            {
                _volume = -1 * ((Mathf.Clamp(Mathf.Min(distance, _maxDistance) / _maxDistance, 0f, .9f)) - 1f);
                Debug.Log("volume" + _volume);
                LoopingAudioSfxProcess(_soundName);
                AudioManager.Instance.SetVolume(_tempSound, _volume, _maxDistance);
            }
            else
            { 
                StopLooping(_soundName);
            }
        }


        #region interface
        public void OnEnter()
        {
            _checkPlayer.enabled= true;
            _checkPlayer.Enter();

        }

        public void OnExit()
        {
            _checkPlayer.enabled = false;
            _checkPlayer.Exit();
            if(_tempSound != null)
            AudioManager.Instance.StopLoopingSFX(_tempSound);
        }

        public void OnStay()
        {

        }
        #endregion



        private void StopLooping(string _sfxTobeStopped)
        {
            if(AudioManager.Instance != null && _tempSound != null)
            AudioManager.Instance.StopLoopingSFX(_tempSound);
        }

        private void LoopingAudioSfxProcess(string _sfxTobePlayed)
        {
            if (_tempSound == null)
            {
                _tempSound = AudioManager.Instance.PlayLoopingSFX(_sfxTobePlayed);
                return;
            }
            if (_tempSound.Name != _sfxTobePlayed)
            {
                AudioManager.Instance.StopLoopingSFX(_tempSound);
                _tempSound = AudioManager.Instance.PlayLoopingSFX(_sfxTobePlayed);
                return;
            }

            if (!_tempSound.AudioSource.isPlaying && _tempSound.Name == _sfxTobePlayed)
            {
                _tempSound = AudioManager.Instance.PlayLoopingSFX(_sfxTobePlayed);
            }

        }
    }
}
