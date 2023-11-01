using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicBaseState : State
{
    protected MagicStateMachine stateMachine;

    public MagicBaseState(MagicStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
