using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;

    [Header("Movement")]
    [SerializeField] private float speed = 40f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float groundCheckRadius = 0.3f;
    private bool readyToJump = true;
    private bool isGrounded;

    [Header("Dashing")]
    [SerializeField] private float dashForce = 50f;
    [SerializeField] private float dashCooldown = 1f;
    private bool readyToDash = true;

    [Header("Gravity")]
    [SerializeField] private float gravityMultiplier = 3f;

    [Header("Drag")]
    [SerializeField] private float drag = 6f;

    private Vector3 moveDirection;
    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        rb.linearDamping = drag;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && readyToJump)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Q) && readyToDash)
        {
            Dash();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection * speed, ForceMode.Acceleration);
        
        rb.AddForce(Vector3.down * gravityMultiplier * 9.81f, ForceMode.Acceleration);
    }

    private void Jump()
    {
        readyToJump = false;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Dash()
    {
        readyToDash = false;

        Vector3 dashDirection = moveDirection == Vector3.zero ? orientation.forward : moveDirection;
        rb.AddForce(dashDirection.normalized * dashForce, ForceMode.Impulse);

        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void ResetDash()
    {
        readyToDash = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}