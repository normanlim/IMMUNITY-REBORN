using System.Collections.Generic;
using UnityEngine;

public class WeaponDamager : MonoBehaviour
{
    public int damage;
    public float knockback;

    private List<Collider> collidedWith = new List<Collider>();

    [field: SerializeField]
    public DamageType DamageType { get; private set; }

    [field: SerializeField]
    public Collider CharacterCollider;

    private void OnEnable()
    {
        collidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
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
            health.DealDamage(damage, DamageType);
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
