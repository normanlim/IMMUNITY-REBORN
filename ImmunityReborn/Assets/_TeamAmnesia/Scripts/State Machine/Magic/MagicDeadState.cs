using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDeadState : MagicBaseState
{
    public MagicDeadState(MagicStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (!stateMachine.HasDiedExploding)
        {
            PlaySFX.PlayThenDestroy(stateMachine.ExplosionSFX, stateMachine.gameObject.transform);
        }
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
