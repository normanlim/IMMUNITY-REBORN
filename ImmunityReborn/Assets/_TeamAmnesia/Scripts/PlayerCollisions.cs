
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollisions : MonoBehaviour
{
    public int health;
    private string[] attackTypeTags = { "MeleeType", "RangedType", "MagicType" };
    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        healthSlider.value = health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Array.Exists(attackTypeTags, tag => tag == other.tag))
        {
            health--;
            Destroy(other.gameObject);
            healthSlider.value = health;
        }
    }
}
