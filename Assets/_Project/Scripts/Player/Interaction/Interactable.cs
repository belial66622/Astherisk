using UnityEngine;

namespace ThePatient
{
    public abstract class Interactable : MonoBehaviour, IInteractable, IPickupable
    {
        public bool OnHold { get; set; }

        public abstract void Interact();

        public abstract void OnFinishInteractEvent();

        public abstract void OnInteractEvent(string name);

        public virtual void Pickup()
        {
            Inventory.Instance.AddItem(this);
        }

        public override string ToString()
        {
            return transform.name;
        }
    }
}
