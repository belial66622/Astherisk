using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace ThePatient
{
    public class AmbienceSound : MonoBehaviour
    {
        [SerializeField] string sound;
        AudioSource audio;
        bool waiting;
        [SerializeField] int _waitingtime;
        SoundUtils utilities;
        [SerializeField] bool _loopingAmbienceNoCooldown; 

        private void Start()
        {
            utilities = new SoundUtils();
            audio = GetComponent<AudioSource>();

        }

        private void Update()
        {
            if (!audio.isPlaying && !waiting)
            {
                StartCoroutine("timer");
                return;
            }

            if (!audio.isPlaying && !waiting && _loopingAmbienceNoCooldown)
            {
                utilities.CustomBPlaySoundLoop(AudioManager.Instance.GetSfx(sound), audio);
                waiting = true;
            }

        }


        IEnumerator timer()
        {
            waiting = true;

            yield return new WaitForSeconds(_waitingtime);
            utilities.CustomBPlaySound(AudioManager.Instance.GetSfx(sound), audio);
            waiting = false;
        }



    }
}
