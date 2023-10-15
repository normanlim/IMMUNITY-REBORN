using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private float drag = 0.3f;

    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

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
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}
