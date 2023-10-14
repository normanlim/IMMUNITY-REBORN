using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// Moves the player based on player input and gravity
    /// </summary>
    /// <param name="movement"></param>
    /// <param name="deltaTime"></param>
    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.CharacterController.Move((movement + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    /// <summary>
    /// Use this if player movement input is not allowed
    /// </summary>
    /// <param name="deltaTime"></param>
    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    /// <summary>
    /// Calculates movement relative to follow camera
    /// </summary>
    /// <returns></returns>
    protected Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        forward.y = 0.0f; // movement only uses x and z values
        forward.Normalize(); // makes sure magnitude is 1

        Vector3 right = stateMachine.MainCameraTransform.right;
        right.y = 0.0f;
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y
            + right * stateMachine.InputReader.MovementValue.x;
    }

    /// <summary>
    /// Face the direction of movement
    /// </summary>
    /// <param name="movement"></param>
    /// <param name="deltaTime"></param>
    protected void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }
}
