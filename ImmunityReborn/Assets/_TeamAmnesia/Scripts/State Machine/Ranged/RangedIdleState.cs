using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedIdleState : RangedBaseState
{
    private readonly int LocomotionStateName = Animator.StringToHash("Locomotion");

    private const float CrossFadeDuration = 0.1f;

    public RangedIdleState(RangedStateMachine stateMachine) : base(stateMachine)
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
            stateMachine.SwitchState(new RangedChasingState(stateMachine));
            return;
        }

        FacePlayer(deltaTime);

        Move(deltaTime);

        UpdateLocomotionAnimator(deltaTime);
    }

    public override void Exit()
    {
    }
}
