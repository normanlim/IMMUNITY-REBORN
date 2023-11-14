using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireballState : DragonBaseState
{
    private readonly int FlyingFireballStateName = Animator.StringToHash("FlyStationarySpitFireBall");
    private readonly int GroundedFireballStateName = Animator.StringToHash("SpitFireBall");

    private const float TransitionDuration = 0.1f;

    public DragonFireballState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (stateMachine.FlyingForceReceiver.IsFlying)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FlyingFireballStateName, TransitionDuration, 0);
        }
        else
        {
            stateMachine.Animator.CrossFadeInFixedTime(GroundedFireballStateName, TransitionDuration, 0);
        }
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        Move(deltaTime);

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 0) >= 1.0f)
        {
            if (stateMachine.FlyingForceReceiver.IsFlying)
            {
                stateMachine.SwitchState(new DragonFlyingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new DragonGroundedState(stateMachine));
            }
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
