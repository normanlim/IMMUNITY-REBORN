using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public abstract class DragonBaseState : State
{
    protected DragonStateMachine stateMachine;
    protected const float FlyingMaxYOffset = 2.0f;
    protected const float FlyUpRate = 0.02f;
    protected const float FlyingMinDistanceToPlayer = 7.0f;
    protected const float FlyingMaxDistanceToPlayer = 10.0f;
    protected const float GroundedDistanceToPlayer = 5.0f;

    private readonly int AnimatorMoveXParam = Animator.StringToHash("MoveX");
    private readonly int AnimatorMoveYParam = Animator.StringToHash("MoveY");
    private readonly LayerMask EnvironmentLayer = LayerMask.NameToLayer("Environment");
    private const float AnimatorDampTime = 0.1f;
    private const float TurnSpeed = 15.0f;

    protected float currentYOffset = 0.0f;
    protected float switchAttackTimer;

    public DragonBaseState(DragonStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        switchAttackTimer = stateMachine.SwitchAttackDelay;
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.CharacterController.Move((movement + stateMachine.FlyingForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
            Vector3 directionToPlayer = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
            float epsilon = 2.0f;

            if (Mathf.Abs(distanceToPlayerSqr - GroundedDistanceToPlayer * GroundedDistanceToPlayer) > epsilon * epsilon)
            {
                Vector3 offset = directionToPlayer * GroundedDistanceToPlayer;
                stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position - offset;
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
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
            float epsilon = 2.0f;

            if (Mathf.Abs(distanceToPointSqr) > epsilon * epsilon)
            {
                stateMachine.NavMeshAgent.destination = point;
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
            }
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity; // needed to sync velocities
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position; // fixes bug where enemies float apart when colliding with each other
    }

    protected void FlyToPlayer(float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
            Vector3 directionToPlayer = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;

            if (distanceToPlayerSqr < FlyingMinDistanceToPlayer * FlyingMinDistanceToPlayer)
            {
                Vector3 minOffset = directionToPlayer * FlyingMinDistanceToPlayer;
                stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position - minOffset;
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
            }
            else if (distanceToPlayerSqr > FlyingMaxDistanceToPlayer * FlyingMaxDistanceToPlayer)
            {
                Vector3 maxOffset = directionToPlayer * FlyingMaxDistanceToPlayer;
                stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position - maxOffset;
                Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
            }
        }

        if (Physics.Raycast(stateMachine.transform.position, stateMachine.transform.TransformDirection(Vector3.down), out RaycastHit hitInfo, 50.0f, 1 << EnvironmentLayer))
        {
            Debug.DrawRay(stateMachine.transform.position, stateMachine.transform.TransformDirection(Vector3.down) * hitInfo.distance, Color.yellow);

            currentYOffset = hitInfo.distance;

            if (currentYOffset > FlyingMaxYOffset)
            {
                stateMachine.CharacterController.Move(new Vector3(0.0f, -(currentYOffset - FlyingMaxYOffset), 0.0f)); // prevents CharacterController from drifting up
            }
            else if (currentYOffset < FlyingMaxYOffset)
            {
                stateMachine.CharacterController.Move(new Vector3(0.0f, FlyUpRate, 0.0f)); // fly up gradually
            }
        }
        
        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity;
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position;
    }

    protected void LandOnPlayer(float deltaTime)
    {
        Vector3 playerXZPos = new(stateMachine.Player.transform.position.x, 0.0f, stateMachine.Player.transform.position.z);
        Vector3 dragonXZPos = new(stateMachine.transform.position.x, 0.0f, stateMachine.transform.position.z);
        Vector3 offsetXZ = (playerXZPos - dragonXZPos).normalized * GroundedDistanceToPlayer;
        Vector3 targetPosition = playerXZPos - offsetXZ;

        stateMachine.NavMeshAgent.destination = targetPosition;
        Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

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

    protected void UpdateGroundedAnimator(float deltaTime)
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

    protected bool RandomPointAroundPlayer(float range, out Vector3 result)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = stateMachine.Player.transform.position + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    protected int RollDie(int min, int max)
    {
        return Random.Range(min, max);
    }
}