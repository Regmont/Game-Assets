using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    [Header("Hunger System")]
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float hungerDecreaseRate = 0.5f;
    [SerializeField] private float currentHunger;
    [SerializeField] private float hungerDamageCooldown = 2f;

    [Header("Hunger Shake Settings")]
    [SerializeField] private float hungerShakeStartThreshold = 70f;
    [SerializeField] private float hungerShakeMaxThreshold = 30f;
    [SerializeField] private float maxHungerShakeIntensity = 0.15f;
    [SerializeField] private float hungerShakeFrequency = 3f;

    public System.Action<float> OnHungerChanged;

    public float GetCurrentHunger => currentHunger;
    public float GetHungerPercentage => currentHunger / maxHunger;
    public bool IsStarving => currentHunger <= 0f;

    private float timeSinceLastHungerDamage;
    private float hungerShakeTimer;
    private Vector3 hungerShakeOffset;

    public Vector3 HungerShakeOffset => hungerShakeOffset;

    public void AddHunger(float amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger);
        OnHungerChanged?.Invoke(currentHunger / maxHunger);
    }

    public bool ShouldTakeHungerDamage()
    {
        if (!IsStarving) return false;

        timeSinceLastHungerDamage += Time.deltaTime;
        if (timeSinceLastHungerDamage >= hungerDamageCooldown)
        {
            timeSinceLastHungerDamage = 0f;
            return true;
        }
        return false;
    }

    private void Start()
    {
        currentHunger = maxHunger;
    }

    private void Update()
    {
        HandleHunger();
        HandleHungerScreenShake();
    }

    private void HandleHunger()
    {
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
        OnHungerChanged?.Invoke(currentHunger / maxHunger);
    }

    private void HandleHungerScreenShake()
    {
        float hungerShakeIntensity = CalculateHungerShakeIntensity();

        if (hungerShakeIntensity > 0)
        {
            hungerShakeTimer += Time.deltaTime * hungerShakeFrequency;

            float xShake = Mathf.PerlinNoise(hungerShakeTimer, 0) * 2f - 1f;
            float yShake = Mathf.PerlinNoise(0, hungerShakeTimer) * 2f - 1f;
            float zShake = Mathf.PerlinNoise(hungerShakeTimer, hungerShakeTimer) * 2f - 1f;

            hungerShakeOffset = new Vector3(xShake, yShake, zShake) * hungerShakeIntensity;
        }
        else
        {
            hungerShakeOffset = Vector3.zero;
        }
    }

    private float CalculateHungerShakeIntensity()
    {
        if (currentHunger > hungerShakeStartThreshold)
        {
            return 0f;
        }

        if (currentHunger <= hungerShakeMaxThreshold)
        {
            return maxHungerShakeIntensity;
        }

        float hungerRange = hungerShakeStartThreshold - hungerShakeMaxThreshold;
        float currentRange = hungerShakeStartThreshold - currentHunger;
        float intensityRatio = currentRange / hungerRange;

        return Mathf.Lerp(0f, maxHungerShakeIntensity, intensityRatio);
    }
}