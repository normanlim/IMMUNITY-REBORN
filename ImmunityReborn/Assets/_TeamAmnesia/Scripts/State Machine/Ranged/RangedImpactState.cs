using UnityEngine;

public class RangedImpactState : RangedBaseState
{

    private readonly int ImpactStateName = Animator.StringToHash("GetHitBow");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");
    private const float TransitionDuration = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    private float duration = 1.0f;
    public RangedImpactState(RangedStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactStateName, CrossFadeDuration, 1);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime); // applies ForceReceiver's movement

        UpdateLocomotionAnimator(deltaTime);

        // Enemy will stay in this state until countdown is finished
        duration -= deltaTime;

        if (duration <= 0.0f)
        {
            stateMachine.SwitchState(new RangedIdleState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.CrossFadeInFixedTime(EmptyDefaultStateName, TransitionDuration, 1);
    }
}
