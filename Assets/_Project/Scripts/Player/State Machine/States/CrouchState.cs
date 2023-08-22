using UnityEngine;

namespace ThePatient
{
    public class CrouchState : BaseState
    {
        public CrouchState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetFloat("crouchMultiplier", 3f);
            animator.CrossFade(crouchHash, crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }

}
