using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private float drag = 0.3f;

    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;
    private NavMeshAgent navMeshAgent;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Start()
    {
        TryGetComponent(out navMeshAgent);
    }

    private void Update()
    {
        if (verticalVelocity < 0.0f && characterController.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime; // stay above the ground
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime; // simulates gravity
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (navMeshAgent != null)
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f) // don't wait until impact force is 0 because it takes too long
            {
                impact = Vector3.zero;
                navMeshAgent.enabled = true; // allow AI to navigate again
            }
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;

        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false; // prevent AI navigation from fighting against impact force
        }
    }
}
