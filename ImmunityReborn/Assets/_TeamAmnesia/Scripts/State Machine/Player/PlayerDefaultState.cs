using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultState : PlayerBaseState
{
    private readonly int BlendTreeSpeedParam = Animator.StringToHash("Speed");

    private const float AnimatorDampTime = 0.1f;

    public PlayerDefaultState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        stateMachine.CharacterController.Move(deltaTime * stateMachine.DefaultMovementSpeed * movement); // moves player

        if (stateMachine.InputReader.MovementValue == Vector2.zero) // if player is not moving
        {
            stateMachine.Animator.SetFloat(BlendTreeSpeedParam, 0.0f, AnimatorDampTime, deltaTime); // sets blend tree param to 0
        }
        else // if player is moving
        {
            stateMachine.Animator.SetFloat(BlendTreeSpeedParam, 1.0f, AnimatorDampTime, deltaTime); // sets blend tree param to 1
            FaceMovementDirection(movement, deltaTime);
        }
    }

    public override void Exit()
    {
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        forward.y = 0.0f; // movement only uses x and z values
        forward.Normalize(); // makes sure magnitude is 1

        Vector3 right = stateMachine.MainCameraTransform.right;
        right.y = 0.0f;
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y 
            + right * stateMachine.InputReader.MovementValue.x; // calculates movement relative to camera
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }
}
