using System;
using UnityEngine;
using UnityEngine.Windows;

namespace ThePatient
{
    public abstract class Interactable : MonoBehaviour, IInteractable, IPickupable
    // Should make child classes for inspectable objects and interactlable objects
    {
        protected bool isInspecting;

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        public override string ToString()
        {
            return transform.name;
        }

        // Interface implementation
        public bool OnHold { get; set; }
        public bool IsInspecting { get => isInspecting; set => isInspecting = value; }

        public abstract bool Interact();

        public abstract void OnFinishInteractEvent();

        public abstract void OnInteractEvent(string name);

        public virtual void Pickup()
        {
            Inventory.Instance.AddItem(this);
            gameObject.SetActive(false);
            Debug.Log("Pickup " + this.ToString());
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}
