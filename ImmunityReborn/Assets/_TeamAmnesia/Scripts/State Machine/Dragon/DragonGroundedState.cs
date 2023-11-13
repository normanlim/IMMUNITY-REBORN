using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGroundedState : DragonBaseState
{
    private readonly int GroundedStateName = Animator.StringToHash("Grounded");

    private const float TransitionDuration = 0.1f;
    private const float PositionQueryDelay = 2.0f;

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

        UpdateGroundedAnimator(deltaTime);

        nextAttackTimer -= deltaTime;
        if (nextAttackTimer <= 0.0f)
        {
            if (RollDie(0, 2 - stateMachine.AttacksCounter[DragonAttackType.Clawing]) == 0) // after number of clawing attacks, guaranteed to change attack
            {
                stateMachine.AttacksCounter[DragonAttackType.Clawing] = 0;
                stateMachine.SwitchState(new DragonFlyingState(stateMachine));
                return;
            }
            else
            {
                stateMachine.AttacksCounter[DragonAttackType.Clawing]++;
                stateMachine.NextAttackType = DragonAttackType.Clawing;
            }
            stateMachine.SwitchState(DragonAttack.CreateNextState(stateMachine));
        }
    }

    private void MoveAroundRandomly(float deltaTime)
    {
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
            MoveToPlayerOffset(deltaTime);
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
