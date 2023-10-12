using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamager : MonoBehaviour
{
    private int damage;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyCollidedWith.Contains(other))
        {
            return;
        }
        else
        {
            alreadyCollidedWith.Add(other);
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
