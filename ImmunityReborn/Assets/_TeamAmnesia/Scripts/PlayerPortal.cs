using Cinemachine;
using UnityEngine;

public class PlayerPortal : MonoBehaviour
{
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
            other.gameObject.transform.position = teleportTo.transform.position;

            if (faceForwards)
            {
                freeLookCamera.m_XAxis.Value = teleportTo.transform.eulerAngles.y;
            }
        }
    }
}
