using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public string typeTag;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == typeTag)
            Destroy(other.gameObject);
    }
}
