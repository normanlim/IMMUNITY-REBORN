using UnityEngine;

public class TormentedSoulRetreatingState : TormentedSoulBaseState
{
    private readonly int CirculatingStateName = Animator.StringToHash("Circulating");
    private readonly int DrawArrowStateName = Animator.StringToHash("DrawArrow");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");
    private const float CrossFadeDuration = 0.1f;
    private const float MinDuration = 1.4f;
    private const float MaxDuration = 2.1f;

    private float duration;

    public TormentedSoulRetreatingState(TormentedSoulStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        duration = Random.Range(MinDuration, MaxDuration);

        stateMachine.Animator.CrossFadeInFixedTime(CirculatingStateName, CrossFadeDuration, 0);
        stateMachine.Animator.CrossFadeInFixedTime(DrawArrowStateName, CrossFadeDuration, 1);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        if (stateMachine.NavMeshAgent.isOnNavMesh && IsInAttackRange())
        {
            Vector3 direction = -stateMachine.transform.forward;

            stateMachine.NavMeshAgent.destination = stateMachine.transform.position + direction * stateMachine.MovementSpeed;

            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        UpdateCirculatingAnimator(deltaTime);

        duration -= deltaTime;

        if (duration <= 0.0f)
        {
            stateMachine.SwitchState(new TormentedSoulCirculatingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.CrossFade(EmptyDefaultStateName, CrossFadeDuration, 1);
    }
}
