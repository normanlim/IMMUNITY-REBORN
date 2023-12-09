using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamager : MonoBehaviour
{
    public event Action<int> OnDamageDealt;

    private int damage;
    private float knockback;

    private List<Collider> collidedWith = new List<Collider>();

    [field: SerializeField]
    public DamageType DamageType { get; private set; }

    [field: SerializeField]
    public Collider CharacterCollider;

    [field: SerializeField]
    public GameObject HitSFX;

    private void OnEnable()
    {
        collidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collidedWith.Contains(other))
        {
            return;
        }
        else
        {
            collidedWith.Add(other);
        }

        if (other.TryGetComponent(out Health health))
        {
            int damageDealt = health.DealDamage(damage, DamageType);
            OnDamageDealt?.Invoke(damageDealt);
            if (HitSFX != null && damageDealt > 0)
                Instantiate(HitSFX, gameObject.transform);
        }

        if (other.TryGetComponent(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
        }
    }

    public void SetDamage(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
