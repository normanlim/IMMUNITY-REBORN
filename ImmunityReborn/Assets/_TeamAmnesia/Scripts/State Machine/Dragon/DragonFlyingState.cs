using UnityEngine;

public class DragonFlyingState : DragonBaseState
{
    private readonly int FlyStateName = Animator.StringToHash("FlyStationary");

    private const float CrossFadeDuration = 0.5f;

    private GameObject FlyingSoundObject;

    public DragonFlyingState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FlyStateName, CrossFadeDuration);
        FlyingSoundObject = PlaySFX.PlayWithLoop(stateMachine.SFXFlying, stateMachine.transform);
    }

    public override void Tick(float deltaTime)
    {
        FacePlayer(deltaTime);

        FlyToPlayer(deltaTime);

        stateMachine.DragonActions.Tick(stateMachine, true, deltaTime);
    }

    public override void Exit()
    {
        PlaySFX.StopLoopedAudio(FlyingSoundObject);
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }
}