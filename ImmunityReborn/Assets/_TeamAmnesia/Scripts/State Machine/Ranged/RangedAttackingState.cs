using UnityEngine;

public class RangedAttackingState : RangedBaseState
{
    private readonly int AnimatorAttackStateName = Animator.StringToHash("Attack2SwordShield");
    private readonly int DefaultEmptyStateName = Animator.StringToHash("Default Empty");
    private const float TransitionDuration = 0.1f;
    private const float AnimationTime = 3f;
    private float elapsedTime;

    public RangedAttackingState(RangedStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.FireArrow.FireArrowAtPlayer(stateMachine.AttackDamage, stateMachine.AttackKnockback);
        stateMachine.Animator.CrossFadeInFixedTime(AnimatorAttackStateName, TransitionDuration, 1);
        elapsedTime = 0;
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        UpdateLocomotionAnimator(deltaTime);
        elapsedTime += Time.deltaTime;
        // @TODO PLACEHOLDER, get rid of elapsedTime when there's an attack animation and switch back to circulating when it finish
        if (elapsedTime >= AnimationTime)
        {
            stateMachine.SwitchState(new RangedRetreatingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;

        stateMachine.Animator.CrossFade(DefaultEmptyStateName, TransitionDuration, 1); // without this, gets stuck at end of attack animation
    }
}
