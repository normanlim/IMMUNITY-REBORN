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

    public bool IsMeleeImmune { get; set; }
    public bool IsRangedImmune { get; set; }
    public bool IsMagicImmune { get; set; }

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

    public void DealDamage( int damage, DamageType damageType = DamageType.None )
    {
        if ( currentHealth == 0 ) { return; } // avoid further calculations if already dead

        if (damageType == DamageType.Melee && IsMeleeImmune) { return; }
        if (damageType == DamageType.Ranged && IsRangedImmune) { return; }
        if (damageType == DamageType.Magic && IsMagicImmune) { return; }

        currentHealth = Mathf.Max(currentHealth - damage, 0); // makes sure health is never negative

        OnTakeDamage?.Invoke();

        if ( currentHealth == 0 ) // logic after health is updated
        {
            OnDie?.Invoke();
        }
    }
}
