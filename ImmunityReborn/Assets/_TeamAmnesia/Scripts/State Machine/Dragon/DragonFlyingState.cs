using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlyingState : DragonBaseState
{
    private readonly int TakeOffStateName = Animator.StringToHash("TakeOffToGlide");
    private readonly int FlyStateName = Animator.StringToHash("FlyStationary");

    private const float CrossFadeDuration = 0.1f;

    public DragonFlyingState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (!stateMachine.FlyingForceReceiver.IsFlying)
        {
            stateMachine.Animator.CrossFadeInFixedTime(TakeOffStateName, CrossFadeDuration, 0);
        }

        stateMachine.FlyingForceReceiver.IsFlying = true;
    }

    public override void Tick(float deltaTime)
    {
        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 0) >= 0.5f)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FlyStateName, 0.5f);
        }

        FacePlayer(deltaTime);

        FlyToPlayer(deltaTime);

        nextAttackTimer -= deltaTime;
        if (nextAttackTimer <= 0.0f)
        {
            if (RollDie(0, 4 - stateMachine.AttacksCounter[DragonAttackType.Fireball]) == 0) // after number of fireball attacks, guaranteed to change attack
            {
                stateMachine.AttacksCounter[DragonAttackType.Fireball] = 0;
                stateMachine.NextAttackType = DragonAttackType.Landing;
            }
            else
            {
                stateMachine.AttacksCounter[DragonAttackType.Fireball]++;
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