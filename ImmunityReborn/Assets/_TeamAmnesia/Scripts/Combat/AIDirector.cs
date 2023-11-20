using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIDirector : MonoBehaviour
{
    [SerializeField]
    private SphereCollider meleeZone;

    private const float MaxSwitchStateDelay = 1.0f;

    private List<MeleeStateMachine> meleeEnemies;
    private MeleeStateMachine attackingMeleeEnemy;

    void Start()
    {
        meleeEnemies = new List<MeleeStateMachine>();
        attackingMeleeEnemy = null;
    }

    void Update()
    {
        meleeEnemies.RemoveAll(x => x == null);

        if (attackingMeleeEnemy == null)
        {
            if (RandomMeleeEnemy(out attackingMeleeEnemy))
            {
                attackingMeleeEnemy.SwitchState(new MeleeAdvancingState(attackingMeleeEnemy));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MeleeStateMachine meleeStateMachine) && meleeStateMachine.CurrentState is not MeleeDeadState)
        {
            AddMeleeEnemy(meleeStateMachine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MeleeStateMachine meleeStateMachine))
        {
            RemoveMeleeEnemy(meleeStateMachine);
        }
    }

    private void HandleEnemyStateChange(State state, StateMachine stateMachine)
    {
        if (stateMachine is MeleeStateMachine meleeStateMachine)
        {
            if (meleeStateMachine == attackingMeleeEnemy)
            {
                if (state is not MeleeAdvancingState && state is not MeleeAttackingState) // if enemy isn't in these states, it means they aren't attacking
                {
                    attackingMeleeEnemy = null;
                }
            }

            if (state is MeleeDeadState)
            {
                RemoveMeleeEnemy(meleeStateMachine);
            }
        }
    }

    private bool RandomMeleeEnemy(out MeleeStateMachine enemy)
    {
        enemy = null;
        List<MeleeStateMachine> availableEnemies = new();

        foreach (MeleeStateMachine meleeEnemy in meleeEnemies)
        {
            if (meleeEnemy.IsCurrentStateInterruptible)
            {
                availableEnemies.Add(meleeEnemy);
            }
        }

        if (availableEnemies.Count == 0) { return false; }

        int randomIndex = Random.Range(0, availableEnemies.Count);

        enemy = availableEnemies[randomIndex];
        return true;
    }

    private float RandomDelay()
    {
        return Random.Range(0.0f, MaxSwitchStateDelay);
    }

    private void AddMeleeEnemy(MeleeStateMachine meleeStateMachine)
    {
        meleeStateMachine.OnStateChange += HandleEnemyStateChange;
        meleeEnemies.Add(meleeStateMachine);

        if (meleeStateMachine.IsCurrentStateInterruptible) // check if allowed to force state change
        {
            meleeStateMachine.SwitchState(new MeleeCirculatingState(meleeStateMachine), RandomDelay()); // delay to vary how deep enemies run into the melee zone
        }
    }

    private void RemoveMeleeEnemy(MeleeStateMachine meleeStateMachine)
    {
        meleeStateMachine.OnStateChange -= HandleEnemyStateChange;
        meleeEnemies.Remove(meleeStateMachine);

        if (meleeStateMachine.IsCurrentStateInterruptible)
        {
            meleeStateMachine.SwitchState(new MeleeIdleState(meleeStateMachine));
        }
    }
}
