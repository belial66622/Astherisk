using System;
using System.Collections;
using System.Collections.Generic;
using ThePatient.Player.InputActions;
using UnityEngine;
using UnityEngine.InputSystem;
using static ThePatient.Player.InputActions.PlayerInputActions;

namespace ThePatient
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "InputAction/PlayerInput")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        public event Action<Vector2> Move = delegate { };
        public event Action<Vector2, bool> Look = delegate { };
        public event Action<bool> Jump = delegate { };
        public event Action Crouch = delegate { };
        public event Action ToggleCrouch = delegate { };
        public event Action Sprint = delegate { };
        public event Action Fire = delegate { };
        public event Action Interact = delegate { };

        public Vector3 MoveInput => _inputs.Player.Move.ReadValue<Vector2>();
        public bool IsCrouching => crouchInputState;// _inputs.Player.Crouch.ReadValue<float>() > 0;
        public bool IsSprinting => _inputs.Player.Sprint.ReadValue<float>() > 0;
        public bool IsPunching => _inputs.Player.Fire.ReadValue<float>() > 0;
        public bool IsInteracting => _inputs.Player.Interact.ReadValue<float>() > 0;

        PlayerInputActions _inputs;
        bool crouchInputState;

        private void OnEnable()
        {
            if(_inputs == null)
            {
                _inputs = new PlayerInputActions();
                _inputs.Player.SetCallbacks(this);
            }
        }

        public void EnablePlayerControll()
        {
            _inputs.Enable();
            ResetInputState();
        }

        void ResetInputState()
        {
            crouchInputState = false;
        }

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
            switch(context.phase)
            {
                case InputActionPhase.Started:
                    Fire.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    Fire.Invoke();
                    break;
            }
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
    }
}
