using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRetreatingState : MeleeBaseState
{
    private readonly int CirculatingStateName = Animator.StringToHash("Circulating");

    private const float RetreatingSpeed = 1.1f;
    private const float CrossFadeDuration = 0.1f;
    private const float MinDuration = 0.5f;
    private const float MaxDuration = 1.0f;

    private float duration;

    public MeleeRetreatingState(MeleeStateMachine stateMachine) : base(stateMachine)
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
            stateMachine.SwitchState(new MeleeCirculatingState(stateMachine));
        }
    }

    public override void Exit()
    {
    }
}
