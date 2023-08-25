using System;
using UnityEngine;
using UnityEngine.Windows;

namespace ThePatient
{
    public abstract class Interactable : MonoBehaviour, IInteractable, IPickupable
    {
        protected Transform defaultParent;
        protected Vector3 defaultPos;
        protected Quaternion defaultRot;
        protected Transform inspectParent;
        protected Vector3 defaultInspectPos;
        protected bool isInspecting;
        [SerializeField] protected InputReader _input;

        protected event Action OnInspectExit = delegate { };
        protected event Action OnInspectDestroy = delegate { };
        protected virtual void Start()
        {
            inspectParent = InteractableManager.Instance.inspectTransform;
            defaultParent = transform.parent;
            defaultInspectPos = inspectParent.localPosition;
            defaultPos = transform.position;
            defaultRot = transform.rotation;
        }

        protected virtual void Update()
        {
            if (_input != null && isInspecting)
            {
                if (_input.InputInspecting)
                {
                    gameObject.transform.Rotate(new Vector3(_input.InspectRotateInput.y, -_input.InspectRotateInput.x,
                        0), Space.Self);    
                }
                if(_input.InspectZoomInput.y != 0)
                {
                    var zoomPos = inspectParent.transform.forward * -_input.InspectZoomInput.y * Time.deltaTime * .1f;
                    var clampZ = Mathf.Clamp(inspectParent.localPosition.z + zoomPos.z, 0.1f, .65f);
                    inspectParent.transform.localPosition = new Vector3(0, 0, clampZ);
                }
            }
        }
        protected void Inspect()
        {
            Debug.Log("start inspect");
            isInspecting = true;
            InteractableManager.Instance.StartInspecting();
            gameObject.GetComponent<Collider>().enabled = false;
            EventAggregate<InteractionInspectEventArgs>.Instance.TriggerEvent(new InteractionInspectEventArgs(true));
            transform.parent = inspectParent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        protected void ExitInspect()
        {
            Debug.Log("stop inspect");
            if (isInspecting)
            {
                InteractableManager.Instance.StopInspecting();
                gameObject.GetComponent<Collider>().enabled = true;
                EventAggregate<InteractionInspectEventArgs>.Instance.TriggerEvent(new InteractionInspectEventArgs(false));
                transform.parent = defaultParent;
                inspectParent.localPosition = defaultInspectPos;
                transform.position = defaultPos;
                transform.rotation = defaultRot;
                isInspecting = false;
                OnInspectExit?.Invoke();
            }
        }

        protected void DestroyInspect()
        {
            Debug.Log("destroy inspect");
            if (isInspecting)
            {
                InteractableManager.Instance.StopInspecting();
                EventAggregate<InteractionInspectEventArgs>.Instance.TriggerEvent(new InteractionInspectEventArgs(false));
                isInspecting = false;
                OnInspectDestroy?.Invoke();
            }
        }

        public bool OnHold { get; set; }

        public abstract void Interact();

        public abstract void OnFinishInteractEvent();

        public abstract void OnInteractEvent(string name);

        public override string ToString()
        {
            return transform.name;
        }

        public virtual void Pickup()
        {
            Inventory.Instance.AddItem(this);
            gameObject.SetActive(false);
            Debug.Log("Pickup " + this.ToString());
        }

    }
}
