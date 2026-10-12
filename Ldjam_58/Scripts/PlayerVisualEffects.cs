using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour
{
    [Header("Camera Shake Settings")]
    [SerializeField] private float walkShakeIntensity = 0.05f;
    [SerializeField] private float sprintShakeIntensity = 0.1f;

    [Header("Step Settings")]
    [SerializeField] private float walkStepFrequency = 2f;
    [SerializeField] private float sprintStepFrequency = 3f;

    [Header("Hands Settings")]
    [SerializeField] private Transform hands;
    [SerializeField] private float handsShakeMultiplier = 1.5f;

    [Header("References")]
    [SerializeField] private PlayerMovement movementSystem;
    [SerializeField] private PlayerHunger hungerSystem;

    private Transform cameraTransform;
    private Vector3 originalCameraPosition;
    private Vector3 originalHandsPosition;
    private float shakeTimer;

    private void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        originalCameraPosition = cameraTransform.localPosition;
        originalHandsPosition = hands.localPosition;
    }

    private void Update()
    {
        HandleMovementCameraShake();
    }

    private void HandleMovementCameraShake()
    {
        if (movementSystem.IsMoving() && movementSystem.GetComponent<CharacterController>().isGrounded)
        {
            ApplyMovementShake();
        }
        else
        {
            ResetShakeState();
        }

        cameraTransform.localPosition = originalCameraPosition + hungerSystem.HungerShakeOffset;
    }

    private void ApplyMovementShake()
    {
        float intensity = movementSystem.IsSprinting ? sprintShakeIntensity : walkShakeIntensity;
        float frequency = movementSystem.IsSprinting ? sprintStepFrequency : walkStepFrequency;

        shakeTimer += Time.deltaTime * frequency;

        Vector3 movementShakeOffset = CalculateMovementShakeOffset(intensity);
        cameraTransform.localPosition = originalCameraPosition + movementShakeOffset + hungerSystem.HungerShakeOffset;

        ApplyHandsShake(intensity, frequency);
    }

    private Vector3 CalculateMovementShakeOffset(float intensity)
    {
        float verticalPhase = Mathf.Sin(shakeTimer * Mathf.PI * 2);
        float horizontalPhase = Mathf.Sin(shakeTimer * Mathf.PI * 2 + Mathf.PI * 0.5f);

        float yShake = verticalPhase * intensity;
        float xShake = horizontalPhase * intensity * 0.3f;

        return new Vector3(xShake, yShake, 0);
    }

    private void ApplyHandsShake(float intensity, float frequency)
    {
        float handsIntensity = intensity * handsShakeMultiplier;

        float handsVerticalPhase = Mathf.Sin(shakeTimer * Mathf.PI * 2.5f);
        float handsHorizontalPhase = Mathf.Sin(shakeTimer * Mathf.PI * 1.8f + Mathf.PI * 0.3f);

        float handsYShake = handsVerticalPhase * handsIntensity;
        float handsXShake = handsHorizontalPhase * handsIntensity * 0.4f;
        float handsZShake = handsHorizontalPhase * handsIntensity * 0.2f;

        Vector3 handsPositionOffset = new Vector3(handsXShake, handsYShake, handsZShake);
        hands.localPosition = originalHandsPosition + handsPositionOffset;
    }

    private void ResetShakeState()
    {
        shakeTimer = 0f;
        ResetHandsPosition();
    }

    private void ResetHandsPosition()
    {
        hands.localPosition = Vector3.Lerp(hands.localPosition, originalHandsPosition, Time.deltaTime * 7f);
    }
}