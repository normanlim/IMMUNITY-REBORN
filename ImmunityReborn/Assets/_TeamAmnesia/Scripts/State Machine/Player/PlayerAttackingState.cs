using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackId) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackId];
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
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
