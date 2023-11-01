using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public RangedBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
