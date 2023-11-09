using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicChasingState : MagicBaseState
{
    public MagicChasingState(MagicStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        if ( IsInAttackRange() )
        {
            Debug.Log( "In range - atking player" );
            stateMachine.SwitchState( new MagicAttackingState( stateMachine ) );
            return;
        }

        MoveToPlayer( deltaTime );

        FacePlayer( deltaTime );

        //UpdateLocomotionAnimator( deltaTime );
    }

    public override void Exit()
    {
    }
}
