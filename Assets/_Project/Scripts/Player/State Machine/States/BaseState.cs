
using UnityEngine;

namespace ThePatient
{
    public abstract class BaseState : IState
    {
        public readonly PlayerController player;
        public readonly Animator animator;

        // animation hash
        protected static readonly int locomotionHash = Animator.StringToHash("Locomotion_Stand");
        protected static readonly int crouchHash = Animator.StringToHash("Locomotion_Crouch");
        protected static readonly int jumpHash = Animator.StringToHash("Jump");
        protected static readonly int punch01Hash = Animator.StringToHash("Punching_01");
        protected static readonly int punch02Hash = Animator.StringToHash("Punching_02");
        protected static readonly int punch03Hash = Animator.StringToHash("Punching_03");

        protected const float crossFadeDuration = 0.1f;

        protected BaseState(PlayerController player, Animator animator)
        {
            this.player = player;
            this.animator = animator;
        }

        public virtual void FixedUpdate()
        {

            // no op
        }

        public virtual void OnEnter()
        {
            // no op
        }

        public virtual void OnExit()
        {
            // no op
        }

        public virtual void Update()
        {
            // no op
        }
    }
}
