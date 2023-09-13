using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ThePatient
{
    public class TTimeline : Trigger
    {
        [SerializeField] PlayableDirector _play;
        [SerializeField]GameObject _entity;
        [SerializeField] GameObject _enemy;

        private void OnEnable()
        {
            if(_entity != null)
            _entity.SetActive(false);

        }

        public override void DoSomething()
        {
            if (_enemy != null)
            { _enemy.SetActive(true); }
            if (_entity != null && _play != null)
            {
                _entity.SetActive(true);
                _play.Play();
            }
            Debug.Log("mampus");

        }


        public void sent()
        {
            if(_eventName != null)
            TriggerManager.Instance.OnEnter(_eventName);
        }
    }
}
