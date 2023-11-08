using UnityEngine;

public class ShieldCollisions : MonoBehaviour
{
    public string typeTag; // Will let us know what shield got triggered
    public PlayerStateMachine playerStateMachine;

    [field: SerializeField]
    public float ShieldKnockback { get; private set; }

    private void OnTriggerEnter( Collider other )
    {
        // Notes for future implementation: 
        // Debug shows that this trigger gets called once on click (hits the player I guess)
        // Then 3 more times as 3 bullets touch the shield (to see which projectile should get deleted)

        if ( other.gameObject.tag == typeTag ) // Ranged projectile block code here
        {
            Destroy( other.gameObject );
            playerStateMachine.MemoryGauge.EarnMemoryGauge( 10 );
        }
        
        if ( typeTag == "MeleeType" && 
             other.gameObject.TryGetComponent( out WeaponDamager weaponDamager ) && 
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
        }
    }
}
