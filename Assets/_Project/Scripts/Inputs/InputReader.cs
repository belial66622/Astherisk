using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient.Player.InputActions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static ThePatient.Player.InputActions.PlayerInputActions;

namespace ThePatient
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "InputAction/PlayerInput")]
    public class InputReader : ScriptableObject, 
        IPlayerActions, IDialogueActions, IInteractionInspectActions
    {
        //player action inputs
        public event Action<Vector2> Move = delegate { };
        public event Action<Vector2, bool> Look = delegate { };
        public event Action<bool> Jump = delegate { };
        public event Action Crouch = delegate { };
        public event Action ToggleCrouch = delegate { };
        public event Action Sprint = delegate { };
        public event Action Interact = delegate { };
       
        //dialogue action inputs
        public event Action NextDialogue = delegate { };

        //interaction action inputs
        public event Action InspectClick = delegate { };
        public event Action<Vector2> InspectRotate = delegate { };
        public event Action InspectExit = delegate { };

        // Player interaction inputs
        public Vector3 MoveInput => _inputs.Player.Move.ReadValue<Vector2>();
        public bool IsCrouching => crouchInputState;// _inputs.Player.Crouch.ReadValue<float>() > 0;
        public bool IsSprinting => _inputs.Player.Sprint.ReadValue<float>() > 0;
        public bool IsInteracting => _inputs.Player.Interact.ReadValue<float>() > 0;

        //ON Interaction inputs
        public bool IsInspecting => _inputs.InteractionInspect.InspectMouse.ReadValue<float>() > 0;
        public Vector2 InspectRotateInput => _inputs.InteractionInspect.InspectRotate.ReadValue<Vector2>();
        public bool IsInspectExit => _inputs.InteractionInspect.InspectExit.ReadValue<float>() > 0;

        // private variables
        PlayerInputActions _inputs;
        bool crouchInputState;

        private void OnEnable()
        {
            if(_inputs == null)
            {
                _inputs = new PlayerInputActions();
                _inputs.Player.SetCallbacks(this);
                _inputs.Dialogue.SetCallbacks(this);
                _inputs.InteractionInspect.SetCallbacks(this);
            }
        }

        public void EnablePlayerControll()
        {
            ResetInputState();
            _inputs.Player.Enable();
        }
        void ResetInputState()
        {
            crouchInputState = false;
            DisableDialogueControll();
            DisableInteractionControl();
        }

        public void DisablePlayerControll() => _inputs.Player.Disable();
        public void EnableDialogueControll() => _inputs.Dialogue.Enable();
        public void DisableDialogueControll() => _inputs.Dialogue.Disable();
        public void EnableInspectControl() => _inputs.InteractionInspect.Enable();
        public void DisableInteractionControl() => _inputs.InteractionInspect.Disable();

        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }

        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";
        
        public void OnFire(InputAction.CallbackContext context)
        {

        }
        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Performed:
                    Interact.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    Interact.Invoke();
                    break;
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    crouchInputState = true;
                    Crouch.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    crouchInputState = false;
                    Crouch.Invoke();
                    break;
            }
        }

        public void OnToggleCrouch(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    crouchInputState = !crouchInputState;
                    ToggleCrouch.Invoke();
                    break;
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Started:
                    Sprint.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    Sprint.Invoke();
                    break;
            }
        }

        public void OnNextDialogue(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Performed:
                    NextDialogue.Invoke();
                    break;
            }
        }


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
            switch(context.phase)
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
