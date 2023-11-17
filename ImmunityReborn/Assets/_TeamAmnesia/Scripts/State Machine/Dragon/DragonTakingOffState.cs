using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonTakingOffState : DragonBaseState
{
    private readonly int TakeOffStateName = Animator.StringToHash("TakeOffToGlide");

    private const float CrossFadeDuration = 0.1f;

    public DragonTakingOffState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.DragonActions.ResetFlyingActionWeights();

        stateMachine.FlyingForceReceiver.IsFlying = true;

        stateMachine.Animator.CrossFadeInFixedTime(TakeOffStateName, CrossFadeDuration, 0);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        FlyToPlayer(deltaTime);

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 0) >= 0.5f) // only need first half of the animation
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