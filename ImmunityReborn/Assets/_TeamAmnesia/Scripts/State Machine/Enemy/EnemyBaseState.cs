using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        stateMachine.CharacterController.Move((movement + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPos.y = 0.0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected bool IsInChaseRange()
    {
        float distanceToPlayerSqr = (stateMachine.Player.transform.position -
            stateMachine.transform.position).sqrMagnitude; // more performant than Vector3.magnitude which uses sqrt

        return distanceToPlayerSqr <= stateMachine.ChaseRange * stateMachine.ChaseRange;
    }

    protected bool IsInAttackRange()
    {
        float distanceToPlayerSqr = (stateMachine.Player.transform.position -
            stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
