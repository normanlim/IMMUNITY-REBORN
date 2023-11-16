using UnityEngine;

public class TormentedSoulBasicAttackingState : TormentedSoulBaseState
{
    private readonly int ShootArrowAnimation = Animator.StringToHash("ShootArrow");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");
    private const float TransitionDuration = 0.1f;
    private static readonly Vector3 TripleFireOffset = new Vector3(0, 0, 3);
    public TormentedSoulBasicAttackingState(TormentedSoulStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ShootArrowAnimation, TransitionDuration, 1);
        // Triple fire 3 arrows in a cone
        stateMachine.ProjectileShooter.FireAtTarget(stateMachine.AttackDamage, stateMachine.AttackKnockback);
        stateMachine.ProjectileShooter.FireAtTarget(stateMachine.AttackDamage, stateMachine.AttackKnockback, TripleFireOffset);
        stateMachine.ProjectileShooter.FireAtTarget(stateMachine.AttackDamage, stateMachine.AttackKnockback, -TripleFireOffset);
        stateMachine.NormalAttackCount++;
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        UpdateLocomotionAnimator(deltaTime);

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 1) >= 1.0f)
        {
            stateMachine.SwitchState(new TormentedSoulRetreatingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;

        stateMachine.Animator.CrossFade(EmptyDefaultStateName, TransitionDuration, 1); // without this, gets stuck at end of attack animation
    }
}
