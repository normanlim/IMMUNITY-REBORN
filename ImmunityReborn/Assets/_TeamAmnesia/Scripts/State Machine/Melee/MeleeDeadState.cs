using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDeadState : MeleeBaseState
{
    public MeleeDeadState(MeleeStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Ragdoll.ToggleRagdoll(true);
        stateMachine.WeaponDamager.gameObject.SetActive(false);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
