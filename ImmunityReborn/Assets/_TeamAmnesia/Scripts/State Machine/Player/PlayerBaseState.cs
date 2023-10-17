using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    private readonly int AnimatorInputXParam = Animator.StringToHash("InputX");
    private readonly int AnimatorInputYParam = Animator.StringToHash("InputY");

    private const float AnimatorDampTime = 0.1f;
    private const float TurnSpeed = 15;

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

    /// <summary>
    /// Face the direction the follow camera is facing
    /// </summary>
    /// <param name="deltaTime"></param>
    protected void FaceCameraDirection(float deltaTime)
    {
        // get camera's y-axis rotational value and gradually rotate player's transform towards the same y-axis value
        float yawCamera = stateMachine.MainCameraTransform.rotation.eulerAngles.y;
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.Euler(0, yawCamera, 0), TurnSpeed * deltaTime);
    }

    /// <summary>
    /// Sets animator parameters
    /// </summary>
    /// <param name="deltaTime"></param>
    protected void UpdateAnimator(float deltaTime)
    { 
        // Sets blend tree value(s) to 1 if any movement at all because when moving diagonally, x and y inputs are roughly 0.7.
        // If we use this value for the blend tree, the animation will not play at normal speed.
        if (Mathf.Approximately(stateMachine.InputReader.MovementValue.y, 0.0f))
        {
            stateMachine.Animator.SetFloat(AnimatorInputYParam, 0.0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.y > 0.0f ? 1.0f : -1.0f;
            stateMachine.Animator.SetFloat(AnimatorInputYParam, value, AnimatorDampTime, deltaTime);
        }

        if (Mathf.Approximately(stateMachine.InputReader.MovementValue.x, 0.0f))
        {
            stateMachine.Animator.SetFloat(AnimatorInputXParam, 0.0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0.0f ? 1.0f : -1.0f;
            stateMachine.Animator.SetFloat(AnimatorInputXParam, value, AnimatorDampTime, deltaTime);
        }
    }
}
