using UnityEngine;

public class ShieldCollisions : MonoBehaviour
{
    public string typeTag;
    public PlayerStateMachine playerStateMachine;

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
            
    }
}
