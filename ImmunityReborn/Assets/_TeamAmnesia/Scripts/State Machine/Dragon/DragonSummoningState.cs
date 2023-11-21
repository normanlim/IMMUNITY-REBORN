using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSummoningState : DragonBaseState
{
    private readonly int FlyingSummonStateName = Animator.StringToHash("FlyStationarySpreadFire");
    private readonly int GroundedSummonStateName = Animator.StringToHash("SpreadFire");

    private const float TransitionDuration = 0.1f;

    public DragonSummoningState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (stateMachine.FlyingForceReceiver.IsFlying)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FlyingSummonStateName, TransitionDuration, 0);
        }
        else
        {
            stateMachine.Animator.CrossFadeInFixedTime(GroundedSummonStateName, TransitionDuration, 0);
        }

        stateMachine.MeshRenderer.material = stateMachine.Materials[2];

        PlaySFX.PlayThenDestroy(stateMachine.SFXSummoning, stateMachine.transform);
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

        stateMachine.MeshRenderer.material = stateMachine.Materials[0];
    }
}
