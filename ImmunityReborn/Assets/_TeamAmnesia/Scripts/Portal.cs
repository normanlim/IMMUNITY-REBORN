using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // offset for when player exits portal
    private const float OffsetFromPortal = 1.5f;

    [SerializeField]
    private GameObject teleportTo;

    [SerializeField, Tooltip("Match player facing direction with portal facing direction")]
    private bool faceForwards = true;

    private CinemachineFreeLook freeLookCamera;

    private void Start()
    {
        freeLookCamera = Camera.main.GetComponent<CinemachineBrain>()
            .ActiveVirtualCamera
            .VirtualCameraGameObject
            .GetComponent<CinemachineFreeLook>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 offset = teleportTo.transform.forward * OffsetFromPortal;

            other.gameObject.transform.position = teleportTo.transform.position + offset;

            if (faceForwards)
            {
                freeLookCamera.m_XAxis.Value = teleportTo.transform.eulerAngles.y;
            }
        }
    }
}
