using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    [SerializeField] TMP_Text energyMeterText;
    [SerializeField] Image energyMeter;
    [SerializeField] float regenSpeed;
    [SerializeField] float depleteSpeed;

    private float currentEnergyValue;
    private float energyRegenDelay;
    private PlayerStateMachine playerStateMachine;

    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;

    private void Start()
    {
        currentEnergyValue = 100;
        depleteSpeed = 5;
        regenSpeed = 10;
        energyRegenDelay = 3;

        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void RegenShield()
    {
        if ( currentEnergyValue < 100 )
        {
            currentEnergyValue += regenSpeed * Time.deltaTime;
            energyMeterText.color = Color.cyan;
            energyMeterText.text = ( (int)currentEnergyValue).ToString() + "%";
            energyMeter.fillAmount = currentEnergyValue / 100;
        }
        else
        {
            energyMeterText.color = Color.white;
        }
    }

    void Update()
    {
        if ( currentEnergyValue > 0 )
        {
            // Energy Meter UI Code
            if ( Input.GetMouseButton( 0 ) || Input.GetMouseButton( 1 ) )
            {
                CancelInvoke( "RegenShield" );
                currentEnergyValue -= depleteSpeed * Time.deltaTime; // How fast to drain energy gauge

                // Update info on remaining energy
                energyMeterText.color = Color.magenta;
                energyMeterText.text = ( (int)currentEnergyValue).ToString() + "%";
                energyMeter.fillAmount = currentEnergyValue / 100;
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
        }
        else
        {
            meleeShield.SetActive(false);
            playerStateMachine.Health.IsMeleeImmune = false;
        }

        if (!Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            // Check if only the right mouse button is down
            rangedShield.SetActive(true);
            playerStateMachine.Health.IsRangedImmune = true;
        }
        else
        {
            rangedShield.SetActive(false);
            playerStateMachine.Health.IsRangedImmune = false;
        }

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            // Check if both mouse buttons are down
            magicShield.SetActive(true);
            playerStateMachine.Health.IsMagicImmune = true;
        }
        else
        {
            magicShield.SetActive(false);
            playerStateMachine.Health.IsMagicImmune = false;
        }
    }

}
