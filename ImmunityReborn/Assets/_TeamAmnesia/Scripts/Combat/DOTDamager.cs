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

    private GameObject player;

    private float damagePerSecond = 1.0f;
    private float accumulatedDamage = 0.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag( "Player" );
        Destroy( gameObject, PuddleLifetime );
    }

    void OnTriggerStay( Collider other )
    {
        if ( other.CompareTag( "Player" ) ) // Check if the collider is tagged as "Player"
        {
            // Calculate the damage for this frame and accumulate it
            accumulatedDamage += damagePerSecond * Time.deltaTime;

            if ( accumulatedDamage > 1f && other.TryGetComponent( out Health health ) )
            {
                int damageFromDotToApply = Mathf.FloorToInt( accumulatedDamage ); // apply damage in whole integers only
                health.DealDamage( damageFromDotToApply, DamageType );

                accumulatedDamage -= damageFromDotToApply;
            }

        }
    }

}
