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
        stateMachine.WeaponDamager.SetDamage(attackData.Damage);
        stateMachine.Animator.CrossFadeInFixedTime(attackData.AnimationName, attackData.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.DefaultMovementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue != Vector2.zero)
        {
            FaceMovementDirection(movement, deltaTime);
        }

        stateMachine.SwitchState(new PlayerDefaultState(stateMachine));
    }

    public override void Exit()
    {
    }
}
