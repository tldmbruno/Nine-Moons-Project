using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 100f;
    [SerializeField] private float _jumpForce = 7.5f;
    [SerializeField] private float _friction = 15f;

    [Header("Settings")]
    [SerializeField] private float _lookSensitivity = 7.5f;

    private Rigidbody _rigidBody;
    private Camera _mainCamera;

    private Vector3 _inputDirection;
    private bool _isOnFloor = false;
    private float _rotationX;

    public GameObject Fire;

    private void Start()
    {
        if (!transform.TryGetComponent(out Rigidbody transformRigidbody)) transform.AddComponent<Rigidbody>();

        _rigidBody = transformRigidbody;
        _rigidBody.drag = 0;

        _mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MovementControl();
        JumpControl();
        MouseControl();
        AttackControl();
    }

    private void FixedUpdate()
    {
        ComputeMovement();
    }

    private void MouseControl()
    {
        _rotationX += -Input.GetAxis("Mouse Y") * _lookSensitivity;
        _rotationX = Mathf.Clamp(_rotationX, -90, 90);
        _mainCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSensitivity, 0);
    }

    private void MovementControl()
    {
        _inputDirection = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _inputDirection = transform.forward * _inputDirection.z + transform.right * _inputDirection.x;
        _inputDirection.Normalize();
    }

    private void AttackControl()
    {
        if (!Input.GetMouseButton(0)) return;

        Debug.Log("Shoot!");
        Instantiate(Fire, transform.position, Quaternion.identity);
    }

    private void JumpControl()
    {
        if (!_isOnFloor) return;
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        _rigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void ComputeMovement()
    {
        _rigidBody.AddForce(_inputDirection * _moveSpeed);

        const float DISTANCE_TO_FLOOR = 0.6f;
        _isOnFloor = Physics.Raycast(transform.position, Vector3.down, DISTANCE_TO_FLOOR);

        _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, new(0, _rigidBody.velocity.y, 0), _friction * Time.deltaTime);
    }
}
