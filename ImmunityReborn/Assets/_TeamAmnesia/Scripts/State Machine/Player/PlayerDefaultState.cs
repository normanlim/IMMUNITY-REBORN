using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultState : PlayerBaseState
{
    private readonly int LocomotionStateName = Animator.StringToHash("Locomotion");
    private readonly int AnimatorSpeedParam = Animator.StringToHash("Speed");

    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;

    public PlayerDefaultState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionStateName, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.IsAttacking) // player pressed attack
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.DefaultMovementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero) // if player is not moving
        {
            stateMachine.Animator.SetFloat(AnimatorSpeedParam, 0.0f, AnimatorDampTime, deltaTime); // updates blend tree state
        }
        else // if player is moving
        {
            stateMachine.Animator.SetFloat(AnimatorSpeedParam, 1.0f, AnimatorDampTime, deltaTime);
            FaceMovementDirection(movement, deltaTime);
        }
    }

    public override void Exit()
    {
    }
}
