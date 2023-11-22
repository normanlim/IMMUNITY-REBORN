using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class DragonBaseState : State
{
    protected DragonStateMachine stateMachine;
    protected const float FlyingMaxYOffset = 2.0f;
    protected const float FlyUpRate = 0.02f;
    protected const float FlyingMinDistanceToPlayer = 6.0f;
    protected const float FlyingMaxDistanceToPlayer = 8.0f;
    protected const float GroundedDistanceToPlayer = 7.0f;

    private readonly int AnimatorMoveXParam = Animator.StringToHash("MoveX");
    private readonly int AnimatorMoveYParam = Animator.StringToHash("MoveY");
    private const float AnimatorDampTime = 0.1f;
    private const float TurnSpeed = 15.0f;

    protected float currentYOffset = 0.0f;

    public DragonBaseState(DragonStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.CharacterController.Move((movement + stateMachine.FlyingForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void MoveToPlayer(float speed, float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * speed, deltaTime);
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity; // needed to sync velocities
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position; // fixes bug where enemies float apart when colliding with each other
    }

    protected void MoveToPlayerOffset(float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
            Vector3 directionToPlayer = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
            float epsilon = 0.5f;

            if (Mathf.Abs(distanceToPlayerSqr - GroundedDistanceToPlayer * GroundedDistanceToPlayer) > epsilon * epsilon)
            {
                Vector3 offset = directionToPlayer * GroundedDistanceToPlayer;
                stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position - offset;
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.GroundedSpeed, deltaTime);
            }
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity; // needed to sync velocities
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position; // fixes bug where enemies float apart when colliding with each other
    }

    protected void MoveToPoint(Vector3 point, float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            float distanceToPointSqr = (point - stateMachine.transform.position).sqrMagnitude;
            float epsilon = 0.5f;

            if (Mathf.Abs(distanceToPointSqr) > epsilon * epsilon)
            {
                stateMachine.NavMeshAgent.destination = point;
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.GroundedSpeed, deltaTime);
            }
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity; // needed to sync velocities
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position; // fixes bug where enemies float apart when colliding with each other
    }

    protected void FlyToPlayer(float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            // uses horizontal axes to determine distance from player
            Vector3 playerXZPos = new(stateMachine.Player.transform.position.x, 0.0f, stateMachine.Player.transform.position.z);
            Vector3 dragonXZPos = new(stateMachine.transform.position.x, 0.0f, stateMachine.transform.position.z);
            float distanceToPlayerXZSqr = (playerXZPos - dragonXZPos).sqrMagnitude;
            Vector3 directionToPlayerXZ = (playerXZPos - dragonXZPos).normalized;

            if (distanceToPlayerXZSqr < FlyingMinDistanceToPlayer * FlyingMinDistanceToPlayer) // if too close to player
            {
                Vector3 minOffset = directionToPlayerXZ * FlyingMinDistanceToPlayer;
                stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position - minOffset;
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.FlyingSpeed, deltaTime); // move towards 'far enough' position
            }
            else if (distanceToPlayerXZSqr > FlyingMaxDistanceToPlayer * FlyingMaxDistanceToPlayer) // if too far from player
            {
                Vector3 maxOffset = directionToPlayerXZ * FlyingMaxDistanceToPlayer;
                stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position - maxOffset;
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.FlyingSpeed, deltaTime); // move towards 'close enough' position
            }
        }

        if (stateMachine.RaycastToGround(out RaycastHit hit))
        {
            currentYOffset = hit.distance;

            if (currentYOffset > FlyingMaxYOffset) // if too high up
            {
                stateMachine.CharacterController.Move(new Vector3(0.0f, -(currentYOffset - FlyingMaxYOffset), 0.0f)); // prevents CharacterController from drifting up
            }
            else if (currentYOffset < FlyingMaxYOffset / 2) // if less than halfway up
            {
                stateMachine.CharacterController.Move(new Vector3(0.0f, FlyUpRate / 4, 0.0f)); // fly up slowly
            }
            else if (currentYOffset < FlyingMaxYOffset) // if between halfway up and max offset
            {
                stateMachine.CharacterController.Move(new Vector3(0.0f, FlyUpRate, 0.0f)); // fly up normally
            }
        }
        
        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity;
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position;
    }

    protected void FacePlayer(float deltaTime)
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0.0f;

        stateMachine.transform.rotation = Quaternion.Slerp(
            stateMachine.transform.rotation, Quaternion.LookRotation(lookPos), TurnSpeed * deltaTime);
    }

    protected bool IsInCombatRange()
    {
        if (stateMachine.PlayerHealth.CurrentHealth == 0) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position -
            stateMachine.transform.position).sqrMagnitude; // more performant than Vector3.magnitude which uses sqrt

        return distanceToPlayerSqr <= stateMachine.CombatRange * stateMachine.CombatRange;
    }

    protected bool IsInClawAttackRange()
    {
        if (stateMachine.PlayerHealth.CurrentHealth == 0) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position -
            stateMachine.transform.position).sqrMagnitude; // more performant than Vector3.magnitude which uses sqrt

        return distanceToPlayerSqr <= stateMachine.ClawAttackRange * stateMachine.ClawAttackRange;
    }

    protected void UpdateGroundedAnimator(float deltaTime, float maxSpeed = 1.0f)
    {
        if (Mathf.Approximately(stateMachine.NavMeshAgent.velocity.y, 0.0f))
        {
            stateMachine.Animator.SetFloat(AnimatorMoveYParam, 0.0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            float value = stateMachine.NavMeshAgent.velocity.y > 0.0f ? maxSpeed : -maxSpeed;
            stateMachine.Animator.SetFloat(AnimatorMoveYParam, value, AnimatorDampTime, deltaTime);
        }

        if (Mathf.Approximately(stateMachine.NavMeshAgent.velocity.x, 0.0f))
        {
            stateMachine.Animator.SetFloat(AnimatorMoveXParam, 0.0f, AnimatorDampTime, deltaTime);
        }
        else
        {
            float value = stateMachine.NavMeshAgent.velocity.x > 0.0f ? maxSpeed : -maxSpeed;
            stateMachine.Animator.SetFloat(AnimatorMoveXParam, value, AnimatorDampTime, deltaTime);
        }
    }

    protected int RollDie(int minInclusive, int maxExclusive)
    {
        return Random.Range(minInclusive, maxExclusive);
    }
}