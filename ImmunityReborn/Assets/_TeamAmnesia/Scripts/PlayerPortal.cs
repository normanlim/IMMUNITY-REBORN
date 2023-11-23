using Cinemachine;
using UnityEngine;

public class PlayerPortal : MonoBehaviour
{
    [SerializeField]
    private GameObject teleportTo;

    [SerializeField, Tooltip("Match player facing direction with portal facing direction")]
    private bool faceForwards = true;

    private CinemachineFreeLook freeLookCamera;
    private LevelManager levelManager;
    private Cloth playerCape;

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        freeLookCamera = Camera.main.GetComponent<CinemachineBrain>()
            .ActiveVirtualCamera
            .VirtualCameraGameObject
            .GetComponent<CinemachineFreeLook>();
        playerCape = FindFirstObjectByType<Cloth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCape.enabled = false;
            other.gameObject.transform.position = teleportTo.transform.position;

            if (faceForwards)
            {
                freeLookCamera.m_XAxis.Value = teleportTo.transform.eulerAngles.y;
            }
            playerCape.enabled = true;
            levelManager.StartNextLevel();
            Destroy(transform.parent.gameObject);// The empty parent is used to set a local y offset, delete the parent
        }
    }
}
