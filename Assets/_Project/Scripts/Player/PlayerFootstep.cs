using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace ThePatient
{
    public class PlayerFootstep : MonoBehaviour
    {
        [Header("Footstep Audio")]
        [SerializeField] InputReader input;
        [SerializeField] AudioClip[] footstepAudio;
        Queue<AudioClip> footstepAudioQueue = new Queue<AudioClip>();
        List<AudioClip> tempFootstepAudio = new List<AudioClip>();

        [Header("Footstep Parameter")]
        [SerializeField] float baseStepSpeed = .5f;
        [SerializeField][Range(1, 2)] float sprintStepMultiplier = 1.6f;
        [SerializeField][Range(0, 1)] float crouchStepMultiplier = .6f;
        [SerializeField] AudioSource playerAudioSource = default;
        public float footstepTimer = 0f;
        float GetCurrentOffset => input.IsCrouching ? baseStepSpeed / crouchStepMultiplier :
            input.IsSprinting ? baseStepSpeed / sprintStepMultiplier :
            baseStepSpeed;

        GroundChecker groundChecker;
        private void Awake()
        {
            groundChecker = GetComponent<GroundChecker>();
        }

        private void Update()
        {
            HandleFootstep();
        }

        private void HandleFootstep()
        {
            if (!groundChecker.IsGrounded) return;
            if (input.MoveInput.magnitude <= 0) return;

            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                PlayFootstepAudio(playerAudioSource);
                footstepTimer = GetCurrentOffset;
            }

        }

        public void SetFootstepAudioQueue()
        {
            tempFootstepAudio.Clear();
            for (int i = 0; i < footstepAudio.Length - 1; i++)
            {
                tempFootstepAudio.Add(footstepAudio[i]);
            }

            footstepAudioQueue.Clear();
            foreach (AudioClip audio in tempFootstepAudio)
            {
                footstepAudioQueue.Enqueue(audio);
            }
        }

        public void PlayFootstepAudio(AudioSource audioSource)
        {

            if (footstepAudioQueue.Count == 0)
            {
                Array.Reverse(footstepAudio);
                SetFootstepAudioQueue();
            }

            AudioClip firstQueue = footstepAudioQueue.Dequeue();
            audioSource.PlayOneShot(firstQueue);
        }

    }
}
