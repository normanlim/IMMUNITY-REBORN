using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DragonFlyingAction
{
    Landing,
    Fireball,
    Summoning
}

public enum DragonGroundedAction
{
    TakingOff,
    Clawing,
    Fireball,
    Summoning
}

/// <summary>
/// Selects a random action to perform and reduces it's weight.
/// Expect to perform Landing (if flying) or TakingOff (if grounded) eventually.
/// Weights are reset during these two actions' states;
/// </summary>
public class DragonActions : MonoBehaviour
{
    private static readonly Dictionary<DragonFlyingAction, Type> FlyingStates = new()
    {
        { DragonFlyingAction.Landing, typeof(DragonLandingState) },
        { DragonFlyingAction.Fireball, typeof(DragonFireballState) },
        { DragonFlyingAction.Summoning, typeof(DragonSummoningState) }
    };

    private static readonly Dictionary<DragonGroundedAction, Type> GroundedStates = new()
    {
        { DragonGroundedAction.TakingOff, typeof(DragonTakingOffState) },
        { DragonGroundedAction.Clawing, typeof(DragonClawingState) },
        { DragonGroundedAction.Fireball, typeof(DragonFireballState) },
        { DragonGroundedAction.Summoning, typeof(DragonSummoningState) }
    };

    private static readonly Dictionary<DragonFlyingAction, int> FlyingActionWeights = new()
    {
        { DragonFlyingAction.Landing, 3 },
        { DragonFlyingAction.Fireball, 6 },
        { DragonFlyingAction.Summoning, 3 }
    };

    private static readonly Dictionary<DragonGroundedAction, int> GroundedActionWeights = new()
    {
        { DragonGroundedAction.TakingOff, 3 },
        { DragonGroundedAction.Clawing, 4 },
        { DragonGroundedAction.Fireball, 2 },
        { DragonGroundedAction.Summoning, 2 }
    };

    [field: SerializeField]
    public Vector2 ActionDelayRange { get; private set; }

    private Dictionary<DragonFlyingAction, int> flyingActionWeights;
    private Dictionary<DragonGroundedAction, int> groundedActionWeights;
    private float actionDelayTimer;

    private void Awake()
    {
        flyingActionWeights = new(FlyingActionWeights);
        groundedActionWeights = new(GroundedActionWeights);
        actionDelayTimer = Random.Range(ActionDelayRange.x, ActionDelayRange.y);
    }

    public void Tick(DragonStateMachine stateMachine, bool isFlying, float deltaTime)
    {
        actionDelayTimer -= deltaTime;
        if (actionDelayTimer <= 0.0f)
        {
            if (isFlying)
            {
                stateMachine.SwitchState(NextFlyingAttackState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(NextGroundedAttackState(stateMachine));
            }
            actionDelayTimer = Random.Range(ActionDelayRange.x, ActionDelayRange.y);
        }
    }

    public void ResetFlyingActionWeights()
    {
        flyingActionWeights = new(FlyingActionWeights);
    }

    public void ResetGroundedActionWeights()
    {
        groundedActionWeights = new(GroundedActionWeights);
    }

    private DragonBaseState NextFlyingAttackState(DragonStateMachine stateMachine)
    {
        DragonFlyingAction nextAttack = PickRandomAction(flyingActionWeights.Keys.ToArray(), flyingActionWeights.Values.ToArray());

        flyingActionWeights[nextAttack]--; // reduce probability of same attack

        return (DragonBaseState)Activator.CreateInstance(FlyingStates[nextAttack], new[] { stateMachine });
    }

    private DragonBaseState NextGroundedAttackState(DragonStateMachine stateMachine)
    {
        DragonGroundedAction nextAttack = PickRandomAction(groundedActionWeights.Keys.ToArray(), groundedActionWeights.Values.ToArray());

        groundedActionWeights[nextAttack]--; // reduce probability of same attack

        return (DragonBaseState)Activator.CreateInstance(GroundedStates[nextAttack], new[] { stateMachine });
    }

    private T PickRandomAction<T>(T[] options, int[] weights)
    {
        int totalWeight = weights.Sum();
        int randomValue = Random.Range(0, totalWeight);

        for (int i = 0; i < options.Length; i++)
        {
            if (randomValue < weights[i])
            {
                return options[i];
            }

            randomValue -= weights[i];
        }

        return options.Last();
    }
}