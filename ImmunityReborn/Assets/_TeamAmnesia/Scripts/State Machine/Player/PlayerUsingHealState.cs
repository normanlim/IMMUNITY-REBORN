using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUsingHealState : PlayerBaseState
{
    private readonly int UseHealStateName = Animator.StringToHash("Sword And Shield Power Up");

    private const float CrossFadeDuration = 0.1f;
    private bool usedHeal;
    public PlayerUsingHealState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.VFXHealing.SetActive(true); 
        usedHeal = false;
        if (stateMachine.HealthConsumable.CurrentItemCount == 0)
        {
            stateMachine.SwitchState(new PlayerDefaultState(stateMachine));
            return;
        }

        stateMachine.Animator.CrossFadeInFixedTime(UseHealStateName, CrossFadeDuration, 1);
    }

    public override void Tick(float deltaTime)
    {
        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 1) >= 0.7f && !usedHeal) // make comparing with less than (1.0f - Transition Duration) in Animator Transition Settings
        {
            stateMachine.HealthConsumable.Use();
            usedHeal = true;
            stateMachine.VFXHealing.SetActive(false);
            stateMachine.SwitchState(new PlayerDefaultState(stateMachine));
            return;
        }

        FaceCameraDirection(deltaTime);

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.DefaultMovementSpeed / 2, deltaTime); // player's movement cut in half

        UpdateAnimator(deltaTime);
    }

    public override void Exit()
    {
        stateMachine.VFXHealing.SetActive(false);
    }
}
