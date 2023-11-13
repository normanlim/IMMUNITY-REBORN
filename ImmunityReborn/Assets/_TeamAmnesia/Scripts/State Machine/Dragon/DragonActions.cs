using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DragonFlyingAction
{
    Fireball,
    Landing,
    Summoning
}

public enum DragonGroundedAction
{
    Clawing
}

public class DragonActions : MonoBehaviour
{
    private static readonly Dictionary<DragonFlyingAction, Type> FlyingStates = new()
    {
        { DragonFlyingAction.Fireball, typeof(DragonFireballState) },
        { DragonFlyingAction.Landing, typeof(DragonLandingState) },
        { DragonFlyingAction.Summoning, typeof(DragonSummoningState) }
    };

    private static readonly Dictionary<DragonGroundedAction, Type> GroundedStates = new()
    {
        { DragonGroundedAction.Clawing, typeof(DragonClawingState) },
    };

    private static readonly Dictionary<DragonFlyingAction, int> FlyingActionWeights = new()
    {
        { DragonFlyingAction.Fireball, 10 },
        { DragonFlyingAction.Landing, 3 },
        { DragonFlyingAction.Summoning, 7 }
    };

    private static readonly Dictionary<DragonGroundedAction, int> GroundedActionWeights = new()
    {
        { DragonGroundedAction.Clawing, 5 },
    };

    private Dictionary<DragonFlyingAction, int> flyingActionWeights;
    private Dictionary<DragonGroundedAction, int> groundedActionWeights;

    public DragonActions()
    {
        flyingActionWeights = new(FlyingActionWeights);
        groundedActionWeights = new(GroundedActionWeights);
    }

    public DragonBaseState NextFlyingAttackState(DragonStateMachine stateMachine)
    {
        DragonFlyingAction nextAttack = PickRandomAction(flyingActionWeights.Keys.ToArray(), flyingActionWeights.Values.ToArray());

        return (DragonBaseState)Activator.CreateInstance(FlyingStates[nextAttack], new[] { stateMachine });
    }

    public DragonBaseState NextGroundedAttackState(DragonStateMachine stateMachine)
    {
        DragonGroundedAction nextAttack = PickRandomAction(groundedActionWeights.Keys.ToArray(), groundedActionWeights.Values.ToArray());

        return (DragonBaseState)Activator.CreateInstance(GroundedStates[nextAttack], new[] { stateMachine });
    }

    public T PickRandomAction<T>(T[] options, int[] weights)
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