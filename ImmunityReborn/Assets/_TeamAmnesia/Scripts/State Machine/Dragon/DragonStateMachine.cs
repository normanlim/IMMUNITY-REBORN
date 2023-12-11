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
    public WeaponDamager RightClawWeaponDamager { get; private set; }

    [field: SerializeField]
    public WeaponDamager LeftClawWeaponDamager { get; private set; }

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
    public float SummonCount { get; private set; }

    [field: SerializeField]
    public GameObject SummonCharacter { get; private set; }

    [field: SerializeField]
    public Transform FirebreathSpawnPoint { get; private set; }

    [field: SerializeField]
    public GameObject FirebreathPrefab { get; private set; }

    [field: SerializeField]
    public int FirebreathDamage { get; private set; }

    [field: SerializeField]
    public float FirebreathKnockback { get; private set; }

    [field: SerializeField]
    public float FirebreathDamageCooldown { get; private set; }

    [field: SerializeField]
    public SkinnedMeshRenderer MeshRenderer { get; private set; }

    [field: SerializeField]
    public Material[] Materials { get; private set; }

    [field: SerializeField]
    public VictoryScreen CongratulationsScreen { get; private set; }

    public GameObject Player { get; private set; }

    public Health PlayerHealth { get; private set; }

    public DragonFlyingAction NextAttackType { get; set; }

    public static int BossHP = 2000;

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
    public GameObject FlyingSoundObject;
    [SerializeField] GameObject TakeDamageBodyPart;

    private LayerMask environmentLayer;
    private int bomberWalkableArea;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();

        environmentLayer = LayerMask.NameToLayer("Environment");
        bomberWalkableArea = 1 << NavMesh.GetAreaFromName("Bomber Walkable");

        // Adjusted health based on difficulty
        Health.SetHealth(BossHP);
        CongratulationsScreen = FindFirstObjectByType<VictoryScreen>(FindObjectsInactive.Include);
    }

    private void Start()
    {
        NavMeshAgent.updatePosition = false; // for manual control
        NavMeshAgent.updateRotation = false;

        if (SampleAroundPoint == null)
            SampleAroundPoint = transform.parent;

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
        if (FlyingSoundObject != null)
            Destroy(FlyingSoundObject);
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

    public void DestroyGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
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
        PlayerPrefs.SetInt("SCCurrentLevel", 0);
        Invoke( "GameComplete", 10.0f );
        SwitchState(new DragonDeadState(this));
    }

    private void HandlePlayerDie()
    {
        SwitchState(new DragonIdleState(this));
    }

    private void GameComplete()
    {
        CongratulationsScreen.Setup();
    }

    private void SpitFireball()
    {
        ProjectileShooter.TryAimingAtTarget();
        ProjectileShooter.FireAtTarget(LandingDamage, LandingKnockback);
    }

    private void Summon()
    {
        for (int i = 0; i < SummonCount; i++)
        {
            if (NavMeshSampler.RandomPointAroundPosition(SampleAroundPoint.position, SummonRadius, out Vector3 result, bomberWalkableArea))
            {
                Instantiate(SummonCharacter, result, Quaternion.LookRotation(Player.transform.position));
            }
        }
    }

    private void StartFirebreath()
    {
        GameObject fireBreath = Instantiate(FirebreathPrefab, FirebreathSpawnPoint);
        fireBreath.GetComponent<ParticleDamager>().SetDamage(FirebreathDamage, FirebreathKnockback, FirebreathDamageCooldown);
    }

    private void StopFirebreath()
    {
        if (FirebreathSpawnPoint.childCount > 0 && FirebreathSpawnPoint.GetChild(0) is { } fireBreath)
        {
            DestroyGameObject(fireBreath.gameObject);
        }
    }

    private void PlayClawSound()
    {
        PlaySFX.PlayThenDestroy(SFXClawing, transform);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CombatRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(SampleAroundPoint.position, SummonRadius);
    }
}
