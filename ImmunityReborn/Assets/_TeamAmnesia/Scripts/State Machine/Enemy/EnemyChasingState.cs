using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionStateName = Animator.StringToHash("Locomotion");
    private readonly int AnimatorSpeedParam = Animator.StringToHash("Speed");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionStateName, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);

        FacePlayer();

        stateMachine.Animator.SetFloat(AnimatorSpeedParam, 1.0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position;

        Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity; // needed to sync velocities
    }
}
