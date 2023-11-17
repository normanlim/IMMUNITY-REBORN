using UnityEngine;

public class TormentedSoulAdvancingState : TormentedSoulBaseState
{
    private readonly int LocomotionStateName = Animator.StringToHash("Locomotion");

    private const float CrossFadeDuration = 0.1f;

    public TormentedSoulAdvancingState(TormentedSoulStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionStateName, CrossFadeDuration, -1);
    }

    public override void Tick(float deltaTime)
    {
        if (IsInAttackRange())
        {
            stateMachine.SwitchState(new TormentedSoulBasicAttackingState(stateMachine));  
        }

        MoveToPlayer(deltaTime);

        FacePlayer(deltaTime);

        UpdateLocomotionAnimator(deltaTime);
    }

    public override void Exit()
    {
    }
}
