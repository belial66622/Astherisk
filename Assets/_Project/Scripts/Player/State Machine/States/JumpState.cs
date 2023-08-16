using UnityEngine;

namespace ThePatient
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            //set animation crossfade
            animator.speed = 1.2f;
            animator.CrossFade(jumpHash, 0);
        }
        public override void OnExit()
        {
            animator.speed = 1;
            Debug.Log("exit jump state");
        }
        public override void FixedUpdate()
        {
            player.HandleJump();
            player.HandleMovement();
        }
    }
}
