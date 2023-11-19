using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedStateMachine : StateMachine
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
    public ProjectileShooter ProjectileShooter { get; private set;  }

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
    public float MinAttackRange { get; private set; }

    [field: SerializeField]
    public int AttackDamage { get; private set; }

    [field: SerializeField]
    public float AttackKnockback { get; private set; }

    [field: SerializeField]
    public GameObject Player { get; private set; }

    [field: SerializeField]
    public Health PlayerHealth { get; private set; }

    [field: SerializeField]
    public GameObject DeathSFX;

    [field: SerializeField]
    public GameObject TakeDamageEffect;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();
    }

    private void Start()
    {
        NavMeshAgent.updatePosition = false; // for manual control
        NavMeshAgent.updateRotation = false;

        SwitchState(new RangedIdleState(this));
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
        Instantiate(TakeDamageEffect, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        SwitchState(new RangedImpactState(this));
    }

    private void HandleDie()
    {
        PlaySFX.PlayThenDestroy(DeathSFX, gameObject.transform);
        ProjectileShooter.ShooterDied();
        SwitchState(new RangedDeadState(this));
    }

    private void HandlePlayerDie()
    {
        SwitchState(new RangedIdleState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
}
