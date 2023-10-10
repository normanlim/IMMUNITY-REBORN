using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform

    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x - 5, player.position.y + 5, transform.position.z);
        }
    }
}
