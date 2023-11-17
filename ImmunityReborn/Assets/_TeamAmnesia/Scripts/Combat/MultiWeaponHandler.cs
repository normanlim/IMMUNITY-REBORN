using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiWeaponHandler : MonoBehaviour
{
    [Serializable]
    private struct Weapon
    {
        public string name;
        public GameObject weaponLogic;
    }

    [SerializeField]
    private Weapon[] weapons;

    private void Start()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.weaponLogic.SetActive(false);
        }
    }

    public void EnableWeapon(string name)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.name == name)
            {
                weapon.weaponLogic.SetActive(true);
                return;
            }
        }

        Debug.Log($"Did not find weapon called {name}");
    }

    public void DisableWeapon(string name)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.name == name)
            {
                weapon.weaponLogic.SetActive(false);
                return;
            }
        }

        Debug.Log($"Did not find weapon called {name}");
    }
}
