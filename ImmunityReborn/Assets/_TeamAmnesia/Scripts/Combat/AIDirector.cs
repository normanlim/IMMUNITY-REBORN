using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIDirector : MonoBehaviour
{
    [SerializeField]
    private SphereCollider meleeZone;

    private const float MaxSwitchStateDelay = 1.0f;
    private const int MaxAttackingMeleeEnemies = 3;

    private List<MeleeStateMachine> meleeEnemies;
    private List<MeleeStateMachine> attackingMeleeEnemies;

    void Start()
    {
        meleeEnemies = new List<MeleeStateMachine>();
        attackingMeleeEnemies = new List<MeleeStateMachine>();
    }

    void Update()
    {
        meleeEnemies.RemoveAll(x => x == null);
        attackingMeleeEnemies.RemoveAll(x => x == null);

        if (attackingMeleeEnemies.Count < MaxAttackingMeleeEnemies)
        {
            if (RandomMeleeEnemy(out MeleeStateMachine meleeEnemy))
            {
                meleeEnemy.SwitchState(new MeleeAdvancingState(meleeEnemy));
                meleeEnemy.AggroIndicator.SetActive(true);
                attackingMeleeEnemies.Add(meleeEnemy);
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
        if (stateMachine is MeleeStateMachine meleeEnemy)
        {
            if (attackingMeleeEnemies.Contains(meleeEnemy))
            {
                if (state is not MeleeAdvancingState && state is not MeleeAttackingState) // if enemy isn't in these states, it means they aren't attacking
                {
                    meleeEnemy.AggroIndicator.SetActive(false);
                    attackingMeleeEnemies.Remove(meleeEnemy);
                }
            }

            if (state is MeleeDeadState)
            {
                RemoveMeleeEnemy(meleeEnemy);
            }
        }
    }

    private bool RandomMeleeEnemy(out MeleeStateMachine enemy)
    {
        enemy = null;
        List<MeleeStateMachine> availableEnemies = new();

        foreach (MeleeStateMachine meleeEnemy in meleeEnemies)
        {
            if (meleeEnemy.IsCurrentStateInterruptible
                && meleeEnemy.CurrentState is not MeleeRetreatingState
                && !attackingMeleeEnemies.Contains(meleeEnemy))
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
