using UnityEngine;

/* This script includes behavior to aim and shoot a projectile at the player, with an aiming indicator before the shot.
 * For the behavior to work as intended, the following conditions must be fulfilled:
 * 1. This script is attached to a game object which will have its transform.position used as the origin point of the projectile attack.
 * 2. The game object has a LineRenderer component to use as the aiming indicator.
 * 3. The script is given a prefab projectile to use, and this prefab must have a WeaponDamager component to do damage.
 */
public class ProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float speed = 25f;
    public GameObject targetObject;
    // Target center Y from their base (otherwise it aims directly at the base of the target object)
    public float targetCenterY = 1f;
    private GameObject arrowObject;
    private Vector3 aimPoint;
    private float timeToHitTarget = -1f;
    private float lifetimeAfterPredictedHit = 2f;
    private LineRenderer aimingIndicator;
    void Start()
    {
        // Assume the target object is the player if unset on start
        if (targetObject == null)
            targetObject = GameObject.FindGameObjectWithTag("Player");
        aimingIndicator = GetComponent<LineRenderer>();
    }

    public bool TryAimingAtTarget()
    {
        // Calculate the direction from the projectile origin point to the player; assume transform position is origin point
        Vector3 originPoint = transform.position;
        // The position of the center of the target game object, which is what the projectile is aimed at
        Vector3 targetCenterPoint = targetObject.transform.position + Vector3.up * targetCenterY;
        // Check if self and the targetObject has a CharacterController or Rigidbody component
        Vector3 targetVelocity = GetVelocityFromRB(targetObject);
        Vector3 ownVelocity = GetVelocityFromRB(gameObject);
        Vector3 relativeDistance = targetCenterPoint - originPoint;
        Vector3 relativeVelocity = targetVelocity - ownVelocity;
        // Aim at the center of the target plus where it will be when the projectile hits based on target's velocity
        timeToHitTarget = AimAhead(relativeDistance, relativeVelocity, speed);
        aimPoint = targetCenterPoint + targetVelocity * timeToHitTarget;
        // Note that this indicator "deceives" the target because it's supposedly aimed at the target center point when its true aim point factors in target's velocity 
        aimingIndicator.enabled = true;
        // Set the positions of the aimingIndicator
        aimingIndicator.SetPosition(0, originPoint);
        aimingIndicator.SetPosition(1, targetCenterPoint);
        // In the future, logic to check line of sight can be added here
        return (timeToHitTarget > 0f);
    }

    /* Should be invoked as close to TryAimingAtTarget as possible for best accuracy. 
     * Alternatively, delaying the invocation of this method after invoking TryAimingAtTarget would aim at the last known trajectory of the target. 
     */
    public void FireAtTarget(int AttackDamage, float Knockback, Vector3 Offset = default)
    {
        if (timeToHitTarget > 0f)
        {
            // Disable aiming indicator
            aimingIndicator.enabled = false;
            // Assume transform position is origin point
            Vector3 originPoint = transform.position;
            // Create a projectile at the enemy's position and make it move towards the predicted target position
            arrowObject = Instantiate(projectilePrefab, originPoint, transform.rotation);
            WeaponDamager arrowWeaponDamager = arrowObject.GetComponent<WeaponDamager>();
            arrowWeaponDamager.SetDamage(AttackDamage, Knockback);

            Rigidbody arrowRigidbody = arrowObject.GetComponent<Rigidbody>();
            if (arrowRigidbody != null)
            {
                // Calculate the direction to the aimPoint plus offset
                Vector3 directionToAimPoint = aimPoint + Offset - originPoint;

                // Calculate the rotation to look at the aimPoint
                Quaternion rotationToAimPoint = Quaternion.LookRotation(directionToAimPoint);

                // Apply the rotation to the arrow prefab
                arrowObject.transform.rotation = rotationToAimPoint;

                // Set the velocity of the projectile to hit the aimPoint
                arrowRigidbody.velocity = directionToAimPoint.normalized * speed;

                // Destroy projectile a few seconds after the time it was supposed to hit the target
                Destroy(arrowObject, timeToHitTarget + lifetimeAfterPredictedHit);
            }
        }
    }

    // delta: relative position
    // vr: relative velocity
    // muzzleV: Speed of the bullet (muzzle velocity)
    // returns: Delta time when the projectile will hit, or -1 if impossible
    private float AimAhead(Vector3 delta, Vector3 vr, float muzzleV)
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

    // Call this when the shooter dies to clear the aiming indicator. In the future other logic may be added here for proper post-death cleanup.
    public void ShooterDied()
    {
        aimingIndicator.enabled = false;
    }

    /* Gets the game object's velocity by checking for a CharacterController first, then a Rigidbody.
     * If successful, extracts the velocity property from the component, otherwise return 0.
     * */
    private Vector3 GetVelocityFromRB(GameObject gameObject)
    {
        // Check if self and the targetObject has a CharacterController or Rigidbody component
        CharacterController characterController = gameObject.GetComponent<CharacterController>();
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        // If either target or self has a CharacterController, get its velocity; otherwise, get the Rigidbody's velocity
        return (characterController != null) ? characterController.velocity : (rigidbody != null) ? rigidbody.velocity : Vector3.zero;

    }
}
