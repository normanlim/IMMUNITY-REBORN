
using UnityEngine;
using UnityEngine.UI;

public class MemoryGauge : MonoBehaviour
{
    public int numBlocksRequired = 5;
    public int numFragmentsRequired = 5;
    public int remainingBlocksRequired;
    public int remainingFragmentsRequired;
    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;
    public Slider memoryGaugeSlider;

    public float currentMeterVal;

    public int currentBlockCount;

    // Start is called before the first frame update
    void Start()
    {
        remainingBlocksRequired = numBlocksRequired;
        remainingFragmentsRequired = numFragmentsRequired;
        currentMeterVal = 0;
    }

    // Update is called once per frame
    void Update()
    {

        //  ****  Admin mode to just add gauge for testing/alpha demo **** //
        if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            if ( currentMeterVal+50 <= 100 )
            {
                EarnMemoryGauge( 50 );
            }
        }
        //  *************************************************************** //

        UpdateMemoryGaugeSlider();
        if (remainingBlocksRequired <= 0 && remainingFragmentsRequired <= 0)
        {
            UnlockMemoryAttack();
            // @TODO maybe destroy script to avoid running it after this point?
        }

    }

    public void SpendMemoryGauge( float amtSpent )
    {
        currentMeterVal -= amtSpent;
        UpdateMemoryGaugeSlider();
    }

    public void EarnMemoryGauge ( float amtEarned )
    {
        currentMeterVal += amtEarned;
    }

    private void UnlockMemoryAttack()
    {
        Debug.Log("Memory Attack unlocked!");
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger zone
        if (other.tag == "MemoryFragment")
        {
            // Respawn the MemoryFragment at a random position within the respawn plane
            other.gameObject.GetComponent<MemoryFragment>().RespawnWithinPlaneBounds();
            EarnMemoryGauge( 10 );

            UpdateMemoryGaugeSlider();
        }
    }

    private void UpdateMemoryGaugeSlider()
    {
        memoryGaugeSlider.value = currentMeterVal;
    }
}
