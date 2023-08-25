using UnityEngine;
using UnityEngine.Windows;

namespace ThePatient
{
    public abstract class Interactable : MonoBehaviour, IInteractable, IPickupable
    {
        protected Vector3 defaultPos;
        protected Quaternion defaultRot;
        protected bool isInspecting;
        [SerializeField] protected InputReader _input;
        protected virtual void Start()
        {
            defaultPos = transform.position;
            defaultRot = transform.rotation;
        }
        protected virtual void Update()
        {
            if (_input != null && _input.IsInspecting && isInspecting)
            {
                gameObject.transform.Rotate(new Vector3(_input.InspectRotateInput.y, -_input.InspectRotateInput.x,
                    0), Space.Self);
            }
        }
        protected void Inspect()
        {
            Debug.Log("start inspect");
            isInspecting = true;
            InteractableManager.Instance.StartInspecting();
            gameObject.GetComponent<Collider>().enabled = false;
            Vector3 targetPos = InteractableManager.Instance.inspectTransform.position;
            transform.position = targetPos;
            transform.localRotation = InteractableManager.Instance.inspectTransform.rotation;
        }

        protected void StopInspect()
        {
            Debug.Log("stop inspect");
            if (isInspecting)
            {
                InteractableManager.Instance.StopInspecting();
                gameObject.GetComponent<Collider>().enabled = true;
                transform.position = defaultPos;
                transform.rotation = defaultRot;
                isInspecting = false;
            }
        }

        protected void DestroyInspect()
        {
            Debug.Log("destroy inspect");
            if (isInspecting)
            {
                InteractableManager.Instance.StopInspecting();
                gameObject.SetActive(false);
                isInspecting = false;
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
        }

    }
}
