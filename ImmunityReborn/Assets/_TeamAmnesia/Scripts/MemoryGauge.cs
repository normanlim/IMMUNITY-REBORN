
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGauge : MonoBehaviour
{

    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;
    public Slider memoryGaugeSlider;

    public Image memoryBall;
    public TMP_Text memoryUsesLeftText;

    private const int MemoryGaugeMinValue = 0;
    private const int MemoryGaugeMaxValue = 100;
    private int currentMemoryMeterValue;
    public int CurrentMemoryMeterValue 
    {
        get { return currentMemoryMeterValue; }
        private set { currentMemoryMeterValue = Mathf.Clamp( value, MemoryGaugeMinValue, MemoryGaugeMaxValue ); }

    }


    // Start is called before the first frame update
    void Start()
    {
        memoryBall.fillAmount = 0.0f;
        memoryUsesLeftText.text = (0).ToString();

        CurrentMemoryMeterValue = 0;
    }

    // Update is called once per frame
    void Update()
    {

        //  ****  Admin mode to just add gauge for testing/alpha demo **** //
        if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            EarnMemoryGauge( 50 );
        }
        //  *************************************************************** //

        UpdateMemoryGaugeSlider();

    }

    public void SpendMemoryGauge( int amtSpent )
    {
        CurrentMemoryMeterValue -= amtSpent;
        UpdateMemoryGaugeSlider();
    }

    public void EarnMemoryGauge ( int amtEarned )
    {
        CurrentMemoryMeterValue += amtEarned;
        UpdateMemoryGaugeSlider();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger zone
        if ( other.tag == "MemoryFragment" )
        {
            // Respawn the MemoryFragment at a random position within the respawn plane
            other.gameObject.GetComponent<MemoryFragment>().RespawnWithinPlaneBounds();
            EarnMemoryGauge( 10 );
        }
    }

    private void UpdateMemoryGaugeSlider()
    {
        memoryBall.fillAmount = (float)CurrentMemoryMeterValue / MemoryGaugeMaxValue;
        memoryUsesLeftText.text = (CurrentMemoryMeterValue / MemoryAttack.memAtkCost).ToString(); // rounds down by truncating

        memoryGaugeSlider.value = CurrentMemoryMeterValue;
    }

    private void UnlockMemoryAttack()
    {
        Debug.Log("Memory Attack unlocked!");
    }
}
