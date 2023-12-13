using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counterattacks : MonoBehaviour
{
    public event Action OnSlashComplete;

    [field: SerializeField]
    public GameObject MeleeObject { get; private set; }

    [field: SerializeField]
    public WeaponDamager MeleeWeaponDamager { get; private set; }

    [field: SerializeField]
    public int MeleeDamage { get; private set; }

    [field: SerializeField]
    public float MeleeKnockback { get; private set; }

    [field: SerializeField]
    public float MeleeSlashSpeed { get; private set; } = 0.2f;

    [field: SerializeField]
    public ProjectileShooter ProjectileShooter { get; private set; }

    [field: SerializeField]
    public int RangedDamage { get; private set; }

    [field: SerializeField]
    public float RangedKnockback { get; private set; }

    [field: SerializeField]
    public WeaponDamager MagicWeaponDamager { get; private set; }

    [field: SerializeField]
    public int MagicDamage { get; private set; }

    [field: SerializeField]
    public float MagicKnockback { get; private set; }

    [field: SerializeField]
    public float ExplosionDuration { get; private set; } = 0.2f;

    private bool isMeleeObjectRotating = false;
    private bool isExploding = false;
    [SerializeField] private GameObject explosionFX;

    private void Start()
    {
        MeleeWeaponDamager.SetDamage(MeleeDamage, MeleeKnockback);
        MagicWeaponDamager.SetDamage(MagicDamage, MagicKnockback);

        MeleeWeaponDamager.gameObject.SetActive(false);
        MagicWeaponDamager.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ShieldCollisions.OnMeleeBlock += HandleMeleeBlock;
        ShieldCollisions.OnRangedBlock += HandleRangedBlock;
        ShieldCollisions.OnMagicBlock += HandleMagicBlock;
        OnSlashComplete += HandleSlashComplete;
    }

    private void OnDisable()
    {
        ShieldCollisions.OnMeleeBlock -= HandleMeleeBlock;
        ShieldCollisions.OnRangedBlock -= HandleRangedBlock;
        ShieldCollisions.OnMagicBlock -= HandleMagicBlock;
        OnSlashComplete -= HandleSlashComplete;
    }

    private void HandleSlashComplete()
    {
        MeleeWeaponDamager.gameObject.SetActive(false);
    }

    private void HandleMeleeBlock(GameObject obj)
    {
        Vector3 enemyDirection = (obj.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(enemyDirection, Vector3.up);
        Vector3 slashStartDirection = lookRotation.eulerAngles + new Vector3(0, -45, 0);

        MeleeObject.transform.eulerAngles = slashStartDirection;
        MeleeWeaponDamager.gameObject.SetActive(true);
        StartCoroutine(RotateMeleeObject(new Vector3(0, 90, 0), MeleeSlashSpeed));
    }

    private void HandleRangedBlock(GameObject obj)
    {
        ProjectileShooter.targetObject = obj;
        ProjectileShooter.TryAimingAtTarget();
        ProjectileShooter.FireAtTarget(RangedDamage, RangedKnockback);
    }

    private void HandleMagicBlock(GameObject obj)
    {
        StartCoroutine(Explode(ExplosionDuration));
    }

    private IEnumerator RotateMeleeObject(Vector3 eulerAngles, float duration)
    {
        if (isMeleeObjectRotating)
        {
            yield break;
        }

        isMeleeObjectRotating = true;
        Vector3 currentRot = MeleeObject.transform.eulerAngles;
        Vector3 newRot = currentRot + eulerAngles;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            MeleeObject.transform.eulerAngles = Vector3.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }

        isMeleeObjectRotating = false;
        OnSlashComplete?.Invoke();
    }

    private IEnumerator Explode(float duration)
    {
        if (isExploding)
        {
            yield break;
        }

        isExploding = true;
        MagicWeaponDamager.gameObject.SetActive(true);
        Instantiate(explosionFX, transform);
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        MagicWeaponDamager.gameObject.SetActive(false);
        isExploding = false;
    }
}
