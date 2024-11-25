using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Transform cameraTransform;
    public CharacterController controller;
    public Rigidbody rb;
    public GameObject model;

    public float speed = 5f; // Movement speed
    public float gravity = 10f;

    void Start()
    {

    }

    bool wasRolling = false;

    void Update()
    {
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

        Movement(movementInput);
    }

    public void Rolling(Vector2 movementInput)
    {
        animator.SetBool("Rolling", true);
        rb.isKinematic = false;

        if (!wasRolling)
        {
            rb.velocity = controller.velocity;
            wasRolling = true;
        }

        controller.enabled = false;
    }

    public void Movement(Vector2 movementInput)
    {
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

        Vector3 finalMoveDirection = new Vector3(desiredMoveDirection.x, -1 * gravity * Time.deltaTime, desiredMoveDirection.z);

        // Move the player
        controller.Move(finalMoveDirection * speed * Time.deltaTime);

        // Optional: Rotate the player to face the movement direction
        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, targetRotation, Time.deltaTime * 10f);

            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }
}
