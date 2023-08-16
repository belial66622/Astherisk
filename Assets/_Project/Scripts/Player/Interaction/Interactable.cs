using UnityEngine;

namespace ThePatient
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public abstract bool OnHold { get; set; }

        public abstract void Interact();

        public abstract void OnFinishInteractEvent();

        public abstract void OnInteractEvent(string name);

        public override string ToString()
        {
            return transform.name;
        }
    }
}
