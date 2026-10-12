using UnityEngine;

public class PlayerFootstepAudio : MonoBehaviour
{
    [Header("Footstep Settings")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float baseStepInterval = 0.5f;
    [SerializeField] private float sprintStepMultiplier = 0.5f;
    [SerializeField] private float pitchRandomness = 0.05f;

    [Header("Dependencies")]
    [SerializeField] private PlayerMovement movementSystem;

    private CharacterController characterController;
    private float stepTimer;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (movementSystem == null)
        {
            movementSystem = GetComponent<PlayerMovement>();
        }

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        InitializeAudioSource();
    }

    private void Update()
    {
        HandleFootsteps();
    }

    private void InitializeAudioSource()
    {
        if (footstepAudioSource == null)
        {
            footstepAudioSource = gameObject.AddComponent<AudioSource>();
            footstepAudioSource.playOnAwake = false;
            footstepAudioSource.loop = false;
        }
    }

    private void HandleFootsteps()
    {
        if (!CanPlayFootstep())
        {
            stepTimer = 0f;

            return;
        }

        UpdateStepTimer();
        TryPlayFootstep();
    }

    private bool CanPlayFootstep()
    {
        return characterController != null &&
               characterController.isGrounded &&
               movementSystem != null &&
               movementSystem.IsMoving();
    }

    private void UpdateStepTimer()
    {
        stepTimer += Time.deltaTime;
    }

    private void TryPlayFootstep()
    {
        float stepInterval = CalculateStepInterval();

        if (stepTimer >= stepInterval)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    private float CalculateStepInterval()
    {
        float speedRatio = GetCurrentSpeedRatio();

        return Mathf.Lerp(baseStepInterval, baseStepInterval * sprintStepMultiplier, speedRatio);
    }

    private float GetCurrentSpeedRatio()
    {
        if (movementSystem == null)
        {
            return 0f;
        }

        float currentSpeed = GetMovementMagnitude();
        float sprintSpeed = movementSystem.GetSprintSpeed;

        return Mathf.Clamp01(currentSpeed / sprintSpeed);
    }

    private float GetMovementMagnitude()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        return new Vector2(horizontal, vertical).magnitude;
    }

    private void PlayFootstep()
    {
        if (footstepAudioSource != null && footstepClip != null)
        {
            footstepAudioSource.pitch = Random.Range(1f - pitchRandomness, 1f + pitchRandomness);
            footstepAudioSource.PlayOneShot(footstepClip);
        }
    }
}