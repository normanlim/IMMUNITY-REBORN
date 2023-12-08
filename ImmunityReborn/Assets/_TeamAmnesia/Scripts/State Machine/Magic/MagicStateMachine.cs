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
    public WeaponDamager WeaponDamager { get; private set; }

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

    [field: SerializeField]
    public float PuddleDPS { get; private set; }

    public GameObject Player { get; private set; }

    public Health PlayerHealth { get; private set; }

    [field: SerializeField]
    public GameObject TakeDamageEffect;

    [field: SerializeField]
    public GameObject DeathEffect;

    [field: SerializeField]
    public GameObject ExplosionSFX;

    public bool HasDiedExploding
        ;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();
    }

    private void Start()
    {
        NavMeshAgent.updatePosition = false; // for manual control
        NavMeshAgent.updateRotation = false;
        DOTPuddle.SetPuddleDPS(PuddleDPS);
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
        Instantiate(TakeDamageEffect, transform.position + new Vector3(0f, 0.3f, 0f), Quaternion.identity);
        SwitchState(new MagicImpactState(this));
    }

    private void HandleDie()
    {
        Instantiate(DeathEffect, transform);
        SwitchState(new MagicDeadState(this));
        Destroy( gameObject, 0.9f); // Since bombers explode, we can just get rid of their body without dealing with ragdoll
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
