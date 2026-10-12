using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina System")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDecreaseRate = 20f;
    [SerializeField] private float staminaIncreaseRate = 15f;
    [SerializeField] private float staminaRecoveryDelay = 0.5f;
    [SerializeField] private float currentStamina;

    [Header("Dependencies")]
    [SerializeField] private PlayerHunger hungerSystem;
    [SerializeField] private PlayerMovement movementSystem;

    public System.Action<float> OnStaminaChanged;

    public float GetCurrentStamina => currentStamina;
    public float GetStaminaPercentage => currentStamina / maxStamina;

    private float timeSinceLastSprint;

    public void AddStamina(float amount)
    {
        float effectiveMaxStamina = maxStamina * (hungerSystem.GetHungerPercentage);
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, effectiveMaxStamina);
        OnStaminaChanged?.Invoke(currentStamina / maxStamina);
    }

    private void Start()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {
        HandleStamina();
    }

    private void HandleStamina()
    {
        float effectiveMaxStamina = maxStamina * (hungerSystem.GetHungerPercentage);

        if (movementSystem.IsSprinting)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            timeSinceLastSprint = 0f;
        }
        else
        {
            timeSinceLastSprint += Time.deltaTime;

            if (timeSinceLastSprint >= staminaRecoveryDelay)
            {
                currentStamina += staminaIncreaseRate * Time.deltaTime;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, effectiveMaxStamina);
        OnStaminaChanged?.Invoke(currentStamina / maxStamina);
    }
}