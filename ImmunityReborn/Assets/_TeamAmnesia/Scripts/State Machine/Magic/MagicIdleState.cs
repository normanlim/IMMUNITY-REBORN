using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicIdleState : MagicBaseState
{

    private readonly int IdleStateAnimation = Animator.StringToHash( "idle" );
    private const float CrossFadeDuration = 0.1f;

    public MagicIdleState(MagicStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime( IdleStateAnimation, CrossFadeDuration );
    }

    public override void Tick(float deltaTime)
    {
        if ( IsInChaseRange() )
        {
            stateMachine.SwitchState( new MagicChasingState( stateMachine ) );
            return;
        }

        FacePlayer( deltaTime );

        Move( deltaTime );

        UpdateLocomotionAnimator( deltaTime );
    }

    public override void Exit()
    {
    }
}
