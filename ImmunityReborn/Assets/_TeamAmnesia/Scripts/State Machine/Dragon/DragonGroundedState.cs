using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGroundedState : DragonBaseState
{
    private readonly int GroundedStateName = Animator.StringToHash("Grounded");

    private const float TransitionDuration = 0.1f;
    private const float PositionQueryDelay = 4.0f;
    private const float MovementRadius = 10.0f;

    private float currentQueryTimer; // query immediately upon entering state
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

        MoveAroundRandomly(deltaTime);

        UpdateGroundedAnimator(deltaTime, 0.8f);

        stateMachine.DragonActions.Tick(stateMachine, false, deltaTime);
    }

    private void MoveAroundRandomly(float deltaTime)
    {
        currentQueryTimer -= deltaTime;
        if (currentQueryTimer <= 0.0f)
        {
            if (stateMachine.NavMeshSampler.RandomPointAroundPosition(
                stateMachine.SampleAroundPoint.position, MovementRadius, GroundedDistanceToPlayer, out targetPos))
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
            MoveToPlayerOffset(deltaTime);
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
