using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonDeadState : DragonBaseState
{
    private readonly int FlyingDeathStateName = Animator.StringToHash("DeathHitTheGround");
    private readonly int GroundedDeathStateName = Animator.StringToHash("Death");

    private const float TransitionDuration = 0.1f;

    public DragonDeadState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (stateMachine.FlyingForceReceiver.IsFlying)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FlyingDeathStateName, TransitionDuration, 0);
        }
        else
        {
            stateMachine.Animator.CrossFadeInFixedTime(GroundedDeathStateName, TransitionDuration, 0);
        }
        PlaySFX.PlayThenDestroy(stateMachine.SFXDeath, stateMachine.transform);
        stateMachine.FlyingForceReceiver.IsFlying = false;

        stateMachine.LandingWeaponDamager.gameObject.SetActive(false);
        stateMachine.RightClawWeaponDamager.gameObject.SetActive(false);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
    }

    public override void Exit()
    {
    }
}