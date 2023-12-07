using UnityEngine;

public class PlayerPortal : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
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
