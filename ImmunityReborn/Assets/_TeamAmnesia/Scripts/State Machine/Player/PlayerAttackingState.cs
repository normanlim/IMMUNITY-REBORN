using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private readonly int MemoryAttackStateName = Animator.StringToHash("Sword And Shield Casting");

    private const float CrossFadeDuration = 0.1f;

    public PlayerAttackingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        PlaySFX.PlayThenDestroy(stateMachine.SFXMemoryAttackActivate, stateMachine.transform);
        stateMachine.Animator.CrossFadeInFixedTime(MemoryAttackStateName, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 1) >= 0.7f) // make comparing with less than (1.0f - Transition Duration) in Animator Transition Settings
        {
            stateMachine.SwitchState(new PlayerDefaultState(stateMachine));
            return;
        }

        FaceCameraDirection(deltaTime);

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.DefaultMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);
    }

    public override void Exit()
    {
    }
}
