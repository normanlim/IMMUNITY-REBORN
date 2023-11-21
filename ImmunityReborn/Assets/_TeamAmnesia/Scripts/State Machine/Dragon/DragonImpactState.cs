using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonImpactState : DragonBaseState
{
    public DragonImpactState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // PlaySFX.PlayThenDestroy(stateMachine.SFXTakingDamage, stateMachine.transform);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}