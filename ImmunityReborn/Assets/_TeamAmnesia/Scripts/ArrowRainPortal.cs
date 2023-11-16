using UnityEngine;

public class ArrowRainPortal : MonoBehaviour
{
    [SerializeField] float maxRandomRadius = 2.0f;
    [SerializeField] float minOrbitSpeed = 0.5f;
    [SerializeField] float maxOrbitSpeed = 2.0f;
    [SerializeField] float MinLifeTimeDuration = 2.2f;
    [SerializeField] float MaxLifeTimeDuration = 4.8f;
    [SerializeField] float MinTrackingDuration = 0.4f;

    private ProjectileShooter ProjectileShooter;
    private float duration;
    private float trackingDuration;
    private int Damage = 20;
    private float Knockback = 1f;
    private GameObject player;
    private GameObject randomTarget;
    private float orbitOffset;
    private float radius;
    private float orbitSpeed;
    private int orbitDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        duration = Random.Range(MinLifeTimeDuration, MaxLifeTimeDuration);
        ProjectileShooter = GetComponent<ProjectileShooter>();

        // Set a unique orbit offset for each portal
        orbitOffset = Random.Range(0f, 360f);

        // Set a unique orbit speed for each portal
        orbitSpeed = Random.Range(minOrbitSpeed, maxOrbitSpeed);

        // Randomize the orbit direction (-1 or 1 for clockwise or counterclockwise)
        orbitDirection = Random.value < 0.5f ? -1 : 1;

        // Set tracking duration, target detaches from player object after this time
        trackingDuration = Random.Range(MinTrackingDuration, MaxLifeTimeDuration);
        SpawnRandomTarget();
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
        if (duration < trackingDuration)
        {
            // Orbit the randomTarget around the player
            OrbitAroundPlayer();
        } else
        {
            // Detach randomTarget from its parent, maintaining its world position
            randomTarget.transform.SetParent(null, true);
        }

        // Rotate the portal to face the player
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0f; // Keep the rotation only in the horizontal plane
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }

    }

    void SpawnRandomTarget()
    {
        radius = Random.Range(0, maxRandomRadius);
        // Instantiate an empty GameObject as the target, set its position, and make it a child of the player
        randomTarget = new GameObject("RandomTarget");
        randomTarget.transform.position = Random.onUnitSphere * radius + player.transform.position + Vector3.up;
        randomTarget.transform.parent = player.transform;
        ProjectileShooter.targetObject = randomTarget;
        ProjectileShooter.targetCenterY = 0;
    }

    void OrbitAroundPlayer()
    {
        // Calculate the new position for the randomTarget in a circular orbit around the player
        float angle = Time.time * orbitSpeed * orbitDirection + orbitOffset;
        Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * radius;
        randomTarget.transform.position = player.transform.position + Vector3.up + offset;
    }
}
