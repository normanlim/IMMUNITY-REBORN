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
        stateMachine.WeaponDamager.SetDamage(stateMachine.AttackDamage, stateMachine.AttackKnockback);
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

    protected void LandOnPlayer(float deltaTime)
    {
        Vector3 playerXZPos = new(stateMachine.Player.transform.position.x, 0.0f, stateMachine.Player.transform.position.z);
        Vector3 dragonXZPos = new(stateMachine.transform.position.x, 0.0f, stateMachine.transform.position.z);
        Vector3 offsetXZ = (playerXZPos - dragonXZPos).normalized * (GroundedDistanceToPlayer / 2);
        Vector3 targetPosition = playerXZPos - offsetXZ;

        stateMachine.NavMeshAgent.destination = targetPosition;
        Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity;
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position;
    }
}
