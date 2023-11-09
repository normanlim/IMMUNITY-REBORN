using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject rangedArrowPrefab;
    public float speed = 25f;
    private GameObject player;
    private GameObject arrowObject;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void FireAtPlayer(int AttackDamage, float Knockback)
    {
        // Calculate the direction from the enemy to the player
        Vector3 relativeDistance = player.transform.position - transform.position;
        Vector3 relativeVelocity = player.GetComponent<CharacterController>().velocity - GetComponentInParent<CharacterController>().velocity;

        float deltaTime = AimAhead(relativeDistance, relativeVelocity, speed);

        if (deltaTime > 0f)
        {
            Vector3 aimPoint = player.transform.position + player.GetComponent<CharacterController>().velocity * deltaTime;
            // Create a projectile at the enemy's position and make it move towards the predicted player position
            arrowObject = Instantiate(rangedArrowPrefab, transform.position + Vector3.up * 1.0f, transform.rotation);
            WeaponDamager arrowWeaponDamager = arrowObject.GetComponent<WeaponDamager>();
            arrowWeaponDamager.SetDamage(AttackDamage, Knockback);
            Rigidbody arrowRigidbody = arrowObject.GetComponent<Rigidbody>();
            if (arrowRigidbody != null)
            {
                // Calculate the direction to the aimPoint
                Vector3 directionToAimPoint = aimPoint - transform.position;

                // Calculate the rotation to look at the aimPoint
                Quaternion rotationToAimPoint = Quaternion.LookRotation(directionToAimPoint);

                // Apply the rotation to the arrow prefab
                arrowObject.transform.rotation = rotationToAimPoint;

                // Set the velocity of the projectile to hit the aimPoint
                // @TODO ADD DELAY BEFORE SHOOTING
                arrowRigidbody.velocity = directionToAimPoint.normalized * speed;

                // Destroy projectile a few seconds after the time it was supposed to hit the player
                Destroy(arrowObject, deltaTime + 3f);
            }
        }
    }

    // delta: relative position
    // vr: relative velocity
    // muzzleV: Speed of the bullet (muzzle velocity)
    // returns: Delta time when the projectile will hit, or -1 if impossible
    float AimAhead(Vector3 delta, Vector3 vr, float muzzleV)
    {
        // Quadratic equation coefficients a*t^2 + b*t + c = 0
        float a = Vector3.Dot(vr, vr) - muzzleV * muzzleV;
        float b = 2f * Vector3.Dot(vr, delta);
        float c = Vector3.Dot(delta, delta);

        float desc = b * b - 4f * a * c;

        // If the discriminant is negative, then there is no solution
        if (desc > 0f)
        {
            return 2f * c / (Mathf.Sqrt(desc) - b);
        }
        else
        {
            return -1f;
        }
    }
}
