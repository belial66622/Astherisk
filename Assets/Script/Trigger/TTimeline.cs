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

        private void OnEnable()
        {
            _entity.SetActive(false);
        }

        public override void DoSomething()
        {
            _entity.SetActive(true);
            _play.Play();

        }


        public void sent()
        {
            TriggerEvent.instance.Enter(name);
        }
    }
}
