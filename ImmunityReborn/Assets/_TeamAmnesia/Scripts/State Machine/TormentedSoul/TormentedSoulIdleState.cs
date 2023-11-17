using UnityEngine;

public class TormentedSoulIdleState : TormentedSoulBaseState
{
    private readonly int LocomotionStateName = Animator.StringToHash("Locomotion");

    private const float CrossFadeDuration = 0.1f;

    public TormentedSoulIdleState(TormentedSoulStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionStateName, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new TormentedSoulChasingState(stateMachine));
            return;
        }

        FacePlayer(deltaTime);

        UpdateLocomotionAnimator(deltaTime);
    }

    public override void Exit()
    {
    }
}
