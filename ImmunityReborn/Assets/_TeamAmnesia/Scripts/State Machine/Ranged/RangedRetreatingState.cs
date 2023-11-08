using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRetreatingState : RangedBaseState
{
    private readonly int CirculatingStateName = Animator.StringToHash("Circulating");

    private const float RetreatingSpeed = 1.1f;
    private const float CrossFadeDuration = 0.1f;
    private const float MinDuration = 1.5f;
    private const float MaxDuration = 2.0f;

    private float duration;

    public RangedRetreatingState(RangedStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        duration = Random.Range(MinDuration, MaxDuration);

        stateMachine.Animator.CrossFadeInFixedTime(CirculatingStateName, CrossFadeDuration, -1);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            Vector3 direction = -stateMachine.transform.forward;

            stateMachine.NavMeshAgent.destination = stateMachine.transform.position + direction * RetreatingSpeed;

            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * RetreatingSpeed, deltaTime);
        }

        UpdateCirculatingAnimator(deltaTime);

        duration -= deltaTime;

        if (duration <= 0.0f)
        {
            stateMachine.SwitchState(new RangedCirculatingState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}
