using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamager : MonoBehaviour
{
    private int damage;
    private float knockback;

    private List<Collider> collidedWith = new List<Collider>();

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
            health.DealDamage(damage);
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
