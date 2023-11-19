using System.Collections.Generic;
using UnityEngine;

public class DOTDamager : MonoBehaviour
{

    [field: SerializeField]
    public DamageType DamageType { get; private set; }

    [SerializeField] 
    private float PuddleLifetime;

    [SerializeField]
    private float puddleDamagePerSecond;

    private float accumulatedDamage = 0.0f;

    void Start()
    {
        Destroy( gameObject, PuddleLifetime );
    }

    void OnTriggerStay( Collider other )
    {
        if ( other.CompareTag( "Player" ) ) // Check if the collider is tagged as "Player"
        {
            // Calculate the damage for this frame and accumulate it
            accumulatedDamage += puddleDamagePerSecond * Time.deltaTime;

            if ( accumulatedDamage > 1f && other.TryGetComponent( out Health health ) )
            {
                int damageFromDotToApply = Mathf.FloorToInt( accumulatedDamage ); // apply damage in whole integers only
                health.DealDamage( damageFromDotToApply, DamageType );

                accumulatedDamage -= damageFromDotToApply;
            }

        }
    }

}
