using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    [System.Serializable]
    public class Shield
    {
        public GameObject shieldObject;
        public float rechargeDelay = 5.0f;
        public float gaugeRemaining = 100;
        public float rechargeTimer = 0.0f;
        public Slider gaugeSlider;
    }
    public Shield meleeShield;
    public Shield rangedShield;
    public Shield magicShield;

    void Update()
    {
        // Check if only the left mouse button is down
        meleeShield.shieldObject.SetActive(Input.GetMouseButton(0) && !Input.GetMouseButton(1) && meleeShield.gaugeRemaining > 0);
        // Check if only the right mouse button is down
        rangedShield.shieldObject.SetActive(Input.GetMouseButton(1) && !Input.GetMouseButton(0) && rangedShield.gaugeRemaining > 0);
        // Check if both mouse buttons are down
        magicShield.shieldObject.SetActive(Input.GetMouseButton(0) && Input.GetMouseButton(1) && magicShield.gaugeRemaining > 0);

        // Recharge the gauges with delay
        RechargeShield(meleeShield);
        RechargeShield(rangedShield);
        RechargeShield(magicShield);

        // Update UI elements with gauge values
        UpdateGaugeUI(meleeShield);
        UpdateGaugeUI(rangedShield);
        UpdateGaugeUI(magicShield);
    }

    private void RechargeShield(Shield shield)
    {
        if (!shield.shieldObject.activeSelf)
        {
            if (shield.rechargeTimer <= 0.0f)
            {
                // Recharge the gauge only when the rechargeTimer reaches 0
                if (shield.gaugeRemaining < 100)
                {
                    shield.gaugeRemaining += Time.deltaTime * 10;
                    // Cap the gauge at 100 if it exceeds
                    shield.gaugeRemaining = Mathf.Clamp(shield.gaugeRemaining, 0, 100);
                }
            }
            else
            {
                // Decrease the recharge timer
                shield.rechargeTimer -= Time.deltaTime;
            }
        }
        else
        {
            // Reset the recharge timer when the shield is active
            shield.rechargeTimer = shield.rechargeDelay;

            // Decrease the gauge when the shield is active
            if (shield.gaugeRemaining > 0)
            {
                shield.gaugeRemaining -= Time.deltaTime * 10;
                // Ensure the gauge doesn't go below 0
                shield.gaugeRemaining = Mathf.Clamp(shield.gaugeRemaining, 0, 100);
            }
        }
    }

    private void UpdateGaugeUI(Shield shield)
    {
        // Ensure the gaugeSlider reference is not null
        if (shield.gaugeSlider != null)
        {
            // Update the slider value based on the associated shield's gaugeRemaining
            shield.gaugeSlider.value = shield.gaugeRemaining / 100f;
        }
    }
}
