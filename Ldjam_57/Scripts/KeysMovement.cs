using UnityEngine;

public class KeysMovement : MonoBehaviour
{
    public float downSpeed = 2f;
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;

    private Transform _cameraTransform;
    private Rigidbody _rb;
    private bool _isSprinting;

    void Start()
    {
        _cameraTransform = GetComponentInChildren<Camera>().transform;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    void FixedUpdate()
    {
        Vector3 gravityVelocity = Vector3.down * downSpeed;

        Vector3 moveInput = GetLocalMoveInput();

        float currentMoveSpeed = _isSprinting
            ? moveSpeed * sprintMultiplier
            : moveSpeed;

        Vector3 moveVelocity = moveInput != Vector3.zero
            ? _cameraTransform.TransformDirection(moveInput.normalized) * currentMoveSpeed
            : Vector3.zero;

        _rb.linearVelocity = gravityVelocity + moveVelocity;
    }

    Vector3 GetLocalMoveInput()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) move += Vector3.back;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;
        if (Input.GetKey(KeyCode.E)) move += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) move += Vector3.down;
        return move;
    }
}