using UnityEngine;

public class PlayerPortal : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private GameObject PortalSpawnSND;
    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        PlaySFX.PlayThenDestroy(PortalSpawnSND, transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.StartNextLevel();
            Destroy(gameObject);
        }       
    }
}
