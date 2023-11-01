using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedBaseState : State
{
    protected RangedStateMachine stateMachine;

    public RangedBaseState(RangedStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
