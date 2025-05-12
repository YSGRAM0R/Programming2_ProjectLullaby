using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 6f;
    public float crouchHeight = 0.5f;
    public float standingHeight = 1.8f;
    public float slopeForce = 8f;
    public float slopeRayLength = 1.5f;

    [Header("Wall Running")]
    public float wallRunSpeed = 7f;
    public float wallRunTime = 1f;
    public LayerMask wallMask;
    private bool isWallRunning = false;
    private float wallRunTimer = 0f;
    private Vector3 wallRunDirection;
    private bool isTouchingWall = false;
    private Vector3 wallNormal;
    public float wallCheckDistance = 0.6f;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashCooldown = 1f;
    private float nextDashTime = 0f;
    private bool isDashing = false;
    public float dashDuration = 0.15f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private float verticalRotation = 0f;
    private bool isGrounded;
    private bool isCrouching = false;
    private CapsuleCollider capsule;

    private Vector3 inputDirection;

    [Header("Movement Tweaks")]
    private float currentSpeed = 0f;
    public float acceleration = 10f;
    public float deceleration = 8f;
    public float airControlFactor = 0.3f;
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float sprintFOV = 80f;

    public float jumpBufferTime = 0.2f;
    private float jumpBufferTimeLeft = 0f;
    public float landImpactForce = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        capsule = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
        HandleMouseLook();
        HandleJump();
        HandleCrouch();
        HandleDash();
        HandleLanding();
        HandleWallRun();

        if (jumpBufferTimeLeft > 0f)
        {
            jumpBufferTimeLeft -= Time.deltaTime;
        }

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, Input.GetKey(KeyCode.LeftShift) ? sprintFOV : normalFOV, Time.deltaTime * 5f);
    }

    void FixedUpdate()
    {
        CheckGrounded();
        CheckWallContact();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        if (isDashing || isWallRunning) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        inputDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        float targetSpeed = isCrouching ? crouchSpeed : (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed);

        if (inputDirection.magnitude > 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        float movementFactor = isGrounded ? 1f : airControlFactor;

        Vector3 targetVelocity = inputDirection * currentSpeed * movementFactor;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimeLeft = jumpBufferTime;
        }

        if (jumpBufferTimeLeft > 0f)
        {
            if (isGrounded && !isCrouching)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                jumpBufferTimeLeft = 0f;
            }
            else if (isTouchingWall && !isGrounded && !isWallRunning && IsCloseToWall())
            {
                StartWallRun();
                jumpBufferTimeLeft = 0f;
            }
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            capsule.height = crouchHeight;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, crouchHeight / 2f, cameraTransform.localPosition.z);
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            capsule.height = standingHeight;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, standingHeight / 2f, cameraTransform.localPosition.z);
            isCrouching = false;
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= nextDashTime && !isDashing)
        {
            Vector3 dashDir = inputDirection != Vector3.zero ? inputDirection : transform.forward;
            StartCoroutine(Dash(dashDir));
            nextDashTime = Time.time + dashCooldown;
        }
    }

    IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        rb.linearVelocity = direction.normalized * dashForce;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    void HandleWallRun()
    {
        if (isWallRunning)
        {
            if (wallRunTimer > 0)
            {
                rb.linearVelocity = new Vector3(wallRunDirection.x, rb.linearVelocity.y, wallRunDirection.z);
                wallRunTimer -= Time.deltaTime;
            }
            else
            {
                EndWallRun();
            }
        }
    }

    void StartWallRun()
    {
        isWallRunning = true;
        wallRunDirection = Vector3.Cross(wallNormal, Vector3.up).normalized * wallRunSpeed;
        wallRunTimer = wallRunTime;
    }

    void EndWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
    }

    void HandleLanding()
    {
        if (isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -landImpactForce, rb.linearVelocity.z);
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    void CheckWallContact()
    {
        isTouchingWall = false;
        wallNormal = Vector3.zero;

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.right, out hit, wallCheckDistance, wallMask) ||
            Physics.Raycast(transform.position, -transform.right, out hit, wallCheckDistance, wallMask))
        {
            isTouchingWall = true;
            wallNormal = hit.normal;
        }
    }

    bool IsCloseToWall()
    {
        return isTouchingWall && Vector3.Angle(wallNormal, Vector3.up) < 70f;
    }
}

