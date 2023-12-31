using UnityEngine;

public class TormentedSoulCirculatingState : TormentedSoulBaseState
{
    private readonly int CirculatingStateName = Animator.StringToHash("Circulating");

    private const float CrossFadeDuration = 0.1f;
    private const float AimingDuration = 0.5f;
    private float CurrentStateDuration;
    private bool isClockwise;

    public TormentedSoulCirculatingState(TormentedSoulStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isClockwise = RandomBoolean();
        CurrentStateDuration = AimingDuration;
        stateMachine.Animator.CrossFadeInFixedTime(CirculatingStateName, CrossFadeDuration, 0);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);
        MoveAroundPlayer(deltaTime);
        UpdateCirculatingAnimator(deltaTime);
        CurrentStateDuration -= deltaTime;
        if (stateMachine.NormalAttackCount < stateMachine.NumberAttacksBetweenMechs)
        {
            if (stateMachine.ProjectileShooter.TryAimingAtTarget() && CurrentStateDuration <= 0.0f)
            {
                stateMachine.SwitchState(new TormentedSoulAdvancingState(stateMachine));
            }
        } else
        {
            stateMachine.SwitchState(new TormentedSoulArrowRainState(stateMachine));
        }
        
    }

    public override void Exit()
    {
    }

    private void MoveAroundPlayer(float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            Vector3 direction = isClockwise ? -stateMachine.transform.right : stateMachine.transform.right;

            // CirculatingSpeed = MovementSpeed / 2
            stateMachine.NavMeshAgent.destination = stateMachine.transform.position + direction * stateMachine.MovementSpeed / 2;
            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed / 2, deltaTime);
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity;
        stateMachine.NavMeshAgent.nextPosition = stateMachine.CharacterController.transform.position;
    }

    private bool RandomBoolean()
    {
        return Random.Range(0.0f, 1.0f) < 0.5f;
    }
}
