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

    private float currentEnergyValue;
    private PlayerStateMachine playerStateMachine;

    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;

    private float meleeShieldActiveTime = 0f;
    private float rangedShieldActiveTime = 0f;
    private float magicShieldActiveTime = 0f;

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
            if ( Input.GetMouseButton( 0 ) || Input.GetMouseButton( 1 ) ) // if mouseclicks are detected, deplete gauge accordingly
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
        }

    }

    private void CheckShieldInputs()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            // Check if only the left mouse button is down
            meleeShield.SetActive(true);
            playerStateMachine.Health.IsMeleeImmune = true;
            meleeShieldActiveTime += Time.deltaTime;
        }
        else
        {
            meleeShield.SetActive(false);
            playerStateMachine.Health.IsMeleeImmune = false;
            meleeShieldActiveTime = 0f;
        }

        if (!Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            // Check if only the right mouse button is down
            rangedShield.SetActive(true);
            playerStateMachine.Health.IsRangedImmune = true;
            rangedShieldActiveTime += Time.deltaTime;
        }
        else
        {
            rangedShield.SetActive(false);
            playerStateMachine.Health.IsRangedImmune = false;
            rangedShieldActiveTime = 0f;
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            // Check if both mouse buttons are down
            magicShield.SetActive(true);
            playerStateMachine.Health.IsMagicImmune = true;
            magicShieldActiveTime += Time.deltaTime;
        }
        else
        {
            magicShield.SetActive(false);
            playerStateMachine.Health.IsMagicImmune = false;
            magicShieldActiveTime = 0f;
        }
    }

}
