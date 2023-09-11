using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ThePatient
{
    public class TriggerAnimorPS : Trigger
    {
        [SerializeField]List<GameObject> _gameObject;
        List<Animator> animation = new List<Animator>();
        List<ParticleSystem> particleSystems= new List<ParticleSystem>();
        string _animationName;
        bool isOn;

        // Start is called before the first frame update
        public override void DoSomething()
        {
            if (animation != null)
            {
                foreach (Animator anim in animation)
                {
                    anim.SetTrigger(_animationName);
                }
            }

            if (particleSystems != null)
            {
                foreach (ParticleSystem p in particleSystems)
                {
                    p.Play();
                }
            }
            base.DoSomething();
        }


        protected override void Start()
        {
            foreach (GameObject obj in _gameObject)
            {
                if(animation != null)
                animation.Add(obj.GetComponent<Animator>());
                if(particleSystems != null)
                particleSystems.Add(obj.GetComponentInChildren<ParticleSystem>());
            }
        }

        protected override void Update()
        {
            
        }

    }
}
