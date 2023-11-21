using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDeadState : RangedBaseState
{
    private readonly int DeathStateName = Animator.StringToHash("DeathFrontBow");
    private const float CrossFadeDuration = 0.1f;
    public RangedDeadState(RangedStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DeathStateName, CrossFadeDuration, 0);

        stateMachine.CharacterController.enabled = false;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
