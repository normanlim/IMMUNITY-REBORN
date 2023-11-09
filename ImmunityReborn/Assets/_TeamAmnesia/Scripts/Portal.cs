using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private static GameObject teleportFrom;

    [SerializeField]
    private GameObject teleportTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && teleportFrom == null)
        {
            teleportFrom = gameObject;

            if (teleportTo == teleportFrom)
            {
                return;
            }

            other.gameObject.transform.position = teleportTo.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameObject != teleportFrom)
        {
            teleportFrom = null;
        }
    }
}
