using UnityEngine;

public class MemoryFragment : MonoBehaviour
{
    public GameObject ground;

    public void RespawnWithinPlaneBounds()
    {
        // Get the boundaries of the respawn plane
        Renderer planeRenderer = ground.GetComponent<Renderer>();
        Vector3 minBounds = planeRenderer.bounds.min;
        Vector3 maxBounds = planeRenderer.bounds.max;

        // Generate a random position within the plane's bounds
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);

        // Set the position of the MemoryFragment to the random position within the plane
        transform.position = new Vector3(randomX, transform.position.y, randomZ);
    }
}
