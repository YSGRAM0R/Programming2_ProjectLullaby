using UnityEngine;

public class ReferencePlayerMovement : MonoBehaviour
{
    private ReferenceInputController _inputController;
    private CharacterController _characterController;
    
    [Header("Movement")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private ReferenceMovementConfig movementConfig;
    private Vector2 _moveInput;
    private Vector3 _currentVelocity;
    
    [Header("Look Rotation")]
    [SerializeField] private Transform lookTarget;
    private Vector2 _lookRotation;
    
    private void Awake()
    {
        _inputController = GetComponent<ReferenceInputController>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        if (_inputController != null)
        {
            _inputController.MoveEvent += HandleMoveInput;
            _inputController.JumpEvent += Jump;
            _inputController.LookEvent += HandleLookRotation;
        }
    }

    private void Update()
    {
        Vector3 targetDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        Vector3 targetVelocity = targetDirection * movementConfig.targetMoveSpeed;
        
        float accel = IsGrounded() ? movementConfig.accelerationRate : movementConfig.airAccelerationRate;
        _currentVelocity = Vector3.MoveTowards(_currentVelocity, targetVelocity, accel * Time.deltaTime);

        if (!IsGrounded())
        {
            _currentVelocity.y += Physics.gravity.y * movementConfig.gravityMultiplier * Time.deltaTime;
        }
        
        _characterController.Move(_currentVelocity * Time.deltaTime);
        
        transform.Rotate(Vector3.up, _lookRotation.x * movementConfig.lookSpeed);
        lookTarget.Rotate(Vector3.right, -_lookRotation.y * movementConfig.lookSpeed);
    }

    private void HandleMoveInput(Vector2 movement)
    {
        _moveInput = movement;
    }

    private void HandleLookRotation(Vector2 lookRotation)
    {
        _lookRotation = lookRotation;
    }
    
    private void Jump()
    {
        if (IsGrounded())
        {
            _currentVelocity.y = movementConfig.baseJumpForce;
        }
    }

    private bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, .5f, Vector3.down, out RaycastHit hit, .6f, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 end = origin + Vector3.down * .6f;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(origin, .5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(end, .5f);
    }
}
