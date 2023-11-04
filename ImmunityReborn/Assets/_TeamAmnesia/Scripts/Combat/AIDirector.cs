using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
        if (attackingMeleeEnemy == null)
        {
            attackingMeleeEnemy = RandomMeleeEnemy();

            if (attackingMeleeEnemy != null)
            {
                attackingMeleeEnemy.SwitchState(new MeleeAdvancingState(attackingMeleeEnemy));
                attackingMeleeEnemy.OnStateChange += HandleMeleeEnemyStateChange;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MeleeStateMachine stateMachine = other.GetComponent<MeleeStateMachine>();        
        meleeEnemies.Add(stateMachine);

        if (stateMachine.IsCurrentStateInterruptible)
        {
            stateMachine.SwitchState(new MeleeCirculatingState(stateMachine), RandomDelay()); // delay to vary how deep enemies run into the melee zone
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MeleeStateMachine stateMachine = other.GetComponent<MeleeStateMachine>();
        meleeEnemies.Remove(stateMachine);
        if (attackingMeleeEnemy == stateMachine)
        {
            DisposeAttackingEnemy();
        }

        if (stateMachine.IsCurrentStateInterruptible)
        {
            stateMachine.SwitchState(new MeleeChasingState(stateMachine));
        }
    }

    private void HandleMeleeEnemyStateChange(State state)
    {
        if (state is MeleeRetreatingState || state is MeleeImpactState) // enemy is no longer attacking
        {
            DisposeAttackingEnemy();
        }
    }

    private MeleeStateMachine RandomMeleeEnemy()
    {
        List<MeleeStateMachine> availableEnemies = new();

        foreach (MeleeStateMachine meleeEnemy in meleeEnemies)
        {
            if (meleeEnemy.IsCurrentStateInterruptible)
            {
                availableEnemies.Add(meleeEnemy);
            }
        }

        if (availableEnemies.Count == 0) { return null; }

        int randomIndex = Random.Range(0, availableEnemies.Count);

        return availableEnemies[randomIndex];
    }

    private float RandomDelay()
    {
        return Random.Range(0.0f, MaxSwitchStateDelay);
    }

    private void DisposeAttackingEnemy()
    {
        attackingMeleeEnemy.OnStateChange -= HandleMeleeEnemyStateChange;
        attackingMeleeEnemy = null;
    }
}
