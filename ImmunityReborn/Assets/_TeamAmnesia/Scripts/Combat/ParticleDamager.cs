using System.Collections.Generic;
using UnityEngine;

public class ParticleDamager : MonoBehaviour
{
    [field: SerializeField]
    public DamageType DamageType { get; private set; }

    [field: SerializeField]
    public GameObject HitSFX;

    private int damage;
    private float knockback;
    private float damageCooldown;
    private float timer;
    private bool isDamageCooldownActive;

    private void Start()
    {
        timer = damageCooldown;
        isDamageCooldownActive = false;
    }

    private void Update()
    {
        if (isDamageCooldownActive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                isDamageCooldownActive = false;
                timer = damageCooldown;
            }
        }
    }

    private void OnParticleCollision(GameObject otherObj)
    {
        if (isDamageCooldownActive) { return; }

        if (otherObj.TryGetComponent(out Collider other))
        {
            if (other.TryGetComponent(out Health health))
            {
                int damageDealt = health.DealDamage(damage, DamageType);
                isDamageCooldownActive = true;

                if (HitSFX != null && damageDealt > 0)
                    Instantiate(HitSFX, gameObject.transform);
            }

            if (other.TryGetComponent(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                forceReceiver.AddForce(direction * knockback);
            }
        }
    }

    public void SetDamage(int damage, float knockback, float damageCooldown)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.damageCooldown = damageCooldown;
    }
}
