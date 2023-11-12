using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackingState : MagicBaseState
{

    private readonly int AttackStateAnimation = Animator.StringToHash( "attack01" );
    private const float CrossFadeDuration = 0.1f;

    public MagicAttackingState(MagicStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime( AttackStateAnimation, CrossFadeDuration );
        stateMachine.DOTPuddle.SpawnPuddleOnFloor();
    }



    public override void Tick(float deltaTime)
    {
        //Debug.Log( "In attacking state" );

        //MoveToPlayer( deltaTime );

        //FacePlayer( deltaTime );
    }

    public override void Exit()
    {
    }
}
