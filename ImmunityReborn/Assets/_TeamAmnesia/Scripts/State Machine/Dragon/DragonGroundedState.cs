using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGroundedState : DragonBaseState
{
    private readonly int GroundedStateName = Animator.StringToHash("Grounded");

    private const float TransitionDuration = 0.1f;
    private const float GroundedStateDuration = 5.0f;
    private const float PositionQueryDelay = 2.0f;

    private float currentQueryTimer = PositionQueryDelay;
    private float currentGroundedTimer = GroundedStateDuration;
    private Vector3 targetPos;

    public DragonGroundedState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.FlyingForceReceiver.IsFlying = false;

        stateMachine.Animator.CrossFadeInFixedTime(GroundedStateName, TransitionDuration, 0);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        currentQueryTimer -= deltaTime;
        if (currentQueryTimer <= 0.0f)
        {
            if (stateMachine.NavMeshSampler.RandomPointAroundPosition(
                stateMachine.Player.transform.position, GroundedDistanceToPlayer * 2, GroundedDistanceToPlayer, out targetPos))
            {
                currentQueryTimer = PositionQueryDelay;
            }
        }

        if (targetPos != Vector3.zero)
        {
            MoveToPoint(targetPos, deltaTime);
        }
        else
        {
            MoveToPlayer(deltaTime);
        }

        UpdateGroundedAnimator(deltaTime);

        currentGroundedTimer -= deltaTime;
        if (currentGroundedTimer <= 0.0f)
        {
            stateMachine.SwitchState(new DragonFlyingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
