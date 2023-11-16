using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TormentedSoulBaseState : State
{
    protected TormentedSoulStateMachine stateMachine;

    private readonly int AnimatorSpeedParam = Animator.StringToHash("Speed");
    private readonly int AnimatorMoveXParam = Animator.StringToHash("MoveX");
    private readonly int AnimatorMoveYParam = Animator.StringToHash("MoveY");
    private const float AnimatorDampTime = 0.1f;
    private const float TurnSpeed = 15.0f;

    public TormentedSoulBaseState(TormentedSoulStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.CharacterController.Move((movement + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity; // needed to sync velocities
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position; // fixes bug where enemies float apart when colliding with each other
    }

    protected void FacePlayer(float deltaTime)
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0.0f;

        stateMachine.transform.rotation = Quaternion.Slerp(
            stateMachine.transform.rotation, Quaternion.LookRotation(lookPos), TurnSpeed * deltaTime);
    }

    protected bool IsInChaseRange()
    {
        if (stateMachine.PlayerHealth.CurrentHealth == 0) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position -
            stateMachine.transform.position).sqrMagnitude; // more performant than Vector3.magnitude which uses sqrt

        return distanceToPlayerSqr <= stateMachine.ChaseRange * stateMachine.ChaseRange;
    }

    protected bool IsInAttackRange()
    {
        if (stateMachine.PlayerHealth.CurrentHealth == 0) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position -
            stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    protected void UpdateLocomotionAnimator(float deltaTime)
    {
        if (stateMachine.NavMeshAgent.velocity != Vector3.zero)
        {
            stateMachine.Animator.SetFloat(AnimatorSpeedParam, 1.0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(AnimatorSpeedParam, 0.0f, AnimatorDampTime, deltaTime);
        }
    }

    protected void UpdateCirculatingAnimator(float deltaTime)
    {
        if (Mathf.Approximately(stateMachine.NavMeshAgent.velocity.y, 0.0f))
        {
            stateMachine.Animator.SetFloat(AnimatorMoveYParam, 0.0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            float value = stateMachine.NavMeshAgent.velocity.y > 0.0f ? 1.0f : -1.0f;
            stateMachine.Animator.SetFloat(AnimatorMoveYParam, value, AnimatorDampTime, deltaTime);
        }

        if (Mathf.Approximately(stateMachine.NavMeshAgent.velocity.x, 0.0f))
        {
            stateMachine.Animator.SetFloat(AnimatorMoveXParam, 0.0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            float value = stateMachine.NavMeshAgent.velocity.x > 0.0f ? 1.0f : -1.0f;
            stateMachine.Animator.SetFloat(AnimatorMoveXParam, value, AnimatorDampTime, deltaTime);
        }
    }
}
