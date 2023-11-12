using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlyingState : DragonBaseState
{
    private readonly int FlyStateName = Animator.StringToHash("FlyStationary");

    private const float CrossFadeDuration = 0.1f;

    private float timeUntilFireball = 3.0f;

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

        if (stateMachine.SwitchAttack)
        {
            switchAttackTimer -= deltaTime;

            if (switchAttackTimer <= 0.0f)
            {
                stateMachine.SwitchState(new DragonLandingState(stateMachine));
            }
        }
        else
        {
            timeUntilFireball -= deltaTime;

            if (timeUntilFireball <= 0.0f)
            {
                stateMachine.SwitchState(new DragonFireballState(stateMachine));
            }
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}