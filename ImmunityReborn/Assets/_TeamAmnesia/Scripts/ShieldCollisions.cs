using System;
using UnityEngine;

public class ShieldCollisions : MonoBehaviour
{
    public string typeTag; // Will let us know what shield got triggered
    public PlayerStateMachine playerStateMachine;

    public static event Action<GameObject> OnMeleeBlock;
    public static event Action<GameObject> OnRangedBlock;
    public static event Action<GameObject> OnMagicBlock;

    [field: SerializeField]
    public float ShieldKnockback { get; private set; }

    private float PerfectParryWindowDuration;

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject.CompareTag("EnemyAttack") && playerStateMachine.isShieldActive )
        {
            float shieldDuration = playerStateMachine.GetShieldActiveDuration();

            if ( shieldDuration < PerfectParryWindowDuration ) 
            {
                // Award extra gauge for last-second block
            }
            else
            {
                // Award regular gauge for regular block timing
            }

        }
    }

    private void OnTriggerEnter( Collider other )
    {
        // Notes for future implementation: 
        // Debug shows that this trigger gets called once on click (hits the player I guess)
        // Then 3 more times as 3 bullets touch the shield (to see which projectile should get deleted)

        Debug.Log( "OnTriggerEnter-ShieldCollision" );

        //if ( other.gameObject.tag == typeTag ) // Ranged projectile block code here
        //{
        //    Destroy( other.gameObject );
        //    playerStateMachine.MemoryGauge.EarnMemoryGauge( 10 );
        //}

        // GOTTA TEST BEFORE DELETING ABOVE CODE
        if ( typeTag == "RangedType" &&
             other.gameObject.TryGetComponent( out WeaponDamager rangedWeaponDamager ) &&
             rangedWeaponDamager.DamageType == DamageType.Ranged )
        {
            Destroy( other.gameObject );
            playerStateMachine.MemoryGauge.EarnMemoryGauge( 10 );
        }

        if ( typeTag == "MeleeType" && other.gameObject.TryGetComponent( out WeaponDamager weaponDamager ) && 
             weaponDamager.DamageType == DamageType.Melee )
        {
            GameObject attacker = weaponDamager.CharacterCollider.gameObject;

            if (attacker.TryGetComponent(out Health health))
            {
                health.DealDamage(0); // Hits the shield so it does 0 dmg 
            }

            if (attacker.TryGetComponent(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                forceReceiver.AddForce(direction * ShieldKnockback);
            }

            playerStateMachine.MemoryGauge.EarnMemoryGauge( 11 );

            OnMeleeBlock?.Invoke(attacker);
        }
        else if (typeTag == "RangedType" && other.gameObject.TryGetComponent(out weaponDamager) &&
            weaponDamager.DamageType == DamageType.Ranged)
        {
            GameObject attacker = weaponDamager.CharacterCollider.gameObject;
            OnRangedBlock?.Invoke(attacker);
        }
        else if (typeTag == "MagicType" && other.gameObject.TryGetComponent(out weaponDamager) &&
            weaponDamager.DamageType == DamageType.Magic)
        {
            GameObject attacker = weaponDamager.CharacterCollider.gameObject;
            OnMagicBlock?.Invoke(attacker);
        }
    }
}
