using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeImpactState : MeleeBaseState
{
    private readonly int ImpactStateName = Animator.StringToHash("GetHitSwordShield");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1.0f;

    public MeleeImpactState(MeleeStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactStateName, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime); // applies ForceReceiver's movement

        UpdateLocomotionAnimator(deltaTime);

        // Enemy will stay in this state until countdown is finished
        duration -= deltaTime;

        if (duration <= 0.0f)
        {
            stateMachine.SwitchState(new MeleeIdleState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}
