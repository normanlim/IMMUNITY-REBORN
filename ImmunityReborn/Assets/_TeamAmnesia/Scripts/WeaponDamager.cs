using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamager : MonoBehaviour
{
    private int damage;

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
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
