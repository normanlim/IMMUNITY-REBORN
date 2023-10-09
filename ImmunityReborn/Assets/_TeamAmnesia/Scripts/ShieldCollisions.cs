using UnityEngine;

public class ShieldCollisions : MonoBehaviour
{
    public string typeTag;
    public int numAttacksBlocked = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == typeTag)
        {
            Destroy(other.gameObject);
            numAttacksBlocked++;
        }
            
    }
}
