using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [field: SerializeField]
    public Health Health { get; private set; }

    [field: SerializeField]
    public GameObject DeathEffect { get; private set; }

    [field: SerializeField]
    public GameObject TakeDamageEffect { get; private set; }

    [field: SerializeField]
    public EnemySpawnManager EnemySpawnManager { get; private set; }

    [field: SerializeField]
    public EnemySpawnManager.SpawnData Summons { get; private set; }

    public float SummonInterval = 8f;


    // Start is called before the first frame update
    void Start()
    {
        EnemySpawnManager = FindFirstObjectByType<EnemySpawnManager>();
        InvokeRepeating("SummonEnemy", SummonInterval / 2, SummonInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void HandleTakeDamage()
    {
        Instantiate(TakeDamageEffect, transform);
    }

    private void HandleDie()
    {
        Instantiate(DeathEffect, transform);
        Destroy(gameObject, DeathEffect.GetComponent<ParticleSystem>().main.duration / 4);
    }

    private void SummonEnemy()
    {
        EnemySpawnManager.SpawnEnemiesNow(Summons);
    }
}
