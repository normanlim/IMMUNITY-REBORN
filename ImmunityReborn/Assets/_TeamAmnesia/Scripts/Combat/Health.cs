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
    public event Action OnHeal;
    public event Action OnDie;

    public bool IsPlayerDead;

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
        IsPlayerDead = false;
    }

    public void SetGodModeHealth()
    {
        currentHealth = 1000000;
        maxHealth = 1000000;
    }

    // Returns damage dealt
    public int DealDamage( int damage, DamageType damageType = DamageType.None )
    {
        if ( currentHealth == 0 ) { return 0; } // avoid further calculations if already dead

        if (damageType == DamageType.Melee && IsMeleeImmune) { return 0; }
        if (damageType == DamageType.Ranged && IsRangedImmune) { return 0; }
        if (damageType == DamageType.Magic && IsMagicImmune) { return 0; }

        currentHealth = Mathf.Max(currentHealth - damage, 0); // makes sure health is never negative

        OnTakeDamage?.Invoke();

        if ( currentHealth == 0 ) // logic after health is updated
        {
            OnDie?.Invoke();
            IsPlayerDead = true;
        }
        return damage;
    }

    public void Heal(int amount)
    {
        currentHealth = currentHealth + amount > maxHealth ? maxHealth : currentHealth + amount;
        OnHeal?.Invoke();
    }
}
