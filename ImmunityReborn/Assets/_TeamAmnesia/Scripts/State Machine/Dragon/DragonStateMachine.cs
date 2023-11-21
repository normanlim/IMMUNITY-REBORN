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
    public Transform SampleAroundPoint { get; private set; }

    [field: SerializeField]
    public DragonActions DragonActions { get; private set; }

    [field: SerializeField]
    public WeaponDamager LandingWeaponDamager { get; private set; }

    [field: SerializeField]
    public WeaponDamager ClawWeaponDamager { get; private set; }

    [field: SerializeField]
    public ProjectileShooter ProjectileShooter { get; private set; }

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
    public float SummonRadius { get; private set; }

    [field: SerializeField]
    public GameObject SummonCharacter { get; private set; }

    [field: SerializeField]
    public SkinnedMeshRenderer MeshRenderer { get; private set; }

    [field: SerializeField]
    public Material[] Materials { get; private set; }

    public GameObject Player { get; private set; }

    public Health PlayerHealth { get; private set; }

    public DragonFlyingAction NextAttackType { get; set; }

    public GameObject SFXAwake;
    public GameObject SFXCombatStart;
    public GameObject SFXTakingDamage;
    public GameObject SFXTakingOff;
    public GameObject SFXLanding;
    public GameObject SFXFlying;
    public GameObject SFXFireball;
    public GameObject SFXClawing;
    public GameObject SFXSummoning;
    public GameObject SFXDeath;
    public GameObject TakeDamageEffect;
    [SerializeField] GameObject TakeDamageBodyPart;

    private LayerMask environmentLayer;
    private int bomberWalkableArea;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();

        environmentLayer = LayerMask.NameToLayer("Environment");
        bomberWalkableArea = 1 << NavMesh.GetAreaFromName("Bomber Walkable");

        // TODO: Remove Debug
        OnStateChange += (state, stateMachine) => { Debug.Log(state); };
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

    public bool RaycastToGround(out RaycastHit hitInfo)
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 50.0f, 1 << environmentLayer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);

            hitInfo = hit;
            return true;
        }

        hitInfo = new RaycastHit();
        return false;
    }

    private void HandleTakeDamage()
    {
        Instantiate(TakeDamageEffect, TakeDamageBodyPart.transform.position, Quaternion.identity);
        PlaySFX.PlayThenDestroy(SFXTakingDamage, transform);
        //SwitchState(new DragonImpactState(this));
    }

    private void HandleDie()
    {
        ProjectileShooter.ShooterDied();
        SwitchState(new DragonDeadState(this));
    }

    private void HandlePlayerDie()
    {
        SwitchState(new DragonIdleState(this));
    }

    private void SpitFireball()
    {
        ProjectileShooter.TryAimingAtTarget();
        ProjectileShooter.FireAtTarget(LandingDamage, LandingKnockback);
    }

    private void Summon()
    {
        if (NavMeshSampler.RandomPointAroundPosition(SampleAroundPoint.position, SummonRadius, out Vector3 result, bomberWalkableArea))
        {
            Instantiate(SummonCharacter, result, Quaternion.LookRotation(Player.transform.position));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CombatRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(SampleAroundPoint.position, SummonRadius);
    }
}
