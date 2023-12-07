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
    public bool CanBerserk { get; private set; }

    [field: SerializeField, Range(0.0f, 1.0f), Tooltip("Berserks when at this health percent")]
    public float BerserkThreshold { get; private set; }

    [field: SerializeField]
    public GameObject BerserkVFX { get; private set; }

    [field: SerializeField]
    List<GameObject> DeathSFXs;

    [field: SerializeField]
    public GameObject TakeDamageEffect;

    public GameObject Player { get; private set; }

    public Health PlayerHealth { get; private set; }

    private bool IsBerserking;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();
        AggroIndicator.SetActive(false);
        BerserkVFX.SetActive(false);
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
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;

        PlayerHealth.OnDie -= HandlePlayerDie;
    }

    private void HandleTakeDamage()
    {
        PlaySFX.PlayThenDestroy(TakeDamageEffect, gameObject.transform);

        if (!IsBerserking && CanBerserk && Health.CurrentHealth < Health.MaxHealth * BerserkThreshold)
        {
            IsBerserking = true;
            BerserkVFX.SetActive(true);
        }

        if (!IsBerserking)
        {
            SwitchState(new MeleeImpactState(this));
        }
    }

    private void HandleDie()
    {
        BerserkVFX.SetActive(false);
        GameObject RandomDeathSFX = DeathSFXs[Random.Range(0, DeathSFXs.Count)];
        PlaySFX.PlayThenDestroy(RandomDeathSFX, gameObject.transform);
        SwitchState(new MeleeDeadState(this));
        Destroy( gameObject , 10f );
    }

    private void HandlePlayerDie()
    {
        SwitchState(new MeleeIdleState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }
}
