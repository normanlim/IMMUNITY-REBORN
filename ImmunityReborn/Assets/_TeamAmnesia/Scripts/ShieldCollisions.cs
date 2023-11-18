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


    private void OnTriggerEnter( Collider other )
    {
        // If we actually get a Trigger from something that can do damage, not an enemy model moving into Shield
        if ( other.gameObject.TryGetComponent( out WeaponDamager weaponDamager ) ) 
        {
            // Get the attacker data for counterattack calculations
            GameObject attacker = weaponDamager.CharacterCollider.gameObject;

            // Melee Damage + Melee Shields = Reward Player For Melee Mechanic Handling
            if ( weaponDamager.DamageType == DamageType.Melee && shieldTypeCollided == "MeleeType" )
            {

                if ( attacker.TryGetComponent( out Health health ) )
                {
                    health.DealDamage( 0 ); // Hits the shield so it does 0 dmg 
                }

                if ( attacker.TryGetComponent( out ForceReceiver forceReceiver ) )
                {
                    Vector3 direction = (other.transform.position - transform.position).normalized;
                    forceReceiver.AddForce( direction * ShieldKnockback );
                }
                OnMeleeBlock?.Invoke( attacker );
                HandleMeleeBlockingReward();
            }


            // Ranged Damage + Ranged Shields = Reward Player For Ranged Mechanic Handling
            if ( weaponDamager.DamageType == DamageType.Ranged && shieldTypeCollided == "RangedType" )
            {
                Destroy( other.gameObject );
                OnRangedBlock?.Invoke( attacker );
                HandleRangedBlockingReward();
            }


            // Magic Damage + Magic Shields = Reward Player For Magic Mechanic Handling
            if ( weaponDamager.DamageType == DamageType.Magic && shieldTypeCollided == "MagicType" )
            {
                OnMagicBlock?.Invoke( attacker );
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

