using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AnimatorAttackStateName = Animator.StringToHash("Attack1SwordShield");

    private const float TransitionDuration = 0.1f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.WeaponDamager.SetDamage(stateMachine.AttackDamage);

        stateMachine.Animator.CrossFadeInFixedTime(AnimatorAttackStateName, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator) >= 1.0f)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}
