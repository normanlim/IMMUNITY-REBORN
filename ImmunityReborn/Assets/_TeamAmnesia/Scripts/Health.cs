using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private int currentHealth;

    public event Action OnTakeDamage;
    public event Action OnDie;

    public int CurrentHealth { 
        get { return currentHealth; }
        private set { currentHealth = value; } 
    }

    public int MaxHealth
    {
        get { return maxHealth; } 
        private set { maxHealth = value; }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage( int damage )
    {
        if ( currentHealth == 0 ) { return; } // avoid further calculations if already dead

        currentHealth = Mathf.Max(currentHealth - damage, 0); // makes sure health is never negative

        OnTakeDamage?.Invoke();

        if ( currentHealth == 0 ) // logic after health is updated
        {
            OnDie?.Invoke();
        }

    }
}
