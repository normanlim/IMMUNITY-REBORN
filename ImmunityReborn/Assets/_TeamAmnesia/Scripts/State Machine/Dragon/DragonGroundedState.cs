using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DragonGroundedState : DragonBaseState
{
    private readonly int GroundedStateName = Animator.StringToHash("Grounded");

    private const float TransitionDuration = 0.1f;
    private const float PositionQueryDelay = 3.0f;

    private float currentQueryDelay = PositionQueryDelay;
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

        currentQueryDelay -= deltaTime;

        if (currentQueryDelay <= 0.0f)
        {
            if (stateMachine.NavMeshSampler.RandomPointAroundPosition(stateMachine.Player.transform.position, 5.0f, out targetPos))
            {
                currentQueryDelay = PositionQueryDelay;
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
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
