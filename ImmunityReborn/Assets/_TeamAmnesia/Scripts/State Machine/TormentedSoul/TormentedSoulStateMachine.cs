using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TormentedSoulStateMachine : StateMachine
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
    public int AttackDamage { get; private set; }

    [field: SerializeField]
    public float AttackKnockback { get; private set; }

    [SerializeField] GameObject ArrowRainPortalPrefab;
    [SerializeField] int numPortalsToSpawn = 20;
    [SerializeField] float minDistance = 5f;
    [SerializeField] float maxDistance = 10f;

    public GameObject Player { get; private set; }

    public Health PlayerHealth { get; private set; }

    public bool IsEnraged;

    public int NumberAttacksBetweenMechs = 3;
    public int NormalAttackCount;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = Player.GetComponent<Health>();
    }

    private void Start()
    {
        NavMeshAgent.updatePosition = false; // for manual control
        NavMeshAgent.updateRotation = false;
        IsEnraged = false;
        NormalAttackCount = 0;
        SwitchState(new TormentedSoulIdleState(this));
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
        if (Health.CurrentHealth < (Health.MaxHealth / 2))
        {
            transform.Find("EnragedGlow").gameObject.SetActive(true);
            IsEnraged = true;
        }
        SwitchState(new TormentedSoulImpactState(this));
    }

    private void HandleDie()
    {
        ProjectileShooter.ShooterDied();
        transform.Find("EnragedGlow").gameObject.SetActive(false);
        SwitchState(new TormentedSoulDeadState(this));
    }

    private void HandlePlayerDie()
    {
        SwitchState(new TormentedSoulIdleState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
    }

    public void PerformArrowRain()
    {
        for (int i = 0; i < numPortalsToSpawn; i++)
        {
            // Calculate a semi-random position behind the current object
            Vector3 spawnPosition = CalculateSpawnPosition();

            // Create a new portal at the calculated position
            GameObject portal = Instantiate(ArrowRainPortalPrefab, spawnPosition, Quaternion.identity);

            // Optionally, you can set the portal's rotation based on the current object's rotation
            portal.transform.rotation = transform.rotation;
        }
    }

    private Vector3 CalculateSpawnPosition()
    {
        float yOffsetMin = 6f;
        float yOffsetMax = 10f; 
        // Calculate a random distance within the specified range
        float distance = Random.Range(minDistance, maxDistance);

        // Calculate a random angle around the Y-axis
        float randomAngle = Random.Range(0f, 360f);

        // Convert the angle to radians
        float randomAngleRad = randomAngle * Mathf.Deg2Rad;

        // Calculate the spawn offset in the X-Z plane
        float xOffset = Mathf.Cos(randomAngleRad) * distance;
        float zOffset = Mathf.Sin(randomAngleRad) * distance;

        // Calculate a random Y offset within the specified range
        float randomYOffset = Random.Range(yOffsetMin, yOffsetMax);

        // Ensure the final Y value is above the calling GameObject's Y position by a certain amount
        float finalY = Mathf.Max(transform.position.y + randomYOffset, transform.position.y + yOffsetMin);

        // Calculate the final spawn position
        Vector3 spawnPosition = new Vector3(transform.position.x + xOffset, finalY, transform.position.z + zOffset);

        return spawnPosition;
    }



}
