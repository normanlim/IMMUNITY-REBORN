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
    [SerializeField]
    private int PerfectParryShieldReward = 10;
    [SerializeField]
    private int PerfectParryMemoryReward = 10;
    [SerializeField]
    private GameObject SFXPerfectParry;
    [SerializeField]
    private int RegularParryMemoryReward = 1;
    [SerializeField]
    private GameObject SFXRegularParry;

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
                HandleMeleeBlockingReward( attacker );
            }


            // Ranged Damage + Ranged Shields = Reward Player For Ranged Mechanic Handling
            if ( weaponDamager.DamageType == DamageType.Ranged && shieldTypeCollided == "RangedType" )
            {
                Destroy( other.gameObject );
                HandleRangedBlockingReward( attacker );
            }


            // Magic Damage + Magic Shields = Reward Player For Magic Mechanic Handling
            if ( weaponDamager.DamageType == DamageType.Magic && shieldTypeCollided == "MagicType" )
            {

                HandleMagicBlockingReward( attacker );
            }
        }

    }

    private void HandleRangedBlockingReward( GameObject attacker )
    {
        float rangedShieldActiveDur = playerStateMachine.ShieldController.GetRangedShieldActiveDuration();

        if ( rangedShieldActiveDur <= PerfectParryWindowDuration )
        {
            // Award extra gauge for last-second block
            playerStateMachine.MemoryGauge.EarnMemoryGauge(PerfectParryMemoryReward);
            playerStateMachine.ShieldController.EarnShieldGauge(PerfectParryShieldReward);
            OnRangedBlock?.Invoke( attacker );
            PlaySFX.PlayThenDestroy(SFXPerfectParry, transform);
            //Debug.Log( "PERFECT PARRY (BONUS) - Shield active: " + rangedShieldActiveDur + " / " + PerfectParryWindowDuration );
        }
        else
        {
            // Award regular gauge for regular block timing
            playerStateMachine.MemoryGauge.EarnMemoryGauge(RegularParryMemoryReward);
            PlaySFX.PlayThenDestroy(SFXRegularParry, transform);
        }
    }

    private void HandleMeleeBlockingReward( GameObject attacker )
    {
        float meleeShieldActiveDur = playerStateMachine.ShieldController.GetMeleeShieldActiveDuration();

        if ( meleeShieldActiveDur <= PerfectParryWindowDuration )
        {
            // Award extra gauge for last-second block
            playerStateMachine.MemoryGauge.EarnMemoryGauge(PerfectParryMemoryReward);
            playerStateMachine.ShieldController.EarnShieldGauge(PerfectParryShieldReward);
            OnMeleeBlock?.Invoke( attacker );
            PlaySFX.PlayThenDestroy(SFXPerfectParry, transform);
            //Debug.Log( "PERFECT PARRY (BONUS) - Shield active: " + meleeShieldActiveDur + " / " + PerfectParryWindowDuration );
        }
        else
        {
            // Award regular gauge for regular block timing
            playerStateMachine.MemoryGauge.EarnMemoryGauge(RegularParryMemoryReward);
            PlaySFX.PlayThenDestroy(SFXRegularParry, transform);
        }
    }

    private void HandleMagicBlockingReward( GameObject attacker )
    {
        float magicShieldActiveDur = playerStateMachine.ShieldController.GetMagicShieldActiveDuration();

        if ( magicShieldActiveDur <= PerfectParryWindowDuration )
        {
            // Award extra gauge for last-second block
            playerStateMachine.MemoryGauge.EarnMemoryGauge( PerfectParryMemoryReward );
            playerStateMachine.ShieldController.EarnShieldGauge( PerfectParryShieldReward );
            OnMagicBlock?.Invoke( attacker );
            PlaySFX.PlayThenDestroy(SFXPerfectParry, transform);
            //Debug.Log( "PERFECT PARRY (BONUS) - Shield active: " + magicShieldActiveDur + " / " + PerfectParryWindowDuration );
        }
        else
        {
            // Award regular gauge for regular block timing
            playerStateMachine.MemoryGauge.EarnMemoryGauge(RegularParryMemoryReward);
            PlaySFX.PlayThenDestroy(SFXRegularParry, transform);
        }
    }
}
