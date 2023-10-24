using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
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
    public WeaponDamager WeaponDamager { get; private set; }

    [field: SerializeField]
    public Health Health { get; private set; }

    [field: SerializeField]
    public Ragdoll Ragdoll { get; private set; }

    [field: SerializeField]
    public float MovementSpeed { get; private set; }

    [field: SerializeField]
    public float ChaseRange { get; private set; }

    [field: SerializeField]
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
        Player = GameObject.FindGameObjectWithTag("Player");

        NavMeshAgent.updatePosition = false; // for manual control
        NavMeshAgent.updateRotation = false;

        SwitchState(new EnemyIdleState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));
    }

    private void HandleDie()
    {
        // Check if the MinibossDeath component exists on this GameObject
        MinibossDeath minibossDeathComponent = GetComponent<MinibossDeath>();
        if (minibossDeathComponent != null)
            minibossDeathComponent.initNextStage(EnemySpawnManager);
        else // A regular enemy, increment the kill counter
            EnemySpawnManager.enemiesKilled++;
        SwitchState(new EnemyDeadState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
}
