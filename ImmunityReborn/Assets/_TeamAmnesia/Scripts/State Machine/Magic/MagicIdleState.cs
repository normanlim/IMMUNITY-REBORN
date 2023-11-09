using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicIdleState : MagicBaseState
{
    public MagicIdleState(MagicStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        //Debug.Log( "Inatk range: "+IsInAttackRange() );
        if ( IsInChaseRange() )
        {
            Debug.Log( "In range - chasing player" );
            stateMachine.SwitchState( new MagicChasingState( stateMachine ) );
            return;
        }

        FacePlayer( deltaTime );

        //UpdateLocomotionAnimator( deltaTime );
    }

    public override void Exit()
    {
    }
}
