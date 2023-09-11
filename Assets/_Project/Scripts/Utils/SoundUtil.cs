using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class SoundUtils
    {

        public void PlaySound(AudioSource spatialsound, AudioSource localspatialsound)
        {
            localspatialsound.clip = spatialsound.clip;
            localspatialsound.volume = 1.0f;
            localspatialsound.pitch = 1.0f;
            localspatialsound.loop = spatialsound.loop;
            localspatialsound.playOnAwake = false;
            localspatialsound.outputAudioMixerGroup = spatialsound.outputAudioMixerGroup;
            localspatialsound.spatialBlend = spatialsound.spatialBlend;
            localspatialsound.Play();
        }

        public void CustomBPlaySound(AudioSource spatialsound, AudioSource localspatialsound)
        {
            localspatialsound.clip = spatialsound.clip;
            localspatialsound.volume = 1.0f;
            localspatialsound.pitch = 1.0f;
            localspatialsound.loop = spatialsound.loop;
            localspatialsound.playOnAwake = false;
            localspatialsound.outputAudioMixerGroup = spatialsound.outputAudioMixerGroup;
            localspatialsound.Play();
        }

        public void CustomBPlaySoundLoop(AudioSource spatialsound, AudioSource localspatialsound)
        {
            localspatialsound.clip = spatialsound.clip;
            localspatialsound.volume = 1.0f;
            localspatialsound.pitch = 1.0f;
            localspatialsound.loop = true;
            localspatialsound.playOnAwake = false;
            localspatialsound.outputAudioMixerGroup = spatialsound.outputAudioMixerGroup;
            localspatialsound.Play();
        }
    }
}
