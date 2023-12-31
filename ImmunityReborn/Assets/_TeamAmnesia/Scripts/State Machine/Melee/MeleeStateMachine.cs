using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeStateMachine : StateMachine
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

    [field: SerializeField, Tooltip("Start chasing if target is within this range")]
    public float ChaseRange { get; private set; }

    [field: SerializeField, Tooltip("Perform attack if target is within this range")]
    public float AttackRange { get; private set; }

    [field: SerializeField]
    public int AttackDamage { get; private set; }

    [field: SerializeField]
    public float AttackKnockback { get; private set; }

    [field: SerializeField]
    public GameObject AggroIndicator { get; private set; }

    [field: SerializeField]
    public bool IsBoss { get; private set; }

    [field: SerializeField, Range(0.0f, 1.0f), Tooltip("Percent of damage dealt that becomes health")]
    public float LeechModifier { get; private set; }

    [field: SerializeField]
    public ParticleSystem LeechVFX { get; private set; }

    [field: SerializeField]
    List<GameObject> DeathSFXs;

    [field: SerializeField]
    public GameObject TakeDamageEffect;

    public GameObject Player { get; private set; }

    public Health PlayerHealth { get; private set; }

    public static int BossHP = 500;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();
        AggroIndicator.SetActive(false);
        // Adjusted health based on difficulty
        if (IsBoss)
            Health.SetHealth(BossHP);
    }

    private void Start()
    {
        NavMeshAgent.updatePosition = false; // for manual control
        NavMeshAgent.updateRotation = false;

        SwitchState(new MeleeIdleState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;

        PlayerHealth.OnDie += HandlePlayerDie;

        WeaponDamager.OnDamageDealt += HandleOnDamageDealt;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;

        PlayerHealth.OnDie -= HandlePlayerDie;

        WeaponDamager.OnDamageDealt -= HandleOnDamageDealt;
    }

    private void HandleTakeDamage()
    {
        PlaySFX.PlayThenDestroy(TakeDamageEffect, gameObject.transform);

        if (!IsBoss)
        {
            SwitchState(new MeleeImpactState(this));
        }
    }

    private void HandleDie()
    {
        GameObject RandomDeathSFX = DeathSFXs[Random.Range(0, DeathSFXs.Count)];
        PlaySFX.PlayThenDestroy(RandomDeathSFX, gameObject.transform);
        SwitchState(new MeleeDeadState(this));
        Destroy( gameObject , 10f );
    }

    private void HandlePlayerDie()
    {
        SwitchState(new MeleeIdleState(this));
    }

    private void HandleOnDamageDealt(int damageDealt)
    {
        if (IsBoss && damageDealt > 0)
        {
            Health.Heal(Mathf.RoundToInt(damageDealt * LeechModifier));
            LeechVFX.Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
}
