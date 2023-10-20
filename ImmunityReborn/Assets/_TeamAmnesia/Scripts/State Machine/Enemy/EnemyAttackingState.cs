using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AnimatorAttackStateName = Animator.StringToHash("Attack2SwordShield");
    private readonly int DefaultEmptyStateName = Animator.StringToHash("Default Empty");

    private const float TransitionDuration = 0.1f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.WeaponDamager.SetDamage(stateMachine.AttackDamage);
        stateMachine.Animator.CrossFadeInFixedTime(AnimatorAttackStateName, TransitionDuration, 1);
    }

    public override void Tick(float deltaTime)
    {
        MoveToPlayer(deltaTime);

        FacePlayer();

        UpdateAnimator(deltaTime);

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 1) >= 1.0f)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.CrossFade(DefaultEmptyStateName, TransitionDuration, 1); // without this, gets stuck at end of attack animation
    }
}
