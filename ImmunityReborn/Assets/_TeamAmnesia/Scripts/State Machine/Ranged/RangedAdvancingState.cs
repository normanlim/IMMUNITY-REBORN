using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAdvancingState : RangedBaseState
{
    private readonly int LocomotionStateName = Animator.StringToHash("Locomotion");

    private const float CrossFadeDuration = 0.1f;

    public RangedAdvancingState(RangedStateMachine stateMachine) : base(stateMachine)
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
            stateMachine.SwitchState(new RangedAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);

        FacePlayer(deltaTime);

        UpdateLocomotionAnimator(deltaTime);
    }

    public override void Exit()
    {
    }
}
