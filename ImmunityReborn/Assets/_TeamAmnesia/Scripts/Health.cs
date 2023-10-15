using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    private int health;

    public event Action OnDie;

    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
        if (health == 0) { return; } // avoid further calculations if already dead

        health = Mathf.Max(health - damage, 0); // makes sure health is never negative

        if (health == 0) // logic after health is updated
        {
            OnDie?.Invoke();
        }

        Debug.Log($"{name} : {health} health");
    }
}