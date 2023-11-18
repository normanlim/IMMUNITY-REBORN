using System;
using UnityEngine;

public class ShieldCollisions : MonoBehaviour
{
    public string shieldTypeCollided; // Will let us know what shield got triggered
    public PlayerStateMachine playerStateMachine;

    public static event Action<GameObject> OnMeleeBlock;
    public static event Action<GameObject> OnRangedBlock;
    public static event Action<GameObject> OnMagicBlock;

    [field: SerializeField]
    public float ShieldKnockback { get; private set; }

    [SerializeField]
    private float PerfectParryWindowDuration;

    private void Start()
    {
        playerStateMachine = GetComponentInParent<PlayerStateMachine>();
    }

    //private void OnCollisionEnter( Collision collision )
    //{
    //    Debug.Log( "X" );
    //    if ( collision.gameObject.CompareTag( "EnemyAttack" ) && playerStateMachine.ShieldController.isShieldActive )
    //    {
    //        float shieldDuration = playerStateMachine.ShieldController.GetShieldActiveDuration();

    //        if ( shieldDuration < PerfectParryWindowDuration )
    //        {
    //            // Award extra gauge for last-second block
    //        }
    //        else
    //        {
    //            // Award regular gauge for regular block timing
    //        }

    //    }
    //}

    private void OnTriggerEnter( Collider other )
    {
        // Notes for future implementation: 

        if ( other.gameObject.TryGetComponent( out WeaponDamager weaponDamager ) ) // If we actually get a Trigger from something that can do damage, not an enemy model
        {
            //Debug.Log( "We Got hit boiz" );

            // Melee Damage + Melee Shields = Reward Player For Melee Mechanic Handling
            if ( weaponDamager.DamageType == DamageType.Melee && shieldTypeCollided == "MeleeType" )
            {
                GameObject attacker = weaponDamager.CharacterCollider.gameObject;

                if ( attacker.TryGetComponent( out Health health ) )
                {
                    health.DealDamage( 0 ); // Hits the shield so it does 0 dmg 
                }

                if ( attacker.TryGetComponent( out ForceReceiver forceReceiver ) )
                {
                    Vector3 direction = (other.transform.position - transform.position).normalized;
                    forceReceiver.AddForce( direction * ShieldKnockback );
                }

                HandleMeleeBlockingReward();
            }


            // Ranged Damage + Ranged Shields = Reward Player For Ranged Mechanic Handling
            if ( weaponDamager.DamageType == DamageType.Ranged && shieldTypeCollided == "RangedType" )
            {
                Destroy( other.gameObject );
                HandleRangedBlockingReward();
            }


            // Magic Damage + Magic Shields = Reward Player For Magic Mechanic Handling
            if ( weaponDamager.DamageType == DamageType.Magic && shieldTypeCollided == "MagicType" )
            {
                HandleMagicBlockingReward();
            }
        }

    }

    private void HandleRangedBlockingReward()
    {
        float rangedShieldActiveDur = playerStateMachine.ShieldController.GetRangedShieldActiveDuration();

        if ( rangedShieldActiveDur <= PerfectParryWindowDuration )
        {
            // Award extra gauge for last-second block
            playerStateMachine.MemoryGauge.EarnMemoryGauge( 10 );

            //Debug.Log( "Perfect Parry (BONUS) - Shield active for: " + shieldDuration + " < PerfectWindowDuration: " + PerfectParryWindowDuration );
        }
        else
        {
            // Award regular gauge for regular block timing
            playerStateMachine.MemoryGauge.EarnMemoryGauge( 2 );

            //Debug.Log( "LATE Parry - Shield active for: " + shieldDuration + " > PerfectWindowDuration: " + PerfectParryWindowDuration );
        }
    }

    private void HandleMeleeBlockingReward()
    {
        float meleeShieldActiveDur = playerStateMachine.ShieldController.GetMeleeShieldActiveDuration();

        if ( meleeShieldActiveDur <= PerfectParryWindowDuration )
        {
            // Award extra gauge for last-second block
            playerStateMachine.MemoryGauge.EarnMemoryGauge( 20 );
        }
        else
        {
            // Award regular gauge for regular block timing
            playerStateMachine.MemoryGauge.EarnMemoryGauge( 10 );
        }
    }

    private void HandleMagicBlockingReward()
    {
        float magicShieldActiveDur = playerStateMachine.ShieldController.GetMagicShieldActiveDuration();

        if ( magicShieldActiveDur <= PerfectParryWindowDuration )
        {
            // Award extra gauge for last-second block
            playerStateMachine.MemoryGauge.EarnMemoryGauge( 50 );
        }
        else
        {
            // Award regular gauge for regular block timing
            playerStateMachine.MemoryGauge.EarnMemoryGauge( 10 );
        }
    }
}

