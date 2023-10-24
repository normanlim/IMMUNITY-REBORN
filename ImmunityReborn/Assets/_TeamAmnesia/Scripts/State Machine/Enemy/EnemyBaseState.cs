using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    private readonly int AnimatorSpeedParam = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;

    public EnemyBaseState(EnemyStateMachine stateMachine)
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
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0.0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected bool IsInChaseRange()
    {
        float distanceToPlayerSqr = (stateMachine.Player.transform.position -
            stateMachine.transform.position).sqrMagnitude; // more performant than Vector3.magnitude which uses sqrt

        return distanceToPlayerSqr <= stateMachine.ChaseRange * stateMachine.ChaseRange;
    }

    protected bool IsInAttackRange()
    {
        float distanceToPlayerSqr = (stateMachine.Player.transform.position -
            stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    protected void UpdateAnimator(float deltaTime)
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
}
