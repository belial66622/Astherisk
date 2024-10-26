using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Windows;
using Utilities;

namespace ThePatient
{
    public class PlayerController : MonoBehaviour
    {
        // Events
        public event Action OnPlayerJump;
        public event Action OnPlayerLand;
        [SerializeField] CanvasUI _gameOver;

        [Header("References")]
        [SerializeField] InputReader _input;
        [SerializeField] Rigidbody _rb;
        [SerializeField] CapsuleCollider _collider;
        [SerializeField] Transform cameraHead;
        [SerializeField] Transform _orientation;
        [SerializeField] Animator _animator;
        [SerializeField] GroundChecker _groundChecker;
        [SerializeField] CinemachineVirtualCamera _virtualCamera;

        [Header("Movement Settings")]	
        [SerializeField] float _baseSpeed = 3f;
        [SerializeField] float _crouchSpeed = 3f;
        [SerializeField] float _sprintSpeed = 3f;

        [Header("Crouch Settings")]
        [SerializeField] float _crouchDuration = .25f;
        [SerializeField] float _crouchHeight = .5f;
        [SerializeField] float _standHeight = .5f;
        [SerializeField] float _crouchCenter = 1.5f;
        [SerializeField] float _standCenter = 1.5f;
        [SerializeField] float _crouchHeadPos = .5f;
        [SerializeField] float _standHeadPos = .5f;

        [Header("Step Height Settings")]
        [SerializeField] Transform _stepUpperTransform;
        [SerializeField] Transform _stepLowerTransform;
        [SerializeField] float _stepHitRange = .5f;
        [SerializeField] float _stepHeight = .3f;
        [SerializeField] float _stepSmooth = .1f;

        [Header("Camera Look Settings")]
        [SerializeField] float _jumpForce = 10f;
        [SerializeField] float _jumpDuration = .5f;
        [SerializeField] float _jumpCooldown = .1f;
        [SerializeField] float _gravityMultiplier = 3f;

        // Private Variables

        Transform _cam;

        Vector3 _moveDir;

        StateMachine _stateMachine;

        Coroutine _crouchCoroutine;

        float _jumpVelocity;

        // Timers
        List<TimerUtils> _timers;
        CountdownTimer _jumpTimer;
        CountdownTimer _jumpCooldownTimer;
        CountdownTimer _crouchTimer;
        StopwatchTimer _onCrouchTimer;

        private void Awake()
        {
            // Setup References
            _cam = Camera.main.transform;
            _stepUpperTransform.localPosition = new Vector3(0, _stepHeight, 0);
            _rb.freezeRotation = true;
            _gameOver = FindObjectOfType<CanvasUI>();

            SetupTimer();
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            //  State Machine
            _stateMachine = new StateMachine();

            //  Declare States
            var locomotionState = new LocomotionState(this, _animator);
            var jumpState = new JumpState(this, _animator);
            var crouchState = new CrouchState(this, _animator);

            //  State Predicates
            AtState(locomotionState, jumpState, new FuncPredicate(() => _jumpTimer.IsRunning));
            AtState(locomotionState, crouchState, new FuncPredicate(() => _onCrouchTimer.IsRunning));

            AnyState(locomotionState, new FuncPredicate(() =>
            _groundChecker.IsGrounded &&
            !_jumpTimer.IsRunning &&
            !_onCrouchTimer.IsRunning
            ));

            //  Initial State
            _stateMachine.SetState(locomotionState);
        }

        private void SetupTimer()
        {
            // Setup Timer
            _jumpTimer = new CountdownTimer(_jumpDuration);
            _jumpCooldownTimer = new CountdownTimer(_jumpCooldown);
            _crouchTimer = new CountdownTimer(_crouchDuration);
            _onCrouchTimer = new StopwatchTimer();

            // Setup Timers List
            _timers = new List<TimerUtils>(4) {
                _jumpTimer,
                _jumpCooldownTimer,
                _crouchTimer,
                _onCrouchTimer
            };

            // Setup Timer Events
            _jumpTimer.OnTimerStart += () => _jumpVelocity = _jumpForce;
            _jumpTimer.OnTimerStop += () => { _jumpCooldownTimer.Start(); OnPlayerLand?.Invoke(); };
            //_crouchTimer.OnTimerTickUpdate += (int tick) => { if (tick == 5) Debug.Log("Tick Number : " + tick); };
        }

        void AtState(IState fromState, IState toState, IPredicate predicate) => _stateMachine.AddTransition(fromState, toState, predicate);
        void AnyState(IState toState, IPredicate predicate) => _stateMachine.AddAnyTransition(toState, predicate);

        private void Start() => _input.EnablePlayerControll();

        private void OnEnable()
        {
            _input.Jump += OnJump;
            _input.ToggleCrouch += OnCrouch;
            _input.Crouch += OnCrouch;
            OnPlayerJump += () => AudioManager.Instance.PlaySFX("PlayerJump");
            OnPlayerLand += () => { AudioManager.Instance.PlaySFX("PlayerLand"); Debug.Log("land"); };
        }
        private void OnDisable()
        {
            _input.Jump -= OnJump;
            _input.ToggleCrouch -= OnCrouch;
            _input.Crouch -= OnCrouch;
            OnPlayerJump -= () => AudioManager.Instance.PlaySFX("PlayerJump");
            OnPlayerLand -= () => AudioManager.Instance.PlaySFX("PlayerLand");
        }

        private void Update()
        {
            _moveDir = new Vector3(_input.MoveInput.x, 0f, _input.MoveInput.y);
            HandleTimer();
            _stateMachine.Update();
        }
        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
        private void HandleTimer()
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                _timers[i].Tick(Time.deltaTime);
            }
        }
        public void HandleMovement()
        {
            //  Get the direction of the movement
            var adjustedDirection = Quaternion.AngleAxis(_cam.eulerAngles.y, Vector3.up) * _moveDir;


            //  Get the speed of the movement
            var speed = _input.IsCrouching ? _crouchSpeed : _input.IsSprinting ? _sprintSpeed : _baseSpeed;

            //  Set the animation parameters
            //_animator.SetFloat("speedX", _moveDir.x);
            //_animator.SetFloat("speedY", _moveDir.z);

            //  Check if the player is moving
            if (adjustedDirection.sqrMagnitude > 0f) 
            { 
                //  Move the player
                var _velocity = adjustedDirection * speed * Time.fixedDeltaTime;
            
                _rb.velocity = new Vector3(_velocity.x, _rb.velocity.y, _velocity.z);

                // Climb over steps
                ClimbOverSteps();
            }
            else
            {
                //  Stop the player 
                _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
                
            }

        }
        private void ClimbOverSteps()
        {
            //  Check for every 45 degrees
            for (int i = 0; i <= 180; i+=45)
            {
                //  Get the direction of the raycast
                Vector3 direction = Quaternion.AngleAxis(i, _orientation.up) * _orientation.right;

                //  Check if there is a step in front of the player
                if (Physics.Raycast(_stepLowerTransform.position, transform.TransformDirection(direction), out _, _stepHitRange))
                {
                    if (!Physics.Raycast(_stepUpperTransform.position, transform.TransformDirection(direction), out _, _stepHitRange * 1.3f))
                    {
                        _rb.position += new Vector3(0f, _stepSmooth, 0f);
                    }
                }
                if (i > 0 && i < 180)
                {
                    if (Physics.Raycast(_stepLowerTransform.position, transform.TransformDirection(-direction), out _, _stepHitRange))
                    {
                        if (!Physics.Raycast(_stepUpperTransform.position, transform.TransformDirection(-direction), out _, _stepHitRange * 1.3f))
                        {
                            _rb.position += new Vector3(0f, _stepSmooth, 0f);
                        }
                    }
                }
            }
        }
        public void HandleJump()
        {
            // set jump velocity to 0 if grounded
            if(!_jumpTimer.IsRunning && _groundChecker.IsGrounded)
            {
                _jumpVelocity = 0f;
                _jumpTimer.Stop();
                return;
            }

            // Apply Gravity
            if(!_jumpTimer.IsRunning)
            {
                _jumpVelocity += Physics.gravity.y * _gravityMultiplier * Time.fixedDeltaTime;
            }

            // OPTIONAL :: Apply Jump Velocity Here
        }

        IEnumerator ToggleCrouchStand(bool isCrouch)
        {
            //  Start the crouch timer
            _crouchTimer.Start();

            //  Check if the player is crouching
            if (isCrouch)
                _onCrouchTimer.Start();

            _animator.speed = 1.8f;

            //  Get the start and target values
            float startHeight = _collider.height;
            float targetHeight = !isCrouch ?  _standHeight : _crouchHeight ;
            float startCenter = _collider.center.y;
            float targetCenter = !isCrouch ? _standCenter : _crouchCenter;
            float startHeadPos = cameraHead.transform.localPosition.y;
            float targetHeadPos = !isCrouch ? _standHeadPos : _crouchHeadPos;

            //  Lerp the values
            while (_crouchTimer.InverseProgress < 1)
            {
                //  Get the timer progress
                float timer = _crouchTimer.InverseProgress;

                //  Apply the easing function
                float t = timer * timer * (3 - 2 * timer);

                //  set collider height
                float heightProgress = Mathf.Lerp(startHeight, targetHeight, t);
                _collider.height = heightProgress;

                //  set collider center
                float centerProgress = Mathf.Lerp(startCenter, targetCenter, t);
                _collider.center = new Vector3(_collider.center.x, centerProgress, _collider.center.z);
    
                //  set camera head position    
                float cameraHeadPos = Mathf.Lerp(startHeadPos, targetHeadPos, t);
                cameraHead.transform.localPosition = new Vector3(0, cameraHeadPos, 0);
                yield return null;
            }

            //  Set the final values
            _collider.height = targetHeight;
            _collider.center = new Vector3(_collider.center.x, targetCenter, _collider.center.z);
            cameraHead.transform.localPosition = new Vector3(0, targetHeadPos, 0);

            _animator.speed = 1f;

            //  Stop the crouch timer
            _crouchTimer.Stop();
        }

        private void OnJump(bool performed)
        {
            if(performed && !_jumpTimer.IsRunning && !_jumpCooldownTimer.IsRunning && _groundChecker.IsGrounded)
            {
                _jumpTimer.Start();
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpVelocity, _rb.velocity.z);
                performed = false;
                OnPlayerJump?.Invoke();
            }
        }
        private void OnCrouch()
        {
            //Do Crouch
            if(_input.IsCrouching && !_onCrouchTimer.IsRunning && _groundChecker.IsGrounded)
            {
                if (_crouchCoroutine != null)
                    StopCoroutine(_crouchCoroutine);

                //_animator.CrossFade(standToCrouch, 0);
                _crouchCoroutine = StartCoroutine(ToggleCrouchStand(_input.IsCrouching));
            }
            //Stand up
            else if(!_input.IsCrouching || _onCrouchTimer.IsRunning)
            {
                if (_crouchCoroutine != null)
                    StopCoroutine(_crouchCoroutine);

                //_animator.CrossFade(crouchToStand, 0);
                _onCrouchTimer.Stop();
                _crouchCoroutine = StartCoroutine(ToggleCrouchStand(_input.IsCrouching));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ITrigger[] itrigger;
            INoCollision icheckcollider;
            Icheck icheck;
            itrigger = other.GetComponents<ITrigger>();

            icheckcollider = other.GetComponent<INoCollision>();
            icheck = other.GetComponent<Icheck>();

            if(icheckcollider != null)
            { 
                if (icheckcollider.CheckCollision())
                {
                    foreach (ITrigger triggerobj in itrigger)

                    //if (other.TryGetComponent(out ITrigger triggerobj))
                    {
                        triggerobj.DoSomething();
                    }
                }
            }

            if(icheck != null)
            {
                icheck.OnEnter();
            }

            if (other.TryGetComponent(out Ghost ghost))
            {
                _gameOver.GameoverHUD();
                Debug.Log("gameover");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Icheck icheck;
            icheck = other.GetComponent<Icheck>();
            if (icheck != null)
            {
                icheck.OnExit();
            }
        }
    }
}
