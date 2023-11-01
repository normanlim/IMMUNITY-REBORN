using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeIdleState : MeleeBaseState
{
    private readonly int LocomotionStateName = Animator.StringToHash("Locomotion");

    private const float CrossFadeDuration = 0.1f;

    public MeleeIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
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
            stateMachine.SwitchState(new MeleeChasingState(stateMachine));
            return;
        }

        FacePlayer();

        UpdateAnimator(deltaTime);
    }

    public override void Exit()
    {
    }
}
