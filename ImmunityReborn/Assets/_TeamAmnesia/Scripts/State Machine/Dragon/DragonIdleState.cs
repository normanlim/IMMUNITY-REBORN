using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonIdleState : DragonBaseState
{
    private readonly int GroundedStateName = Animator.StringToHash("Grounded");

    private const float CrossFadeDuration = 0.1f;

    public DragonIdleState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        PlaySFX.PlayThenDestroy(stateMachine.SFXAwake, stateMachine.transform);
        CameraShaker.Instance.Shake(5.0f, 3.0f);
        stateMachine.Animator.CrossFadeInFixedTime(GroundedStateName, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (IsInCombatRange())
        {
            stateMachine.SwitchState(new DragonTakingOffState(stateMachine));
            return;
        }

        FacePlayer(deltaTime);

        Move(deltaTime);
    }

    public override void Exit()
    {
        PlaySFX.PlayThenDestroy(stateMachine.SFXCombatStart, stateMachine.transform);
    }
}