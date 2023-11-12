using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicStateMachine : StateMachine
{
    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public CharacterController CharacterController { get; private set; }

    [field: SerializeField]
    public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField]
    public NavMeshAgent NavMeshAgent { get; private set; }

    [field: SerializeField]
    public DOTPuddle DOTPuddle { get; private set; }

    [field: SerializeField]
    public Health Health { get; private set; }

    [field: SerializeField]
    public Ragdoll Ragdoll { get; private set; }

    [field: SerializeField]
    public float MovementSpeed { get; private set; }

    [field: SerializeField, Tooltip("Start chasing if target is within this range")]
    public float ChaseRange { get; private set; }

    [field: SerializeField, Tooltip("Perform attack if target is within this range")]
    public float AttackRange { get; private set; }

    [field: SerializeField]
    public int AttackDamage { get; private set; }

    [field: SerializeField]
    public float AttackKnockback { get; private set; }

    public GameObject Player { get; private set; }

    public Health PlayerHealth { get; private set; }

    public EnemySpawnManager EnemySpawnManager { get; private set; }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();
        EnemySpawnManager = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawnManager>();
    }

    private void Start()
    {
        NavMeshAgent.updatePosition = false; // for manual control
        NavMeshAgent.updateRotation = false;

        SwitchState(new MagicIdleState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;

        PlayerHealth.OnDie += HandlePlayerDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;

        PlayerHealth.OnDie -= HandlePlayerDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new MagicImpactState(this));
    }

    private void HandleDie()
    {
        // Check if the MinibossDeath component exists on this GameObject
        MinibossDeath minibossDeathComponent = GetComponent<MinibossDeath>();
        if (minibossDeathComponent != null)
            minibossDeathComponent.initNextStage(EnemySpawnManager);
        else // A regular enemy, increment the kill counter
            EnemySpawnManager.enemiesKilled++;
        SwitchState(new MagicDeadState(this));
    }

    private void HandlePlayerDie()
    {
        SwitchState(new MagicIdleState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
}
