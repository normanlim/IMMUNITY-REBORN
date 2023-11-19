using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
        stateMachine.WeaponDamager.SetDamage( stateMachine.AttackDamage, stateMachine.AttackKnockback );
        stateMachine.Animator.CrossFadeInFixedTime( AttackStateAnimation, CrossFadeDuration );
        stateMachine.DOTPuddle.SpawnPuddleOnFloor();
    }

    public override void Tick(float deltaTime)
    {
        // The end part of the animation is very empty without much particles,
        // but still does damage since collider is still there. Might be a problem for player
        if ( GetPlayingAnimationTimeNormalized( stateMachine.Animator, 0 ) >= 1.0f ) // animation is done playing
        {

            stateMachine.Health.DealDamage( 100 ); // self destruct, normal bombers should auto die

            if ( stateMachine.Health.CurrentHealth > 0 )
            {
                if ( IsInChaseRange() )
                {
                    stateMachine.SwitchState( new MagicChasingState( stateMachine ) );
                    return;
                }
                else
                {
                    stateMachine.SwitchState( new MagicIdleState( stateMachine ) );
                    return;
                }
            }
            else
            {
                stateMachine.SwitchState( new MagicDeadState( stateMachine ) );
            }
        }

    }

    public override void Exit()
    {
    }
}
