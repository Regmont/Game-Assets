using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    //Light light;

    float duration = 1f;
    float targetIntensity = 100f;
    float currentIntensity = 0f;
    float elapsedTime = 0f;

    void Start()
    {
        //light = GetComponent<Light>();

        //light.intensity = elapsedTime;

        Destroy(gameObject, duration);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        float currentIntensity;

        if (elapsedTime < duration / 2)
        {
            currentIntensity = Mathf.Lerp(0, targetIntensity, elapsedTime * 2 / duration);
        }
        else
        {
            currentIntensity = Mathf.Lerp(targetIntensity, 0, (elapsedTime - duration / 2) * 2 / duration);
        }

        //light.intensity = currentIntensity;
    }
}