using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] // makes this property a field and serializes it
    public InputReader InputReader { get; private set; }

    [field: SerializeField]
    public CharacterController CharacterController { get; private set; }

    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField]
    public Health Health { get; private set; }

    [field: SerializeField]
    public HealthConsumable HealthConsumable { get; private set; }

    [field: SerializeField]
    public Ragdoll Ragdoll { get; private set; }

    [field: SerializeField]
    public MemoryGauge MemoryGauge { get; private set; }

    [field: SerializeField]
    public ShieldController ShieldController { get; private set; }

    [field: SerializeField]
    public float DefaultMovementSpeed { get; private set; }

    [field: SerializeField]
    public float RotationDamping { get; private set; }

    [field: SerializeField]
    public AttackData[] Attacks { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    public bool IsGodModeActive { get; private set; }

    [SerializeField] public GameObject VFXHealing;
    [SerializeField] public GameObject SFXTakeDamage;
    [SerializeField] public GameObject SFXDeath;
    [SerializeField] GameObject SFXShieldActivate;
    [SerializeField] GameObject SFXShieldActive;
    [SerializeField] GameObject SFXShieldCollision;
    [SerializeField] GameObject SFXMemoryAttackActivate;
    [SerializeField] GameObject SFXMemoryAttackActive;
    [SerializeField] GameObject SFXMemoryAttackCollision;

    private void Start()
    {
        IsGodModeActive = true;
        //IsGodModeActive = PlayerPrefs.GetInt( "GodMode", 0 ) == 1;
        if ( IsGodModeActive ) { Health.SetGodModeHealth(); }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerDefaultState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
        InputReader.UseHealEvent += HandleUseHeal;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
        InputReader.UseHealEvent -= HandleUseHeal;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        Invoke( "ResetCurrentScene", 5.0f );
        SwitchState(new PlayerDeadState(this));
    }

    private void HandleUseHeal()
    {
        SwitchState(new PlayerUsingHealState(this));
    }

    private void ResetCurrentScene()
    {
        // Get the name of the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Load the current scene again to reset it
        SceneManager.LoadScene( currentSceneName );
    }

    private bool canPlaySFX = true;
    private const float SFXCooldown = 0.6f; // Adjust the cooldown duration as needed

    public void PlaySFXThenDestroy(GameObject soundPrefab, Transform transform)
    {
        if (canPlaySFX)
        {
            PlaySFX.PlayThenDestroy(soundPrefab, transform);
            canPlaySFX = false;
            StartCoroutine(SFXCooldownCoroutine());
        }
    }

    private IEnumerator SFXCooldownCoroutine()
    {
        yield return new WaitForSeconds(SFXCooldown);
        canPlaySFX = true; // Reset the cooldown
    }
}
