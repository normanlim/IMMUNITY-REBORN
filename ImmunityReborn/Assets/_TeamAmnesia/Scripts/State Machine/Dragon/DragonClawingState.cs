using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonClawingState : DragonBaseState
{
    private readonly int RightClawAttackStateName = Animator.StringToHash("ClawsAttackRightForward");
    private readonly int LeftClawAttackStateName = Animator.StringToHash("ClawsAttackLeftForward");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");

    private const float TransitionDuration = 0.1f;

    private bool isClawing;
    private int clawedCount;
    private int previousClawAttack;

    public DragonClawingState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.RightClawWeaponDamager.SetDamage(stateMachine.ClawAttackDamage, stateMachine.ClawAttackKnockback);
        stateMachine.LeftClawWeaponDamager.SetDamage(stateMachine.ClawAttackDamage, stateMachine.ClawAttackKnockback);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        if (!isClawing)
        {
            MoveToPlayer(stateMachine.GroundedSpeed * 2, deltaTime);
            UpdateGroundedAnimator(deltaTime);
        }
        else
        {
            MoveToPlayer(stateMachine.GroundedSpeed / 2, deltaTime);
            UpdateGroundedAnimator(deltaTime, 0.8f);
        }

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 1) >= 1.0f)
        {
            isClawing = false;
            stateMachine.Animator.CrossFade(EmptyDefaultStateName, TransitionDuration, 1);

            if (clawedCount == 2)
            {
                stateMachine.SwitchState(new DragonGroundedState(stateMachine));
                return;
            }
        }

        if (!isClawing && IsInClawAttackRange())
        {
            Claw();
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;

        stateMachine.Animator.CrossFade(EmptyDefaultStateName, TransitionDuration, 1);
    }

    /// <summary>
    /// Alternate between right and left claw attacks. First attack is random.
    /// </summary>
    private void Claw()
    {
        isClawing = true;
        clawedCount++;

        if (previousClawAttack == RightClawAttackStateName)
        {
            stateMachine.Animator.CrossFadeInFixedTime(LeftClawAttackStateName, TransitionDuration, 1);
            previousClawAttack = LeftClawAttackStateName;
        }
        else if (previousClawAttack == LeftClawAttackStateName)
        {
            stateMachine.Animator.CrossFadeInFixedTime(RightClawAttackStateName, TransitionDuration, 1);
            previousClawAttack = RightClawAttackStateName;
        }
        else
        {
            if (RollDie(0, 2) == 0)
            {
                stateMachine.Animator.CrossFadeInFixedTime(RightClawAttackStateName, TransitionDuration, 1);
                previousClawAttack = RightClawAttackStateName;
            }
            else
            {
                stateMachine.Animator.CrossFadeInFixedTime(LeftClawAttackStateName, TransitionDuration, 1);
                previousClawAttack = LeftClawAttackStateName;
            }
        }

        PlaySFX.PlayThenDestroy(stateMachine.SFXClawing, stateMachine.transform);
    }
}
