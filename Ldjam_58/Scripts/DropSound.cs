using UnityEngine;

public class PhysicsDropSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip[] dropSounds;
    public AudioSource audioSource;

    [Header("Physics Settings")]
    public float minVelocity = 2f;
    public float maxVelocity = 10f;
    public bool enablePitchVariation = true;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 1f;
                audioSource.playOnAwake = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity > minVelocity)
        {
            PlayDropSound(impactVelocity);
        }
    }

    private void PlayDropSound(float velocity)
    {
        if (audioSource != null && dropSounds != null && dropSounds.Length > 0)
        {
            AudioClip selectedClip = dropSounds[Random.Range(0, dropSounds.Length)];

            float volume = Mathf.InverseLerp(minVelocity, maxVelocity, velocity);
            volume = Mathf.Clamp01(volume);

            if (enablePitchVariation)
            {
                audioSource.pitch = Random.Range(0.95f, 1.05f);
            }
            else
            {
                audioSource.pitch = 1f;
            }

            audioSource.PlayOneShot(selectedClip, volume);

            Debug.Log($"Drop sound played! Velocity: {velocity:F1}, Volume: {volume:F2}");
        }
    }
}