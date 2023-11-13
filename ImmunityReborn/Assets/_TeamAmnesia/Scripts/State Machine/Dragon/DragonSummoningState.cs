using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSummoningState : DragonBaseState
{
    private readonly int SummonStateName = Animator.StringToHash("FlyStationarySpreadFire");

    private const float TransitionDuration = 0.1f;

    public DragonSummoningState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(SummonStateName, TransitionDuration, 0);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        FlyToPlayer(deltaTime);

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 0) >= 1.0f)
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
