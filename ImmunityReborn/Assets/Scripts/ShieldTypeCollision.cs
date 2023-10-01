using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public string typeTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == typeTag)
            Destroy(other.gameObject);
    }
}
