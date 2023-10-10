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
        Vector3 playerDirection = player.transform.position - transform.position;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = playerDirection;
        Destroy(projectile, 6f);
    }
}
