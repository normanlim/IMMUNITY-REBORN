
using UnityEngine;
using UnityEngine.UI;

public class MemoryGauge : MonoBehaviour
{
    public int numBlocksRequired = 20;
    public int numFragmentsRequired = 20;
    public int remainingBlocksRequired;
    public int remainingFragmentsRequired;
    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;
    public Slider memoryGaugeSlider;
    // Start is called before the first frame update
    void Start()
    {
        remainingBlocksRequired = numBlocksRequired;
        remainingFragmentsRequired = numFragmentsRequired;
    }

    // Update is called once per frame
    void Update()
    {
        remainingBlocksRequired = Mathf.Max(0, numBlocksRequired
            - meleeShield.GetComponent<ShieldCollisions>().numAttacksBlocked
            - rangedShield.GetComponent<ShieldCollisions>().numAttacksBlocked
            - magicShield.GetComponent<ShieldCollisions>().numAttacksBlocked);
        UpdateMemoryGaugeSlider();
        if (remainingBlocksRequired <= 0 && remainingFragmentsRequired <= 0)
        {
            UnlockMemoryAttack();
            // @TODO maybe destroy script to avoid running it after this point?
        }

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
            remainingFragmentsRequired = Mathf.Max(0, remainingFragmentsRequired - 1);
            UpdateMemoryGaugeSlider();
        }
    }

    private void UpdateMemoryGaugeSlider()
    {
        memoryGaugeSlider.value = (float) ( numBlocksRequired - remainingBlocksRequired 
                                            + numFragmentsRequired - remainingFragmentsRequired) * 100 / 
                                            ( numBlocksRequired + numFragmentsRequired );
    }
}
