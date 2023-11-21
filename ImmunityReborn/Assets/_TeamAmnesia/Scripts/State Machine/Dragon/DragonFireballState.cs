using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireballState : DragonBaseState
{
    private readonly int FlyingFireballStateName = Animator.StringToHash("FlyStationarySpitFireBall");
    private readonly int GroundedFireballStateName = Animator.StringToHash("SpitFireBall");

    private const float TransitionDuration = 0.1f;
    private const float MinGroundedDistanceToPerformFireball = 6.0f;

    public DragonFireballState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (stateMachine.FlyingForceReceiver.IsFlying)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FlyingFireballStateName, TransitionDuration, 0);
            PlaySFX.PlayThenDestroy(stateMachine.SFXFireball, stateMachine.transform);
        }
        else
        {
            float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

            if (distanceToPlayerSqr < MinGroundedDistanceToPerformFireball * MinGroundedDistanceToPerformFireball) // if too close skip fireball state
            {
                stateMachine.DragonActions.SetTimer(0.0f);
                stateMachine.SwitchState(new DragonGroundedState(stateMachine));
                return;
            }

            stateMachine.Animator.CrossFadeInFixedTime(GroundedFireballStateName, TransitionDuration, 0);
            PlaySFX.PlayThenDestroy(stateMachine.SFXFireball, stateMachine.transform);
        }
        
        stateMachine.MeshRenderer.material = stateMachine.Materials[1];
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
