using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReferenceInputController : MonoBehaviour
{
    private GameControls _gameControls;

    public event Action JumpEvent;
    public event Action FireEvent;
    public event Action FireEventCancelled;
    public event Action JumpEventCancelled;
    public event Action<Vector2> MoveEvent;

    public event Action<Vector2> LookEvent;
    
    private void Awake()
    {
        _gameControls = new GameControls();
    }

    private void OnEnable()
    {
        _gameControls.Player.Enable();
        
        _gameControls.Player.Move.performed += OnMovePerformed;
        _gameControls.Player.Move.canceled += OnMoveCancelled;
        _gameControls.Player.Jump.performed += OnJumpPerformed;
        _gameControls.Player.Jump.canceled += OnJumpCancelled;
        _gameControls.Player.Look.performed += OnLookPerformed;
        _gameControls.Player.Look.canceled += OnLookCancelled;
        _gameControls.Player.Attack.performed += OnFirePerformed;
        _gameControls.Player.Attack.canceled += OnFireCancelled;
        
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        FireEvent?.Invoke();
    }

    private void OnFireCancelled(InputAction.CallbackContext context)
    {
        FireEventCancelled?.Invoke();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(Vector2.zero);
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnLookCancelled(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(Vector2.zero);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke();
    }

    private void OnJumpCancelled(InputAction.CallbackContext context)
    {
        JumpEventCancelled?.Invoke();
    }
}
