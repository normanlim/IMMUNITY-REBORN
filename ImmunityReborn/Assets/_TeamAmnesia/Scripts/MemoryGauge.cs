using UnityEngine;

public class MemoryGauge : MonoBehaviour
{
    public int numBlocksRequired = 50;
    public int numFragmentsRequired = 50;
    public int remainingBlocksRequired;
    public int remainingFragmentsRequired;
    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;
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

    }
}
