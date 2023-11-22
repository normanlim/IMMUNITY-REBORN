using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int ImpactStateName = Animator.StringToHash("GetHitFront2");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1.0f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        PlaySFX.PlayThenDestroy(stateMachine.SFXTakeDamage, stateMachine.transform);
        stateMachine.Animator.CrossFadeInFixedTime(ImpactStateName, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // Player will stay in this state until countdown is finished
        duration -= deltaTime;

        if (duration <= 0.0f)
        {
            stateMachine.SwitchState(new PlayerDefaultState(stateMachine));
        }

        // The following is copied over from PlayerDefaultState
        // To restrict player actions while they're being hit, remove its respective line of code
        // Ex. if player can't move after being hit, remove Move()
        FaceCameraDirection(deltaTime);

        if (stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.DefaultMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);
    }

    public override void Exit()
    {
    }
}
