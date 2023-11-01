using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeChasingState : MeleeBaseState
{
    private readonly int LocomotionStateName = Animator.StringToHash("Locomotion");
    
    private const float CrossFadeDuration = 0.1f;

    public MeleeChasingState(MeleeStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionStateName, CrossFadeDuration, -1);
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            stateMachine.SwitchState(new MeleeIdleState(stateMachine));
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new MeleeAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);

        FacePlayer();

        UpdateAnimator(deltaTime);
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}
