using UnityEngine;

public class TormentedSoulArrowRainState : TormentedSoulBaseState
{
    private readonly int SkyAimAnimation = Animator.StringToHash("SkyAim");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");
    private const float TransitionDuration = 0.1f;
    private const float StateDuration = 6f;
    private float duration;
    public TormentedSoulArrowRainState(TormentedSoulStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.transform.Find("ArrowRainAura").gameObject.SetActive(true);
        duration = StateDuration;
        stateMachine.NormalAttackCount = 0;
        stateMachine.Animator.CrossFadeInFixedTime(SkyAimAnimation, TransitionDuration, 0);
        // Triple fire 3 arrows in a cone
        stateMachine.PerformArrowRain();
    }

    public override void Tick(float deltaTime)
    {

        duration -= deltaTime;
        if (duration <= 0.0f)
        {
            stateMachine.SwitchState(new TormentedSoulRetreatingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
        stateMachine.transform.Find("ArrowRainAura").gameObject.SetActive(false);
        stateMachine.Animator.CrossFade(EmptyDefaultStateName, TransitionDuration, 0); // without this, gets stuck at end of attack animation
    }
}
