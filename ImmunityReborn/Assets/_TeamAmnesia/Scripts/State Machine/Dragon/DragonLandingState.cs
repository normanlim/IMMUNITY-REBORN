using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLandingState : DragonBaseState
{
    private readonly int LandingStateName = Animator.StringToHash("GlideToLanding");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");

    private const float TransitionDuration = 0.1f;
    private bool isFeetOnGround;

    public DragonLandingState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.DragonActions.ResetGroundedActionWeights();

        stateMachine.FlyingForceReceiver.IsFlying = false;

        stateMachine.LandingWeaponDamager.SetDamage(stateMachine.LandingDamage, stateMachine.LandingKnockback);

        stateMachine.Animator.CrossFadeInFixedTime(LandingStateName, TransitionDuration, 0);

        PlaySFX.PlayThenDestroy(stateMachine.SFXLanding, stateMachine.transform);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        LandOnPlayer(deltaTime);

        if (!isFeetOnGround && GetPlayingAnimationTimeNormalized(stateMachine.Animator, 0) >= 0.8f)
        {
            isFeetOnGround = true;
        }

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

    protected void LandOnPlayer(float deltaTime)
    {
        if (isFeetOnGround)
        {
            Move(deltaTime);
        }
        else
        {
            Vector3 playerXZPos = new(stateMachine.Player.transform.position.x, 0.0f, stateMachine.Player.transform.position.z);
            Vector3 dragonXZPos = new(stateMachine.transform.position.x, 0.0f, stateMachine.transform.position.z);
            Vector3 offsetXZ = (playerXZPos - dragonXZPos).normalized * (GroundedDistanceToPlayer * 0); // currently no offset
            Vector3 targetPosition = playerXZPos - offsetXZ;

            stateMachine.NavMeshAgent.destination = targetPosition;
            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.FlyingSpeed, deltaTime);
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity;
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position;
    }
}
