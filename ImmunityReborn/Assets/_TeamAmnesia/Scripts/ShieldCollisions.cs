using UnityEngine;

public class ShieldCollisions : MonoBehaviour
{
    public string typeTag;
    public PlayerStateMachine playerStateMachine;

    [field: SerializeField]
    public float ShieldKnockback { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        // Notes for future implementation: 
        // Debug shows that this trigger gets called once on click (hits the player I guess)
        // Then 3 more times as 3 bullets touch the shield (to see which projectile should get deleted)
        if (other.gameObject.tag == typeTag)
        {
            Destroy(other.gameObject);
            playerStateMachine.MemoryGauge.EarnMemoryGauge( 10 );
        }
        
        if (other.gameObject.TryGetComponent(out WeaponDamager weaponDamager))
        {
            GameObject attacker = weaponDamager.CharacterCollider.gameObject;

            if (attacker.TryGetComponent(out Health health))
            {
                health.DealDamage(0);
            }

            if (attacker.TryGetComponent(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                forceReceiver.AddForce(direction * ShieldKnockback);
            }
        }
    }
}
