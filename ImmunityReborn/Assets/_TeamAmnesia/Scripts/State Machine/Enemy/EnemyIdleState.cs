using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LocomotionStateName = Animator.StringToHash("Locomotion");

    private const float CrossFadeDuration = 0.1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionStateName, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        FacePlayer();
    }

    public override void Exit()
    {
    }
}
