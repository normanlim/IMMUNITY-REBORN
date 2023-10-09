using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    [SerializeField] TMP_Text energyMeterText;
    [SerializeField] Image energyMeter;
    [SerializeField] float regenSpeed;

    private float currentEnergyValue;
    private float energyRegenDelay;

    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;

    private void Start()
    {
        currentEnergyValue = 100;
        regenSpeed = 10;
        energyRegenDelay = 3;
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
                currentEnergyValue -= regenSpeed * Time.deltaTime;
                energyMeterText.color = Color.magenta;
                energyMeterText.text = ( (int)currentEnergyValue).ToString() + "%";
                energyMeter.fillAmount = currentEnergyValue / 100;
            }
            else
            {
                Invoke( "RegenShield", energyRegenDelay );
            }

            // Check if only the left mouse button is down
            magicShield.SetActive( Input.GetMouseButton( 0 ) && !Input.GetMouseButton( 1 ) );
            // Check if only the right mouse button is down
            rangedShield.SetActive( !Input.GetMouseButton( 0 ) && Input.GetMouseButton( 1 ) );
            // Check if both mouse buttons are down
            meleeShield.SetActive( Input.GetMouseButton( 0 ) && Input.GetMouseButton( 1 ) );

        }
        else
        {
            // All shields should be disabled once drained of resources
            magicShield.SetActive( false );
            rangedShield.SetActive(false);
            meleeShield.SetActive( false );

            // Begin Delayed Shield Regen (only thing they can do) 
            Invoke( "RegenShield", energyRegenDelay );
            energyMeterText.text = "";
        }

    }

}
