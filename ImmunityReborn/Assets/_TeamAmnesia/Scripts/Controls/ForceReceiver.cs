using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;

    private float verticalVelocity;

    public Vector3 VerticalMovement => Vector3.up * verticalVelocity;

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
    }
}
