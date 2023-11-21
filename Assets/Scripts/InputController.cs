using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private SwordController SwordController;

    private void OnEnable()
    {
        _playerInput.actions["Throw"].performed += OnBallThrow;

        _playerInput.actions["Move"].performed += OnMoveInput;
        _playerInput.actions["Move"].canceled += OnMoveInput;

        _playerInput.actions["Rotate"].performed += OnRotateSword;
    }

    private void OnDisable()
    {
        _playerInput.actions["Throw"].performed -= OnBallThrow;

        _playerInput.actions["Move"].performed -= OnMoveInput;
        _playerInput.actions["Move"].canceled -= OnMoveInput;

        _playerInput.actions["Rotate"].performed -= OnRotateSword;
    }

    private void OnRotateSword(InputAction.CallbackContext context)
    {
        float direction = context.ReadValue<float>();

        SwordController.RotateSword(direction);
    }

    private void OnBallThrow(InputAction.CallbackContext context)
    {
        if (SwordController is not null) 
            SwordController.ThrowBall();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            SwordController.MoveDirection = Vector3.zero;
        }
        var moveVector = context.ReadValue<float>();
        SwordController.MoveDirection = moveVector * Vector3.right;
    }
    
}