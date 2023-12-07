using System.Linq;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [field: SerializeField]
    public Health Health { get; private set; }

    [field: SerializeField]
    public GameObject PortalPrefab { get; private set; }

    [field: SerializeField]
    public GameObject DeathEffect { get; private set; }

    [field: SerializeField]
    public GameObject TakeDamageEffect { get; private set; }

    [field: SerializeField]
    public SpawnManager EnemySpawnManager { get; private set; }

    [field: SerializeField]
    public SpawnManager.SpawnData Summons { get; private set; }

    [field: SerializeField, Tooltip("How much to damage self by for every enemy spawned")]
    public int CostPerEnemy { get; private set; }

    public float SummonInterval = 8f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject portalObject = Instantiate(PortalPrefab, transform.position, transform.rotation);
        portalObject.transform.SetParent(transform);
        portalObject.transform.localScale = Vector3.one;
        // The spawn manager is the parent of the spawn location, which is the parent of spawned enemy portals
        EnemySpawnManager = EnemySpawnManager = GetComponentInParent<Transform>().parent.GetComponentInParent<SpawnManager>();
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
        // The empty parent is used to set a local y offset, delete the parent
        Destroy(transform.parent.gameObject, DeathEffect.GetComponent<ParticleSystem>().main.duration / 4);
    }

    private void SummonEnemy()
    {
        EnemySpawnManager.SpawnDataset(Summons);

        int totalCount = Summons.spawnGroups.Sum(group => group.count);
        Health.DealDamage(totalCount * CostPerEnemy);
    }
}
