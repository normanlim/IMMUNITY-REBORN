using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFirebreathState : DragonBaseState
{
    private readonly int FlyingFirebreathStateName = Animator.StringToHash("FlyStationarySpreadFire");
    private readonly int GroundedFirebreathStateName = Animator.StringToHash("SpreadFire");

    private const float TransitionDuration = 0.1f;
    private const float MinFlyingDistanceToPerformFirebreath = 10.0f;
    private const float MinGroundedDistanceToPerformFirebreath = 6.0f;

    public DragonFirebreathState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        if (stateMachine.FlyingForceReceiver.IsFlying)
        {
            /* Doesn't work well currently */
            //if (distanceToPlayerSqr < MinFlyingDistanceToPerformFirebreath * MinFlyingDistanceToPerformFirebreath)
            //{
            //    stateMachine.DragonActions.SetTimer(0.0f);
            //    stateMachine.SwitchState(new DragonFlyingState(stateMachine));
            //    return;
            //}

            stateMachine.Animator.CrossFadeInFixedTime(FlyingFirebreathStateName, TransitionDuration, 0);
            PlaySFX.PlayThenDestroy(stateMachine.SFXSummoning, stateMachine.transform);
        }
        else
        {
            if (distanceToPlayerSqr < MinGroundedDistanceToPerformFirebreath * MinGroundedDistanceToPerformFirebreath) // if too close skip firebreath state
            {
                stateMachine.DragonActions.SetTimer(0.0f);
                stateMachine.SwitchState(new DragonGroundedState(stateMachine));
                return;
            }

            stateMachine.Animator.CrossFadeInFixedTime(GroundedFirebreathStateName, TransitionDuration, 0);
            PlaySFX.PlayThenDestroy(stateMachine.SFXSummoning, stateMachine.transform);
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

        if (stateMachine.FirebreathSpawnPoint.childCount > 0 && stateMachine.FirebreathSpawnPoint.GetChild(0) is { } fireBreath)
        {
            stateMachine.DestroyGameObject(fireBreath.gameObject);
        }
    }
}
