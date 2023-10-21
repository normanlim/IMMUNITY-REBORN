using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float speed = 10f;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ShootAtPlayer", 1f, 3f);
    }

    private void ShootAtPlayer()
    {
        // Calculate the direction from the enemy to the player
        Vector3 playerDirection = player.transform.position - transform.position;

        // Rotate the enemy to face the player
        transform.LookAt(player.transform);

        // Create a projectile and make it move towards the player
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = playerDirection.normalized * speed;
        Destroy(projectile, 9f);
    }
}
