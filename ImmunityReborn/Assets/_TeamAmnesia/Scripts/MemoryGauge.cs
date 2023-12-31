
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGauge : MonoBehaviour
{

    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;
    public Slider memoryGaugeSlider; // extra

    public PlayerStateMachine stateMachine;

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

        //  ****  God mode to just add gauge for testing/alpha/beta demo **** //
        if ( Input.GetKeyDown( KeyCode.Space ) && stateMachine.IsGodModeActive )
        {
            EarnMemoryGauge( 100 );
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
            EarnMemoryGauge( 100 );
            Destroy(other.gameObject);
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
