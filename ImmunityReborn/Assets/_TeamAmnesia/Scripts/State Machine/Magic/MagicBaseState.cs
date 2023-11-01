using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public MagicBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
