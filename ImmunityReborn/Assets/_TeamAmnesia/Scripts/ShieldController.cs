using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    [SerializeField] TMP_Text energyMeterText;
    [SerializeField] Image shieldMeter;
    [SerializeField] float regenSpeed;

    private float currentShieldValue;
    private float regenShieldTimer;

    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;

    private void Start()
    {
        currentShieldValue = 100;
        regenSpeed = 10;
        regenShieldTimer = 3;
    }

    private void RegenShield()
    {
        if ( currentShieldValue < 100 )
        {
            currentShieldValue += regenSpeed * Time.deltaTime;
            energyMeterText.color = Color.cyan;
            energyMeterText.text = ( (int)currentShieldValue).ToString() + "%";
            shieldMeter.fillAmount = currentShieldValue / 100;
        }
        else
        {
            energyMeterText.color = Color.white;
        }
    }

    void Update()
    {
        if ( currentShieldValue > 0 )
        {
            // Energy Meter UI Code
            if ( Input.GetMouseButton( 0 ) || Input.GetMouseButton( 1 ) )
            {
                CancelInvoke( "RegenShield" );
                currentShieldValue -= regenSpeed * Time.deltaTime;
                energyMeterText.color = Color.magenta;
                energyMeterText.text = ( (int)currentShieldValue).ToString() + "%";
                shieldMeter.fillAmount = currentShieldValue / 100;
            }
            else
            {
                Invoke( "RegenShield", regenShieldTimer );
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
            Invoke( "RegenShield", regenShieldTimer );
            energyMeterText.text = "";
        }

    }

}
