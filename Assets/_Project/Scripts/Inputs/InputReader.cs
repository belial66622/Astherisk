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
        IPlayerActions, IDialogueActions, IInteractionInspectActions, ILockPuzzleActions, IUIActions
    {
        //player action Events
        public event Action<Vector2> Move = delegate { };
        public event Action<Vector2, bool> Look = delegate { };
        public event Action<bool> Jump = delegate { };
        public event Action Crouch = delegate { };
        public event Action ToggleCrouch = delegate { };
        public event Action Sprint = delegate { };
        public event Action Interact = delegate { };
        public event Action<bool> Pause = delegate { };
        public event Action UIClick = delegate { };
        public bool UIClicked => _inputs.UI.Click.ReadValue<float>() > 0;

        //dialogue action Events
        public event Action NextDialogue = delegate { };

        //interaction action Events
        public event Action InspectClick = delegate { };
        public event Action<Vector2> InspectRotate = delegate { };
        public event Action<Vector2> InspectZoom = delegate { };
        public event Action InspectExit = delegate { };

        //lock puzzle action Events
        public event Action SelectUp = delegate { };
        public event Action SelectDown = delegate { };
        public event Action NumberLeft = delegate { };
        public event Action NumberRight = delegate { };

        // Player inputs values
        public Vector3 MoveInput => _inputs.Player.Move.ReadValue<Vector2>();
        public bool IsCrouching => crouchInputState;
        public bool IsSprinting => _inputs.Player.Sprint.ReadValue<float>() > 0;
        public bool IsInteracting => _inputs.Player.Interact.ReadValue<float>() > 0;

        //ON Interaction inputs values
        public bool InputInspecting => _inputs.InteractionInspect.InspectMouse.ReadValue<float>() > 0;
        public Vector2 InspectRotateInput => _inputs.InteractionInspect.InspectRotate.ReadValue<Vector2>();
        public Vector2 InspectZoomInput => _inputs.InteractionInspect.InspectZoom.ReadValue<Vector2>();
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
                _inputs.LockPuzzle.SetCallbacks(this);
                _inputs.UI.SetCallbacks(this);
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
            DisableLockPuzzleControl();
            DisableUIControl();
        }

        public void DisablePlayerControll() => _inputs.Player.Disable();
        public void EnableDialogueControll() => _inputs.Dialogue.Enable();
        public void DisableDialogueControll() => _inputs.Dialogue.Disable();
        public void EnableInspectControl() => _inputs.InteractionInspect.Enable();
        public void DisableInteractionControl() => _inputs.InteractionInspect.Disable();
        public void EnableLockPuzzleControl() => _inputs.LockPuzzle.Enable();
        public void DisableLockPuzzleControl() => _inputs.LockPuzzle.Disable();
        public void EnableUIControl() => _inputs.UI.Enable();
        public void DisableUIControl() => _inputs.UI.Disable();

        #region Player Input Action

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


 #endregion

        #region Dialogue Input Actions

        public void OnNextDialogue(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Performed:
                    NextDialogue.Invoke();
                    break;
            }
        }

        #endregion

        #region InteractionInspect Input Actions

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

        public void OnInspectZoom(InputAction.CallbackContext context)
        {
            InspectZoom.Invoke(context.ReadValue<Vector2>());
        }

        #endregion

        #region Lock Puzzle Inputs Actions

        public void OnSelectUp(InputAction.CallbackContext context)
        {
            context.action.performed += ctx => SelectUp.Invoke();
        }

        public void OnSelectDown(InputAction.CallbackContext context)
        {
            context.action.performed += ctx => SelectDown.Invoke();
        }

        public void OnNumberRight(InputAction.CallbackContext context)
        {
            context.action.performed += ctx => NumberRight.Invoke();
        }

        public void OnNumberLeft(InputAction.CallbackContext context)
        {
            context.action.performed += ctx => NumberLeft.Invoke();
        }
        #endregion

        #region UI Input Actions
        public void OnNavigate(InputAction.CallbackContext context)
        {
            // no op
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            // no op
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            // no op
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            // no op
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Performed:
                    UIClick.Invoke();
                    break;
            }
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            // no op
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            // no op
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            // no op
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            // no op
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            // no op
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Performed:
                    Pause.Invoke(true);
                    break;
            }
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    Pause.Invoke(false);
                    break;
            }
        }

        #endregion
    }
}
