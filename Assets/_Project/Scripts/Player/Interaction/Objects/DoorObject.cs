using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Utilities;

public class DoorObject : Interactable
{
    enum DoorState
    {
        Closed,
        Opened,
        Locked
    }

    [Header("Reference")]
    [SerializeField] Transform doorPivot;
    [SerializeField] DoorState doorState = DoorState.Closed;
    [SerializeField] GameObject requiredKey;

    [Header("Door Settings")]
    [SerializeField] Vector3 defaultRotAxis = Vector3.forward;
    [SerializeField] float closedRotation;
    [SerializeField] float openRotation = 120;
    [SerializeField] float rattleRotation = 1;
    [SerializeField] float openOrCloseDuration = 1f;
    [SerializeField] float rattleDuration = .05f;
    [SerializeField] float cooldownTime = 0f;
    [SerializeField] float lockDuration = 1f;

    CountdownTimer doorCooldownTimer;
    CountdownTimer rattleTimer;
    CountdownTimer openOrCloseTimer;
    StopwatchTimer lockTimer;

    Quaternion defaultRotation;

    IPickupable key;
    bool needKey = false;
    AudioSource _spatialsound;

    protected override void Start()
    {
        _spatialsound = this.AddComponent<AudioSource>();
        defaultRotation = doorPivot.localRotation;
        doorCooldownTimer = new CountdownTimer(cooldownTime);
        openOrCloseTimer = new CountdownTimer(openOrCloseDuration);
        rattleTimer = new CountdownTimer(rattleDuration);
        lockTimer = new StopwatchTimer();

        openOrCloseTimer.OnTimerStop += () => doorCooldownTimer.Start();
        lockTimer.OnTimerStop += () => doorCooldownTimer.Start();
        doorCooldownTimer.OnTimerStart += () => OnFinishInteractEvent();

        needKey = requiredKey != null;
    
        if (needKey)
            key = requiredKey.GetComponent<IPickupable>();
    }

    private void OnEnable()
    {
    }

    protected override void Update()
    {
        doorCooldownTimer.Tick(Time.deltaTime);
        openOrCloseTimer.Tick(Time.deltaTime);
        lockTimer.Tick(Time.deltaTime);
        rattleTimer.Tick(Time.deltaTime);

        if(OnHold && !lockTimer.IsRunning && !doorCooldownTimer.IsRunning && needKey && Inventory.Instance.HasItem(key))
        {
            lockTimer.Start();
        }
        else if(!OnHold && lockTimer.IsRunning && lockTimer.GetTime() < lockDuration)
        {
            OnFinishInteractEvent();
            lockTimer.Reset();
            lockTimer.Stop();
        }
        else if(OnHold && lockTimer.GetTime() >= lockDuration)
        {
            Interact();
            OnFinishInteractEvent();
            lockTimer.Reset();
            lockTimer.Stop();
        }
    }

    public override bool Interact()
    {
        HandleDoorState(doorState);
        return false;
    }

    private void HandleDoorState(DoorState state)
    {
        if(doorCooldownTimer.IsRunning) return;
        if(openOrCloseTimer.IsRunning) return;

        switch (state)
        {
            case DoorState.Closed:
                if (lockTimer.GetTime() >= lockDuration)
                {
                    //locked sound
                    playsound(AudioManager.Instance.GetSfx("LockDoor"));
                    doorState = DoorState.Locked;
                }
                else
                {
                    //open sound
                    playsound(AudioManager.Instance.GetSfx("Door"));
                    StartCoroutine(ToggleDoor(closedRotation, openRotation, openOrCloseTimer));
                    doorState = DoorState.Opened;
                }
                break;
            case DoorState.Opened:
                //close sound
                playsound(AudioManager.Instance.GetSfx("Door"));
                StartCoroutine(ToggleDoor(openRotation, closedRotation, openOrCloseTimer));
                doorState = DoorState.Closed;
                break;
            case DoorState.Locked:
                if (lockTimer.GetTime() >= lockDuration)
                {
                    //unlock sound
                    playsound(AudioManager.Instance.GetSfx("UnlockDoor"));
                    doorState = DoorState.Closed;
                }
                else
                {
                    //ratlle sound
                    playsound(AudioManager.Instance.GetSfx("RattleDoor"));
                    StartCoroutine(RattleDoorRepeat());
                }
                break;
        }
    }

    void playsound(AudioSource spatialsound)
    {
        _spatialsound.clip = spatialsound.clip;
        _spatialsound.volume = 1.0f;
        _spatialsound.pitch = 1.0f;
        _spatialsound.loop = spatialsound.loop;
        _spatialsound.playOnAwake = false;
        _spatialsound.outputAudioMixerGroup = spatialsound.outputAudioMixerGroup;
        _spatialsound.spatialBlend = spatialsound.spatialBlend;
        _spatialsound.Play();
    }

    IEnumerator ToggleDoor(float startRot, float targetRot, TimerUtils time)
    {
        time.Start();

        Quaternion startRotation = Quaternion.AngleAxis(startRot, defaultRotAxis );
        Quaternion targetRotation = Quaternion.AngleAxis(targetRot, defaultRotAxis );



        while (time.InverseProgress < 1)
        {
            float t = time.InverseProgress * time.InverseProgress * (3 - 2 * time.InverseProgress);

            var currentRot = Quaternion.Slerp(startRotation, targetRotation, t);

            doorPivot.localRotation = defaultRotation  * currentRot;
            yield return null;
        }

        doorPivot.localRotation = defaultRotation * targetRotation;

        time.Stop();
    }
    IEnumerator RattleDoorRepeat()
    {
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(RattleDoorOnce());
            yield return new WaitForSeconds(.125f);
        }
    }
    IEnumerator RattleDoorOnce()
    {
        //audioSource.PlayOneShot(rattleAudio);
        StartCoroutine(ToggleDoor(closedRotation, rattleRotation, rattleTimer));
        yield return new WaitForSeconds(.075f);
        StartCoroutine(ToggleDoor(rattleRotation, closedRotation, rattleTimer));
        yield return new WaitForSeconds(.075f);
    }

    public override void OnFinishInteractEvent()
    {
        EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(new InteractionTextEventArgs(false, ""));
        EventAggregate<InteractionLockUIEventArgs>.Instance.TriggerEvent(new InteractionLockUIEventArgs(false, 0));
    }

    

    public override void OnInteractEvent(string objectName)
    {
        EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(new InteractionTextEventArgs(true,
            GetText(doorState, objectName)));

        if (lockTimer.GetTime() >= .2f && lockTimer.GetTime() < lockDuration && doorState != DoorState.Opened)
        {
            EventAggregate<InteractionLockUIEventArgs>.Instance.TriggerEvent(new InteractionLockUIEventArgs(true, lockTimer.GetTime()));
        }
    }
    string GetText(DoorState state, string name)
    {
        if (state == DoorState.Locked)
        {
            if (Inventory.Instance.HasItem(key))
            {
                return $"[ HOLD E ]\nUnlock";
            }
            else
            {
                return "Required Key";
            }
        }
        return $"[ E ]\nInteract With {name}";
    }
}
