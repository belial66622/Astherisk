using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ThePatient
{
    public class Tiggertype1 : Trigger
    {
        [SerializeField]List<GameObject> _lamp;
        List<Animator> animation = new List<Animator>();
        List<ParticleSystem> particleSystems= new List<ParticleSystem>();
        bool isOn;

        // Start is called before the first frame update
        public override void DoSomething()
        {
            foreach (Animator anim in animation)
            {
                anim.SetTrigger("Flicker");
            }
            foreach (ParticleSystem p in particleSystems) 
            {
                p.Play();
            }
            base.DoSomething();
        }


        protected override void Start()
        {
            foreach (GameObject lamp in _lamp)
            {
                animation.Add(lamp.GetComponent<Animator>());
                particleSystems.Add(lamp.GetComponentInChildren<ParticleSystem>());
            }
        }

        protected override void Update()
        {
            
        }

    }
}
