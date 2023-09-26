using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ThePatient
{
    public class SoundRandom : MonoBehaviour
    {
        AudioSource _audio;
        ESound _eSound;
        string _sound;
        // Start is called before the first frame update
        void Start()
        {
            AudioManager.Instance.PlayBGM("LevelBGM");
            _audio = GetComponent<AudioSource>();
            StartCoroutine("Soundrandom");
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Soundrandom()
        {
            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(30f,40f));
                if (!_audio.isPlaying)
                {
                    _eSound = (ESound)UnityEngine.Random.Range(0, Enum.GetNames(typeof(ESound)).Length);
                    _sound = _eSound.ToString();
                    Debug.Log(_sound);
                    _audio = AudioManager.Instance.GetSfx(_sound);
                    _audio.Play();
                }

            }
        }
    }

    enum ESound
    {
        HitAnvil,
        HitDark,
        Whisper,
        Noise,
        Breath,
        Bugs

    }


    
}
