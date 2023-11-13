using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DragonAttackType
{
    Fireball,
    Landing,
    Clawing,
    Summoning
}

public static class DragonAttack
{
    private static Dictionary<DragonAttackType, Type> stateType = new()
    {
        { DragonAttackType.Fireball, typeof(DragonFireballState) },
        { DragonAttackType.Landing, typeof(DragonLandingState) },
        { DragonAttackType.Clawing, typeof(DragonClawingState) }
    };

    public static DragonBaseState CreateNextState(DragonStateMachine stateMachine)
    {
        return (DragonBaseState)Activator.CreateInstance(stateType[stateMachine.NextAttackType], new[] {stateMachine});
    }
}