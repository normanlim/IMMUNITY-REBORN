using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLandingState : DragonBaseState
{
    private readonly int LandingStateName = Animator.StringToHash("GlideToLanding");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");

    private const float TransitionDuration = 0.1f;

    public DragonLandingState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.FlyingForceReceiver.IsFlying = false;

        stateMachine.Animator.CrossFadeInFixedTime(LandingStateName, TransitionDuration, 0);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        LandOnPlayer(deltaTime);

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 0) >= 1.0f)
        {
            stateMachine.SwitchState(new DragonGroundedState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;

        stateMachine.Animator.CrossFade(EmptyDefaultStateName, TransitionDuration, 0);
    }
}
