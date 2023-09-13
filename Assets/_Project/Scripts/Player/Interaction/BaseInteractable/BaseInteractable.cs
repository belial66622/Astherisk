using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;

namespace ThePatient
{
    [SelectionBase]
    [RequireComponent(typeof(SaveableEntity))]
    public abstract class BaseInteractable : MonoBehaviour, IInteractable, ISaveable
    {
        protected bool isInspecting;

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        public override string ToString() => transform.name;

        #region IInteractable Implementation

        public bool OnHold { get; set; }
        public bool IsInspecting { get => isInspecting; set => isInspecting = value; }

        public abstract bool Interact();

        public abstract void OnFinishInteractEvent();

        public abstract void OnInteractEvent();

        public Transform GetTransform() => transform;

        public abstract object CaptureState();

        public abstract void RestoreState(object state);

        #endregion
    }
}
