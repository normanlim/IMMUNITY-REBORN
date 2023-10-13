using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Upon jumping, timer logged to the console will reset due to change in state.
/// </summary>
public class PlayerExampleState1 : PlayerBaseState
{
    private string exampleParam;
    private float exampleTimer;

    public PlayerExampleState1(PlayerStateMachine stateMachine, string exampleParam = "exampleParam") : base(stateMachine)
    {
        this.exampleParam = exampleParam;
    }

    public override void Enter()
    {
        stateMachine.InputReader.JumpEvent += OnJump;
    }

    public override void Tick(float deltaTime)
    {
        exampleTimer += deltaTime;
        Debug.Log(exampleTimer);
    }

    public override void Exit()
    {
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerExampleState1(stateMachine));
    }
}
