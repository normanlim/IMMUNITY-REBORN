using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    [SerializeField] Image energyBall;
    [SerializeField] TMP_Text energyMeterText;
    [SerializeField] Image energyMeter; // extra
    [SerializeField] float energyRegenDelay = 2;
    [SerializeField] float regenSpeed = 10;
    [SerializeField] float depleteSpeed = 5;
    [SerializeField] GameObject SFXShielding;

    private float currentEnergyValue;
    private PlayerStateMachine playerStateMachine;

    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;

    private float meleeShieldActiveTime = 0f;
    private float rangedShieldActiveTime = 0f;
    private float magicShieldActiveTime = 0f;

    private GameObject ShieldingSoundObject;

    public float GetMeleeShieldActiveDuration()
    {
        return meleeShieldActiveTime;
    }

    public float GetRangedShieldActiveDuration()
    {
        return rangedShieldActiveTime;
    }

    public float GetMagicShieldActiveDuration()
    {
        return magicShieldActiveTime;
    }

    public void SetShieldRates(float delay, float regenSpeed, float depleteSpeed)
    {
        energyRegenDelay = delay;
        this.regenSpeed = regenSpeed;
        this.depleteSpeed = depleteSpeed;
    }

    public void EarnShieldGauge( int amtEarned )
    {
        currentEnergyValue += amtEarned;
    }

    private void Start()
    {
        currentEnergyValue = 100;

        playerStateMachine = GetComponent<PlayerStateMachine>();

        energyBall.fillAmount = currentEnergyValue / 100;
    }

    private void RegenShield()
    {
        if ( currentEnergyValue < 100 )
        {
            currentEnergyValue += regenSpeed * Time.deltaTime;
            energyMeterText.color = Color.cyan;
            energyMeterText.text = ( (int)currentEnergyValue).ToString() + "%";
            energyMeter.fillAmount = currentEnergyValue / 100;

            energyBall.fillAmount = currentEnergyValue / 100;
        }
        else
        {
            energyMeterText.color = Color.white;
        }
    }

    void Update()
    {
        //Debug.Log( shieldActiveTime );
        if ( currentEnergyValue > 0 )
        {
            // Energy Meter UI Code
            if ( ( Input.GetMouseButton( 0 ) || Input.GetMouseButton( 1 ) ) && !playerStateMachine.Health.IsPlayerDead && !PauseController.isPaused )
            {
                CancelInvoke( "RegenShield" );
                currentEnergyValue -= depleteSpeed * Time.deltaTime; // How fast to drain energy gauge

                // Update info on remaining energy
                energyMeterText.color = Color.magenta;
                energyMeterText.text = ( (int)currentEnergyValue).ToString() + "%";
                energyMeter.fillAmount = currentEnergyValue / 100;

                energyBall.fillAmount = currentEnergyValue / 100;
            }
            else
            {
                Invoke( "RegenShield", energyRegenDelay );
                // Cancel looping shield sound
                if (ShieldingSoundObject)
                    PlaySFX.StopLoopedAudio(ShieldingSoundObject, this);
            }

            CheckShieldInputs();
        }
        else
        {
            // All shields should be disabled once drained of resources
            magicShield.SetActive(false);
            rangedShield.SetActive(false);
            meleeShield.SetActive(false);

            // Begin Delayed Shield Regen (only thing they can do) 
            Invoke( "RegenShield", energyRegenDelay );
            energyMeterText.text = "";

            // Cancel looping shield sound
            if (ShieldingSoundObject)
                PlaySFX.StopLoopedAudio(ShieldingSoundObject, this);
        }

    }

    private void CheckShieldInputs()
    {
        if ( Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !playerStateMachine.Health.IsPlayerDead && !PauseController.isPaused )
        {
            // Check if only the left mouse button is down
            meleeShield.SetActive(true);
            playerStateMachine.Health.IsMeleeImmune = true;
            meleeShieldActiveTime += Time.deltaTime;

            // Play shielding sound on loop
            if ( !ShieldingSoundObject )
                ShieldingSoundObject = PlaySFX.PlayWithLoop( SFXShielding, transform );
        }
        else
        {
            meleeShield.SetActive(false);
            playerStateMachine.Health.IsMeleeImmune = false;
            meleeShieldActiveTime = 0f;
        }

        if ( !Input.GetMouseButton(0) && Input.GetMouseButton(1) && !playerStateMachine.Health.IsPlayerDead && !PauseController.isPaused )
        {
            // Check if only the right mouse button is down
            rangedShield.SetActive(true);
            playerStateMachine.Health.IsRangedImmune = true;
            rangedShieldActiveTime += Time.deltaTime;

            // Play shielding sound on loop
            if ( !ShieldingSoundObject )
                ShieldingSoundObject = PlaySFX.PlayWithLoop( SFXShielding, transform );
        }
        else
        {
            rangedShield.SetActive(false);
            playerStateMachine.Health.IsRangedImmune = false;
            rangedShieldActiveTime = 0f;
        }

        if ( Input.GetMouseButton(0) && Input.GetMouseButton(1) && !playerStateMachine.Health.IsPlayerDead && !PauseController.isPaused )
        {
            // Check if both mouse buttons are down
            magicShield.SetActive(true);
            playerStateMachine.Health.IsMagicImmune = true;
            magicShieldActiveTime += Time.deltaTime;

            // Play shielding sound on loop
            if ( !ShieldingSoundObject )
                ShieldingSoundObject = PlaySFX.PlayWithLoop( SFXShielding, transform );
        }
        else
        {
            magicShield.SetActive(false);
            playerStateMachine.Health.IsMagicImmune = false;
            magicShieldActiveTime = 0f;
        }
    }

}
