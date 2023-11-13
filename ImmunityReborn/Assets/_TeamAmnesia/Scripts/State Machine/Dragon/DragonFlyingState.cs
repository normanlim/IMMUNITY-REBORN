using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlyingState : DragonBaseState
{
    private readonly int FlyStateName = Animator.StringToHash("FlyStationary");

    private const float CrossFadeDuration = 0.1f;

    public DragonFlyingState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.FlyingForceReceiver.IsFlying = true;

        stateMachine.Animator.CrossFadeInFixedTime(FlyStateName, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        FlyToPlayer(deltaTime);

        nextAttackTimer -= deltaTime;
        if (nextAttackTimer <= 0.0f)
        {
            if (RollDie(0, 3) == 0)
            {
                stateMachine.NextAttackType = DragonAttackType.Landing;
            }
            else
            {
                stateMachine.NextAttackType = DragonAttackType.Fireball;
            }
            stateMachine.SwitchState(DragonAttack.CreateNextState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}