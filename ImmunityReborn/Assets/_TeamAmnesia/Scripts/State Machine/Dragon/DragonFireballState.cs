using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireballState : DragonBaseState
{
    private readonly int FireballStateName = Animator.StringToHash("FlyStationarySpitFireBall");

    private const float TransitionDuration = 0.1f;

    public DragonFireballState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FireballStateName, TransitionDuration, 0);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        FlyToPlayer(deltaTime);

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 0) >= 1.0f)
        {
            if (RollDie(0, 1) == 0)
            {
                stateMachine.SwitchAttack = true;
            }

            stateMachine.SwitchState(new DragonFlyingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
