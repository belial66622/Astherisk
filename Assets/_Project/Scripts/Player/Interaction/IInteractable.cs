using System;
using UnityEngine;

namespace ThePatient
{
    public interface IInteractable
    {
        void OnInteractEvent();
        void OnFinishInteractEvent();
        bool Interact();
        bool OnHold { get; set; }
        bool IsInspecting { get; set; }
        Transform GetTransform();
    }
}
