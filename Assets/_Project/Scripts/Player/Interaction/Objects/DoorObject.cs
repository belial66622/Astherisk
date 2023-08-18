using System.Collections;
using ThePatient;
using UnityEngine;

public class DoorObject : Interactable
{
    public enum DoorState
    {
        Closed,
        Opened,
        Locked
    }
    [SerializeField] Transform doorPivot;

    [Header("Timer Parameter")]
    [SerializeField] float doorCooldown;
    [SerializeField] float timeToLock;


    [Header("Audio Parameter")]
    [SerializeField] AudioClip doorOpenAudio;
    [SerializeField] AudioClip lockUnlockAudio;
    [SerializeField] AudioClip rattleAudio;
    [SerializeField] new AudioSource audio;

    [Header("Door State Parameter")]
    [SerializeField] float closedRotation;
    [SerializeField] float openedRotation;
    [SerializeField] float rattleRotation;
    [SerializeField] private DoorState doorState;

    Coroutine coroutine;
    public float timerCooldown;
    public float lockTimer;
    [field: SerializeField] public bool openingDoor { get; private set; }
    [field: SerializeField] public override bool OnHold { get; set; }

    private void Update()
    {
        if(timerCooldown > 0) timerCooldown -= Time.deltaTime;

        if (OnHold)
        {
            lockTimer += Time.deltaTime;
            if(lockTimer >= timeToLock)
            {
                DoorStateHandle(doorState);
                OnFinishInteractEvent();
                OnHold = false;
            }
        }

    }

    private void DoorStateHandle(DoorState tempState)
    {
        if (timerCooldown <= 0)
        {
            switch (tempState)
            {
                case DoorState.Locked:
                    if (lockTimer >= timeToLock)
                    {
                        doorState = DoorState.Closed;
                        audio.PlayOneShot(lockUnlockAudio);
                        lockTimer = 0;
                        timerCooldown = doorCooldown / 2f;
                        break;
                    }
                    else
                    {
                        //StartCoroutine(RattleDoorRepeat());
                        //timerCooldown = doorCooldown / 2;
                    }
                    break;
                case DoorState.Opened:
                    audio.PlayOneShot(doorOpenAudio);
                    coroutine = StartCoroutine(OpenOrCloseDoor(openedRotation, closedRotation, doorCooldown));
                    timerCooldown = doorCooldown / 2;
                    break;
                case DoorState.Closed:
                    if (lockTimer >= timeToLock)
                    {
                        doorState = DoorState.Locked;
                        audio.PlayOneShot(lockUnlockAudio);
                        lockTimer = 0;
                        timerCooldown = doorCooldown / 2;
                        break;
                    }
                    else
                    {
                        coroutine = StartCoroutine(OpenOrCloseDoor(closedRotation, openedRotation, doorCooldown));
                        timerCooldown = doorCooldown / 2;
                    }
                    break;
                default:
                    break;
            }
        }
        
    }
    IEnumerator RattleDoorRepeat()
    {
        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(RattleDoor());
            yield return new WaitForSeconds(.125f);
        }
        doorPivot.transform.localRotation = Quaternion.Euler(0, closedRotation, 0);
    }
    IEnumerator RattleDoor()
    {
        openingDoor = true;
        audio.PlayOneShot(rattleAudio);
        StartCoroutine(ToogleDoor(closedRotation, rattleRotation, 20));
        yield return new WaitForSeconds(.075f);
        StartCoroutine(ToogleDoor(rattleRotation, closedRotation, 20));
        yield return new WaitForSeconds(.075f);
        openingDoor = false;
        
    }
    IEnumerator OpenOrCloseDoor(float startRot, float targetRot, float time)
    {
        openingDoor = true;

        audio.PlayOneShot(doorOpenAudio);

        //if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(ToogleDoor(startRot, targetRot, time));
        doorState = doorState == DoorState.Closed ? DoorState.Opened : DoorState.Closed;

        yield return null;

        openingDoor = false;
        
    }

    IEnumerator ToogleDoor(float startRot, float targetRot, float time)
    {
        var startRotation = startRot;
        float targetRotation = targetRot;
        float timer = 0;

        while (timer < 1)
        {
            float x = timer * timer * (3 - 2 * timer);

            float angle = Mathf.LerpAngle(startRotation, targetRotation, x);
            doorPivot.localRotation = Quaternion.Euler(doorPivot.localRotation.x, angle, doorPivot.localRotation.z);

            timer += Time.deltaTime * (1 / time);
            yield return null;
        }
        doorPivot.localEulerAngles = new Vector3(doorPivot.localRotation.x, targetRotation, doorPivot.localRotation.z);
    }

    public override void OnInteractEvent(string objectName)
    {
        EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(new InteractionTextEventArgs(true,
            doorState == DoorState.Locked ? $"LOCKED" : $"Press E To Interact with " + objectName));

        if (lockTimer >= .2f * timeToLock && lockTimer < timeToLock && doorState != DoorState.Opened && OnHold)
        {
            EventAggregate<InteractionLockUIEventArgs>.Instance.TriggerEvent(new InteractionLockUIEventArgs(true, (lockTimer / timeToLock)));
        }
    }


    public override void Interact()
    {
        if (openingDoor)
        {
            return;
        }
        DoorStateHandle(doorState);
        OnFinishInteractEvent();
        lockTimer = 0;
        timerCooldown = doorCooldown / 2f;
    }

    public override void OnFinishInteractEvent()
    {
        EventAggregate<InteractionTextEventArgs>.Instance.TriggerEvent(new InteractionTextEventArgs(false, ""));
        EventAggregate<InteractionLockUIEventArgs>.Instance.TriggerEvent(new InteractionLockUIEventArgs(false, 0));
    }
}