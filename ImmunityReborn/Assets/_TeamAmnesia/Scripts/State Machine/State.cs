using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

    /// <summary>
    /// Gets the currently playing animation's duration played no matter how long the animation is.
    /// </summary>
    /// <returns>0.0f to 1.0f where 1.0f represents 100% of animation played</returns>
    protected float GetPlayingAnimationTimeNormalized(Animator animator)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0.0f;
        }
    }
}
