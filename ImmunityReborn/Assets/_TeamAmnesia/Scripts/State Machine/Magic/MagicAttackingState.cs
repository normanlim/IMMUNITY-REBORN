using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackingState : MagicBaseState
{
    public MagicAttackingState(MagicStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        //Debug.Log( "In attacking state" );

        MoveToPlayer( deltaTime );

        FacePlayer( deltaTime );
    }

    public override void Exit()
    {
    }
}
