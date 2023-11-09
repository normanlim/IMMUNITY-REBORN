using UnityEngine;

public class RangedAttackingState : RangedBaseState
{
    private readonly int ShootArrowAnimation = Animator.StringToHash("ShootArrow");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");
    private const float TransitionDuration = 0.1f;

    public RangedAttackingState(RangedStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ShootArrowAnimation, TransitionDuration, 1);
        stateMachine.Arrow.FireAtPlayer(stateMachine.AttackDamage, stateMachine.AttackKnockback);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        UpdateLocomotionAnimator(deltaTime);

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 1) >= 1.0f)
        {
            stateMachine.SwitchState(new RangedRetreatingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;

        stateMachine.Animator.CrossFade(EmptyDefaultStateName, TransitionDuration, 1); // without this, gets stuck at end of attack animation
    }
}
