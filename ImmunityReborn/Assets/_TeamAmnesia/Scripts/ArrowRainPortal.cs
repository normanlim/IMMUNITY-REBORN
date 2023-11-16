using UnityEngine;

public class ArrowRainPortal : MonoBehaviour
{
    [SerializeField] float minRandomRadius = 3f;
    [SerializeField] float maxRandomRadius = 8f;

    private ProjectileShooter ProjectileShooter;
    private float duration;
    private int Damage = 10;
    private float Knockback = 1f;
    private GameObject player;
    private GameObject randomTarget;
    private const float MinDuration = 2.5f;
    private const float MaxDuration = 4f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        duration = Random.Range(MinDuration, MaxDuration);
        ProjectileShooter = GetComponent<ProjectileShooter>();

        // Calculate a random direction and normalize it
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.Normalize();

        // Calculate a random radius
        float randomRadius = Random.Range(minRandomRadius, maxRandomRadius);

        // Calculate the random position around the player
        Vector3 randomPosition = player.transform.position + randomDirection * randomRadius;

        // Instantiate an empty GameObject as the target
        randomTarget = new GameObject("RandomTarget");
        randomTarget.transform.position = randomPosition;
        // Add a Rigidbody for gravity so the portal aims downwards over time
        Rigidbody targetRB = randomTarget.AddComponent<Rigidbody>();
        targetRB.useGravity = false;
        targetRB.velocity = GenerateRandomVector(-0.5f, 0.5f, -0.5f, 0.5f, -0.5f, 0.5f);
        ProjectileShooter.targetObject = randomTarget;
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0.0f)
        {
            ProjectileShooter.FireAtTarget(Damage, Knockback);
            Destroy(randomTarget);
            Destroy(gameObject);
        }
        else
        {
            ProjectileShooter.TryAimingAtTarget();
        }

        // Rotate the portal to face the player
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0f; // Keep the rotation only in the horizontal plane
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
    }

    private Vector3 GenerateRandomVector(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = Random.Range(minZ, maxZ);

        return new Vector3(randomX, randomY, randomZ);
    }
}
