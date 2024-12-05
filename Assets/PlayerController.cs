using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Transform cameraTransform;
    public CharacterController controller;
    public Rigidbody rb;
    public GameObject model;
    public ParticleSystem particles;

    public float speed = 5f; // Movement speed
    public float gravity = 10f;
    float gravityModifier = 1f;
    public bool disableControl;

    public float jumpTime = 0f;
    float maxJumpTime = 0.2f;
    public float jumpStrength = 5f;

    public bool DEBUGIsGrounded = false;

    public Vector3 velocity;

    bool wasRolling = false;

    void Update()
    {
        if (disableControl)
        {
            return;
        }

        DEBUGIsGrounded = controller.isGrounded;

        bool ballin = Input.GetKey(KeyCode.LeftShift);

        Vector2 movementInput = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        if (ballin)
        {
            Rolling(movementInput);
            return;
        }

        transform.rotation = Quaternion.Euler(Vector3.zero);
        animator.SetBool("Rolling", false);
        rb.isKinematic = true;
        controller.enabled = true;
        wasRolling = false;


        // Calculate the forward and right direction based on the camera's orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;


        // Calculate the desired movement direction
        Vector3 desiredMoveDirection = (forward * movementInput.y + right * movementInput.x).normalized;

        velocity.x = desiredMoveDirection.x * speed * Time.deltaTime;
        velocity.z = desiredMoveDirection.z * speed * Time.deltaTime;

        var emission = particles.emission;

        // Optional: Rotate the player to face the movement direction
        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);

            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, Time.deltaTime * 10f);

            animator.SetBool("Moving", true);
            
            if (controller.isGrounded)
            {
                emission.rateOverTime = 5;
            }
        }
        else
        {
            animator.SetBool("Moving", false);
            emission.rateOverTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = jumpStrength;
        }

        if (!controller.isGrounded)
        {
            velocity.y += -gravity * Time.deltaTime;
        }

        controller.Move(velocity);
    }

    public void Rolling(Vector2 movementInput)
    {
        animator.SetBool("Rolling", true);
        rb.isKinematic = false;

        var emission = particles.emission;
        emission.rateOverTime = 0;

        if (!wasRolling)
        {
            rb.velocity = controller.velocity;
            rb.velocity += model.transform.forward * 10;
            wasRolling = true;
        }

        controller.enabled = false;
    }
}
