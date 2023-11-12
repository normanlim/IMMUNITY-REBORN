using System.Collections.Generic;
using UnityEngine;

public class DOTDamager : MonoBehaviour
{

    [field: SerializeField]
    public DamageType DamageType { get; private set; }

    [field: SerializeField]
    public Collider CharacterCollider;

    [SerializeField] 
    private float PuddleLifetime;

    [SerializeField] 
    private float PuddleDelay;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag( "Player" );
    }

    void OnTriggerStay( Collider other )
    {
        if ( other.CompareTag( "Player" ) ) // Check if the collider is tagged as "Player"
        {
            // Apply damage based on the time between frames
            //playerHealth.TakeDamage( damagePerSecond * Time.deltaTime );
            //Debug.Log( "BOMBA EXPLODE - ONTRIGGERENTER" );
            //    if (other.TryGetComponent(out Health health))
            //    {
            //        health.DealDamage(damage, DamageType);
            //    }
        }
    }
    //private void OnEnable()
    //{
    //    collidedWith.Clear();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (collidedWith.Contains(other))
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        collidedWith.Add(other);
    //    }

    //    if (other.TryGetComponent(out Health health))
    //    {
    //        health.DealDamage(damage, DamageType);
    //    }

    //    if (other.TryGetComponent(out ForceReceiver forceReceiver))
    //    {
    //        Vector3 direction = (other.transform.position - transform.position).normalized;
    //        forceReceiver.AddForce(direction * knockback);
    //    }
    //}

}
