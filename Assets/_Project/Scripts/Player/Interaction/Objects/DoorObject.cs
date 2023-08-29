using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient;
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
    [SerializeField] float closedRotation;
    [SerializeField] float openRotation;
    [SerializeField] float rattleRotation;
    [SerializeField] float openOrCloseDuration = 1f;
    [SerializeField] float rattleDuration = .05f;
    [SerializeField] float cooldownTime = 0f;
    [SerializeField] float lockDuration = 1f;

    CountdownTimer doorCooldownTimer;
    CountdownTimer rattleTimer;
    CountdownTimer openOrCloseTimer;
    StopwatchTimer lockTimer;

    IPickupable key;

    private void Start()
    {
        doorCooldownTimer = new CountdownTimer(cooldownTime);
        openOrCloseTimer = new CountdownTimer(openOrCloseDuration);
        rattleTimer = new CountdownTimer(rattleDuration);
        lockTimer = new StopwatchTimer();

        openOrCloseTimer.OnTimerStop += () => doorCooldownTimer.Start();
        lockTimer.OnTimerStop += () => doorCooldownTimer.Start();
        doorCooldownTimer.OnTimerStart += () => OnFinishInteractEvent();

        key = requiredKey.GetComponent<IPickupable>();
    }
    private void Update()
    {
        doorCooldownTimer.Tick(Time.deltaTime);
        openOrCloseTimer.Tick(Time.deltaTime);
        lockTimer.Tick(Time.deltaTime);
        rattleTimer.Tick(Time.deltaTime);

        if(OnHold && !lockTimer.IsRunning && !doorCooldownTimer.IsRunning && Inventory.Instance.HasItem(key))
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

    public override void Interact()
    {
        HandleDoorState(doorState);
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
                    doorState = DoorState.Locked;
                }
                else
                {
                    //open sound
                    StartCoroutine(ToggleDoor(closedRotation, openRotation, openOrCloseTimer));
                    doorState = DoorState.Opened;
                }
                break;
            case DoorState.Opened:
                //close sound
                StartCoroutine(ToggleDoor(openRotation, closedRotation, openOrCloseTimer));
                doorState = DoorState.Closed;
                break;
            case DoorState.Locked:
                if (lockTimer.GetTime() >= lockDuration)
                {
                    //unlock sound
                    doorState = DoorState.Closed;
                }
                else
                {
                    //ratlle sound
                    StartCoroutine(RattleDoorRepeat());
                }
                break;
        }
    }

    IEnumerator ToggleDoor(float startRot, float targetRot, TimerUtils time)
    {
        time.Start();

        Quaternion startRotation = Quaternion.AngleAxis(startRot, Vector3.up);
        Quaternion targetRotation = Quaternion.AngleAxis(targetRot, Vector3.up);

        while (time.InverseProgress < 1)
        {
            float t = time.InverseProgress * time.InverseProgress * (3 - 2 * time.InverseProgress);

            var currentRot = Quaternion.Slerp(startRotation, targetRotation, t);
            doorPivot.localRotation = currentRot;
            yield return null;
        }

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
            doorState == DoorState.Locked ? Inventory.Instance.HasItem(key) ? "Locked" : "Required Door Key" : "Press E To Interact with " + objectName));

        if (lockTimer.GetTime() >= .2f && lockTimer.GetTime() < lockDuration && doorState != DoorState.Opened)
        {
            EventAggregate<InteractionLockUIEventArgs>.Instance.TriggerEvent(new InteractionLockUIEventArgs(true, lockTimer.GetTime()));
        }
    }
}
