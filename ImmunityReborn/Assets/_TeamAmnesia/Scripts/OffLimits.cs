using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Marks object as off limits. Works if other collider is an object with Health component or its child.
/// Attach a Rigidbody to objects containing this component.
/// </summary>
public class OffLimits : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("OffLimitsTrigger")) { return; }

        Health health = other.GetComponentInParent<Health>();

        if (health != null)
        {
            health.DealDamage( health.MaxHealth );
        }
        else if (other.TryGetComponent(out health))
        {
            health.DealDamage( health.MaxHealth );
        }
    }
}
