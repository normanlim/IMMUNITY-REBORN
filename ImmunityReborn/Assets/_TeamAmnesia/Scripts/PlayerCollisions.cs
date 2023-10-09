using UnityEngine;
using System;

public class PlayerCollisions : MonoBehaviour
{
    public float health;
    private string[] attackTypeTags = { "MeleeType", "RangedType", "MagicType" };
    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isMatchFound = Array.Exists(attackTypeTags, tag => tag == other.tag);
        Debug.Log(isMatchFound);
        if (isMatchFound)
        {
            health--;
            Destroy(other.gameObject);
        }
    }
}
