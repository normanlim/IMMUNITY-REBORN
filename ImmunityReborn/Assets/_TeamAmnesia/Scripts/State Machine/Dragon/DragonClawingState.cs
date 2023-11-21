using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonClawingState : DragonBaseState
{
    private readonly int ClawAttackStateName = Animator.StringToHash("ClawsAttackRightForward");

    private const float TransitionDuration = 0.1f;

    private bool isClawing;

    public DragonClawingState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ClawWeaponDamager.SetDamage(stateMachine.ClawAttackDamage, stateMachine.ClawAttackKnockback);
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

        if (!isClawing && IsInClawAttackRange())
        {
            isClawing = true;
            stateMachine.Animator.CrossFadeInFixedTime(ClawAttackStateName, TransitionDuration, 0);
            PlaySFX.PlayThenDestroy(stateMachine.SFXClawing, stateMachine.transform);
        }

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 0) >= 1.0f)
        {
            isClawing = false;
            stateMachine.SwitchState(new DragonGroundedState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
