
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
        // Removed temporarily in case will be used. Health logic moved
        //if (Array.Exists(attackTypeTags, tag => tag == other.tag))
        //{
        //    Debug.Log( "NTest2" );
        //    health--;
        //    Destroy(other.gameObject);
        //    healthSlider.value = health;
        //}
    }
}
