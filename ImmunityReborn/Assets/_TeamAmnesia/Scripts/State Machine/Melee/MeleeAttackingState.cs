using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackingState : MeleeBaseState
{
    private readonly int AttackStateName = Animator.StringToHash("Attack2SwordShield");
    private readonly int AttackFollowUpStateName = Animator.StringToHash("Attack3SwordShield");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");

    private const float TransitionDuration = 0.1f;

    private bool alreadyDidFollowUpAttack;

    public MeleeAttackingState(MeleeStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.WeaponDamager.SetDamage(stateMachine.AttackDamage, stateMachine.AttackKnockback);
        stateMachine.Animator.CrossFadeInFixedTime(AttackStateName, TransitionDuration, 1);
    }

    public override void Tick(float deltaTime)
    {
        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 1) >= 1.0f)
        {
            stateMachine.Animator.CrossFade(EmptyDefaultStateName, TransitionDuration, 1);

            if (stateMachine.IsBoss)
            {
                if (alreadyDidFollowUpAttack)
                {
                    stateMachine.SwitchState(new MeleeRetreatingState(stateMachine));
                    return;
                }
                else
                {
                    stateMachine.Animator.CrossFadeInFixedTime(AttackFollowUpStateName, TransitionDuration, 1);
                    alreadyDidFollowUpAttack = true;
                }
            }
            else
            {
                stateMachine.SwitchState(new MeleeRetreatingState(stateMachine));
                return;
            }
        }

        MoveToPlayerOffset(deltaTime, stateMachine.AttackRange);

        FacePlayer(deltaTime);

        UpdateCirculatingAnimator(deltaTime);
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;

        stateMachine.Animator.CrossFade(EmptyDefaultStateName, TransitionDuration, 1); // without this, gets stuck at end of attack animation
    }
}
