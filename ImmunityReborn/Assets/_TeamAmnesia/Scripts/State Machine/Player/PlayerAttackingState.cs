using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private AttackData attackData;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attackData = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.WeaponDamager.SetDamage(attackData.Damage, attackData.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(attackData.AnimationName, attackData.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        FaceCameraDirection(deltaTime);

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.DefaultMovementSpeed, deltaTime);

        stateMachine.SwitchState(new PlayerDefaultState(stateMachine));
    }

    public override void Exit()
    {
    }
}
