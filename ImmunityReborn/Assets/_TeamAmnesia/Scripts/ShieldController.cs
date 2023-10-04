using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject meleeShield;
    public GameObject rangedShield;
    public GameObject magicShield;

    void Update()
    {
        // Check if only the left mouse button is down
        meleeShield.SetActive(Input.GetMouseButton(0) && !Input.GetMouseButton(1));
        // Check if only the right mouse button is down
        rangedShield.SetActive(Input.GetMouseButton(1) && !Input.GetMouseButton(0));
        // Check if both mouse buttons are down
        magicShield.SetActive(Input.GetMouseButton(0) && Input.GetMouseButton(1));
    }
}
