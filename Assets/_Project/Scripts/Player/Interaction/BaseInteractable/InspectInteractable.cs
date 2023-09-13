using System;
using UnityEngine;

namespace ThePatient
{
    public abstract class InspectInteractable : BaseInteractable
    {

        [SerializeField] protected InputReader _input;
        [SerializeField] protected Vector3 inspectRotation = new Vector3(0, 0, 0);
        [SerializeField][Range(.15f, .5f)] protected float zoomInspect = .5f;

        protected Transform defaultParent;
        protected Vector3 defaultPos;
        protected Quaternion defaultRot;
        protected Transform inspectParent;
        protected Vector3 defaultInspectPos;
        protected Quaternion defaultInspectRot;

        protected event Action<string> OnInspectExit = delegate { };

        float mouseX;
        float mouseY;

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
                    mouseX -= _input.InspectRotateInput.x * InteractableManager.Instance.InspectRotateSpeed;
                    mouseY += _input.InspectRotateInput.y * InteractableManager.Instance.InspectRotateSpeed;

                    mouseX = Mathf.Clamp(mouseX, -90, 90);
                    mouseY = Mathf.Clamp(mouseY, -90, 90);

                    var targetRotationX = Quaternion.Euler(Vector3.up * mouseX);
                    var targetRotationY = Quaternion.Euler(Vector3.right * mouseY);

                    inspectParent.transform.localRotation = Quaternion.Lerp(inspectParent.transform.localRotation, targetRotationY, 10 * Time.deltaTime);
                    gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, targetRotationX, 10 * Time.deltaTime);
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
            inspectParent.transform.localPosition = new Vector3(0, 0, zoomInspect);
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

                OnInspectExit?.Invoke("");
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

                OnInspectExit?.Invoke("");
            }
        }

        protected virtual void OnInspectEventStart() =>
            InteractableManager.Instance.OnInteractionInspect(new InteractionInspectEventArgs(true));
        protected virtual void OnInspectEventExit() =>
            InteractableManager.Instance.OnInteractionInspect(new InteractionInspectEventArgs(false));
    }
}
