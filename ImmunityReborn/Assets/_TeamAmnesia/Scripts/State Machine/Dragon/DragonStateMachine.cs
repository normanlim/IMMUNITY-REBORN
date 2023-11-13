using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonStateMachine : StateMachine
{
    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public CharacterController CharacterController { get; private set; }

    [field: SerializeField]
    public FlyingForceReceiver FlyingForceReceiver { get; private set; }

    [field: SerializeField]
    public NavMeshAgent NavMeshAgent { get; private set; }

    [field: SerializeField]
    public NavMeshSampler NavMeshSampler { get; private set; }

    [field: SerializeField]
    public WeaponDamager WeaponDamager { get; private set; }

    [field: SerializeField]
    public Arrow Arrow { get; private set; }

    [field: SerializeField]
    public Health Health { get; private set; }

    [field: SerializeField]
    public Ragdoll Ragdoll { get; private set; }

    [field: SerializeField]
    public float FlyingSpeed { get; private set; }

    [field: SerializeField]
    public float GroundedSpeed { get; private set; }

    [field: SerializeField, Tooltip("Engage in combat if player is within this range")]
    public float CombatRange { get; private set; }

    [field: SerializeField, Tooltip("Perform attack if target is within this range")]
    public float ClawAttackRange { get; private set; }

    [field: SerializeField]
    public int ClawAttackDamage { get; private set; }

    [field: SerializeField]
    public float ClawAttackKnockback { get; private set; }

    [field: SerializeField]
    public int LandingDamage { get; private set; }

    [field: SerializeField]
    public float LandingKnockback { get; private set; }

    [field: SerializeField]
    public Vector2 NextAttackDelayRange { get; private set; }

    public GameObject Player { get; private set; }

    public Health PlayerHealth { get; private set; }

    public DragonAttackType NextAttackType { get; set; }

    public Dictionary<DragonAttackType, int> AttacksCounter { get; set; } = new()
    {
        { DragonAttackType.Fireball, 0 },
        { DragonAttackType.Clawing, 0 }
    };

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();

        // TODO: Remove Debug
        OnStateChange += (state) => { Debug.Log(state); };
    }

    private void Start()
    {
        NavMeshAgent.updatePosition = false; // for manual control
        NavMeshAgent.updateRotation = false;

        SwitchState(new DragonIdleState(this));
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
        //SwitchState(new DragonImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new DragonDeadState(this));
    }

    private void HandlePlayerDie()
    {
        SwitchState(new DragonIdleState(this));
    }

    private void SpitFireball()
    {
        Arrow.FireAtPlayer(LandingDamage, LandingKnockback);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CombatRange);
    }
}
