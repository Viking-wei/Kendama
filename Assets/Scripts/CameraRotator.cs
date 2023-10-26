using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotator : MonoBehaviour
{
    public Transform RotateCenter;
    [Tooltip("Camera rotating speed")] public float RotateSpeed = 9f;

    [Tooltip("Speed for increasing rotate speed")]
    public float DampSpeed = 13f;

    private float _currentRotateSpeed = 0f;
    private float _rotateState = 0f;

    private float RotateState
    {
        get => _rotateState;
        set => _rotateState = Mathf.Abs(value) > 1 ? Mathf.Sign(value) : value;
    }

    public PlayerInput PlayerInput;

    /*private void Start()
    {
        PlayerInput = GetComponent<PlayerInput>();
    }*/

    private void OnEnable()
    {
        if (PlayerInput == null) return;

        PlayerInput.actions["Rotate Camera"].performed += GetRotateState;
        PlayerInput.actions["Rotate Camera"].canceled += ResetRotateState;
    }

    private void OnDisable()
    {
        if (PlayerInput == null) return;

        PlayerInput.actions["Rotate Camera"].performed -= GetRotateState;
        PlayerInput.actions["Rotate Camera"].canceled -= ResetRotateState;
    }

    // Update is called once per frame
    void Update()
    {
        _currentRotateSpeed += (DampSpeed * RotateState) * Time.deltaTime;
        _currentRotateSpeed = Mathf.Clamp(_currentRotateSpeed, -RotateSpeed, RotateSpeed);
        
        transform.RotateAround(RotateCenter.position, Vector3.up, _currentRotateSpeed * Time.deltaTime);
    }

    void GetRotateState(InputAction.CallbackContext context)
    {
        RotateState = context.ReadValue<float>();
    }

    void ResetRotateState(InputAction.CallbackContext context)
    {
        _currentRotateSpeed = 0f;
        RotateState = 0f;
    }
}