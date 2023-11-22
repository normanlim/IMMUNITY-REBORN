public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Ragdoll.ToggleRagdoll(true);
        PlaySFX.PlayThenDestroy(stateMachine.SFXDeath, stateMachine.transform);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
