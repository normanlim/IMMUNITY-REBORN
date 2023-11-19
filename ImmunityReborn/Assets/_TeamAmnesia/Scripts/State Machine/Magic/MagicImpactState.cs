using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicImpactState : MagicBaseState
{

    private readonly int ImpactStateName = Animator.StringToHash( "damage" );
    private const float CrossFadeDuration = 0.1f;
    private float duration = 0.3f;

    public MagicImpactState(MagicStateMachine stateMachine) : base(stateMachine)
    {
        stateMachine.Animator.CrossFadeInFixedTime( ImpactStateName, CrossFadeDuration );
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime( ImpactStateName, CrossFadeDuration );
    }

    public override void Tick(float deltaTime)
    {
        Move( deltaTime ); // applies ForceReceiver's movement

        UpdateLocomotionAnimator( deltaTime );

        // Enemy will stay in this state until countdown is finished
        duration -= deltaTime;

        if ( duration <= 0.0f )
        {
            stateMachine.SwitchState( new MagicIdleState( stateMachine ) );
        }
    }

    public override void Exit()
    {
    }
}
