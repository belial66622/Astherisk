using System;
using UnityEngine;

namespace ThePatient
{
    public abstract class InspectInteractable : Interactable
    {

        [SerializeField] protected InputReader _input;
        [SerializeField] protected Vector3 inspectRotation = new Vector3(0, 0, 0);

        protected Transform defaultParent;
        protected Vector3 defaultPos;
        protected Quaternion defaultRot;
        protected Transform inspectParent;
        protected Vector3 defaultInspectPos;
        protected Quaternion defaultInspectRot;

        protected event Action OnInspectExit = delegate { };
        protected event Action OnInspectDestroy = delegate { };

        protected override void Start()
        {
            inspectParent = InteractableManager.Instance.inspectTransform;
            defaultParent = transform.parent;
            defaultInspectPos = inspectParent.localPosition;
            defaultInspectRot = inspectParent.localRotation;
            defaultPos = transform.position;
            defaultRot = transform.rotation;
        }

        protected override void Update()
        {
            if (_input != null && isInspecting)
            {
                if (_input.InputInspecting)
                {
                    inspectParent.transform.Rotate(new Vector3(_input.InspectRotateInput.y, 0, 0) * .5f, Space.Self);
                    gameObject.transform.Rotate(new Vector3(0, -_input.InspectRotateInput.x, 0) * .5f, Space.Self);
                }
                if (_input.InspectZoomInput.y != 0)
                {
                    var zoomPos = inspectParent.transform.localPosition.z * -_input.InspectZoomInput.y * Time.deltaTime * .1f;
                    var clampZ = Mathf.Clamp(inspectParent.localPosition.z + zoomPos, 0.15f, .65f);
                    inspectParent.transform.localPosition = new Vector3(0, 0, clampZ);
                }
            }
        }
        protected virtual void Inspect()
        {
            Debug.Log("start inspect");
            isInspecting = true;
            InteractableManager.Instance.StartInspecting();
            gameObject.GetComponent<Collider>().enabled = false;
            transform.parent = inspectParent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(inspectRotation);

            OnInspectEventStart();
        }

        protected virtual void ExitInspect()
        {
            Debug.Log("stop inspect");
            if (isInspecting)
            {
                InteractableManager.Instance.StopInspecting();
                gameObject.GetComponent<Collider>().enabled = true;
                transform.parent = defaultParent;
                inspectParent.localPosition = defaultInspectPos;
                inspectParent.localRotation = defaultInspectRot;
                transform.position = defaultPos;
                transform.rotation = defaultRot;
                isInspecting = false;

                OnInspectEventExit();

                OnInspectExit?.Invoke();
            }
        }

        protected virtual void DestroyInspect()
        {
            Debug.Log("destroy inspect");
            if (isInspecting)
            {
                InteractableManager.Instance.StopInspecting();
                inspectParent.localPosition = defaultInspectPos;
                inspectParent.localRotation = defaultInspectRot;
                isInspecting = false;

                OnInspectEventExit();

                OnInspectDestroy?.Invoke();
            }
        }

        protected virtual void OnInspectEventStart() =>
            InteractableManager.Instance.OnInteractionInspect(new InteractionInspectEventArgs(true));
        protected virtual void OnInspectEventExit() =>
            InteractableManager.Instance.OnInteractionInspect(new InteractionInspectEventArgs(false));
    }
}
