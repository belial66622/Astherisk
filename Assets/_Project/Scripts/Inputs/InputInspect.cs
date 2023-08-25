using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient.Player.InputActions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static ThePatient.Player.InputActions.PlayerInputActions;

namespace ThePatient
{
    public class InputInspect : MonoBehaviour, IInteractionInspectActions
    {
        PlayerInputActions inputs;

        //interaction action inputs
        public event Action InspectClick = delegate { };
        public event Action<Vector2> InspectRotate = delegate { };
        public event Action InspectExit = delegate { };

        //ON Interaction inputs
        public bool IsInspecting => inputs.InteractionInspect.InspectMouse.ReadValue<float>() > 0;
        public Vector2 InspectRotateInput => inputs.InteractionInspect.InspectRotate.ReadValue<Vector2>();
        public bool IsInspectExit => inputs.InteractionInspect.InspectExit.ReadValue<float>() > 0;

        private void OnEnable()
        {
            if (inputs == null)
            {
                inputs = new PlayerInputActions();
                inputs.InteractionInspect.SetCallbacks(this);
            }
        }

        public void EnableInspectInput() => inputs.InteractionInspect.Enable();
        public void DisableInspectInput() => inputs.InteractionInspect.Disable();

        public void OnInspectMouse(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    InspectClick.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    InspectClick.Invoke();
                    break;
            }
        }

        public void OnInspectRotate(InputAction.CallbackContext context)
        {
            InspectRotate.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInspectExit(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Debug.Log("inspect exit");
                    InspectExit.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    InspectExit.Invoke();
                    break;
            }
        }
    }
}
