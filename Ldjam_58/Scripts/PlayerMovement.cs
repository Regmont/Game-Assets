using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private StorageHandler storageHandler;
    [SerializeField] private PlayerStamina staminaSystem;

    [Header("Speed Modifiers")]
    [SerializeField] private float emptyHandsSpeedModifier = 1f;
    [SerializeField] private float oneHandOccupiedSpeedModifier = 0.85f;
    [SerializeField] private float bothHandsOccupiedSpeedModifier = 0.7f;

    public float GetSprintSpeed => sprintSpeed;
    public float GetWalkSpeed => walkSpeed;

    private CharacterController characterController;
    private Transform cameraTransform;
    private float xRotation;
    private Vector3 velocity;
    private float currentSpeed;
    private bool isSprinting;

    private const float GROUNDED_VELOCITY_Y = -2f;

    public bool IsSprinting => isSprinting;

    public bool IsMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        Vector3 movementInput = GetMovementInput();
        float speedModifier = CalculateSpeedModifier();
        float targetSpeed = CalculateTargetSpeed(speedModifier);

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        characterController.Move(movementInput * currentSpeed * Time.deltaTime);

        ApplyGravity();
    }

    private Vector3 GetMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        return transform.right * moveX + transform.forward * moveZ;
    }

    private float CalculateTargetSpeed(float speedModifier)
    {
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0 &&
                           staminaSystem != null && staminaSystem.GetCurrentStamina > 0;
        float baseSpeed = wantsToSprint ? sprintSpeed : walkSpeed;

        isSprinting = wantsToSprint && baseSpeed == sprintSpeed;

        return baseSpeed * speedModifier;
    }

    private float CalculateSpeedModifier()
    {
        if (storageHandler == null)
        {
            return emptyHandsSpeedModifier;
        }

        bool leftHandEmpty = storageHandler.LeftHandIsEmpty();
        bool rightHandEmpty = storageHandler.RightHandIsEmpty();

        if (leftHandEmpty && rightHandEmpty)
        {
            return emptyHandsSpeedModifier;
        }

        if (!leftHandEmpty && !rightHandEmpty)
        {
            return bothHandsOccupiedSpeedModifier;
        }

        return oneHandOccupiedSpeedModifier;
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = GROUNDED_VELOCITY_Y;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}