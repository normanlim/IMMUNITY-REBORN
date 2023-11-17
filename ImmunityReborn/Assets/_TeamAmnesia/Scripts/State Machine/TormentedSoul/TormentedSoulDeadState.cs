using UnityEngine;

public class TormentedSoulDeadState : TormentedSoulBaseState
{
    private readonly int DeathStateName = Animator.StringToHash("DeathFrontBow");
    private const float CrossFadeDuration = 0.1f;
    public TormentedSoulDeadState(TormentedSoulStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DeathStateName, CrossFadeDuration, 0);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
