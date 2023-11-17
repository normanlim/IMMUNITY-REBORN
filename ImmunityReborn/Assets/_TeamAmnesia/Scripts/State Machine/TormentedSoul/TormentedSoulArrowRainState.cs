using UnityEngine;

public class TormentedSoulArrowRainState : TormentedSoulBaseState
{
    private readonly int SkyAimAnimation = Animator.StringToHash("SkyAim");
    private readonly int EmptyDefaultStateName = Animator.StringToHash("Empty Default");
    private const float TransitionDuration = 0.1f;
    public TormentedSoulArrowRainState(TormentedSoulStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.transform.Find("ArrowRainAura").gameObject.SetActive(true);
        stateMachine.NormalAttackCount = 0;
        stateMachine.Animator.CrossFadeInFixedTime(SkyAimAnimation, TransitionDuration, 0);
        
    }

    public override void Tick(float deltaTime)
    {

        if (GetPlayingAnimationTimeNormalized(stateMachine.Animator, 0) >= 1.0f)
        {
            stateMachine.SwitchState(new TormentedSoulRetreatingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.PerformArrowRain();
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
        stateMachine.transform.Find("ArrowRainAura").gameObject.SetActive(false);
        stateMachine.Animator.CrossFade(EmptyDefaultStateName, TransitionDuration, 0); // without this, gets stuck at end of attack animation
    }
}
