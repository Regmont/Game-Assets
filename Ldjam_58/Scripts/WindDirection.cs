using UnityEngine;
using System.Collections;

public class ParticleWindDirection : MonoBehaviour
{
    [Header("Direction Settings")]
    [SerializeField] private float directionUpdateRate = 0.1f;
    [SerializeField] private float steerStrength = 2f;
    [SerializeField] private float maxSteerForce = 3f;

    private ParticleSystem windParticleSystem;
    private ParticleSystem.Particle[] particles;
    private Vector3 targetDirection;
    private Coroutine directionCoroutine;

    private void Start()
    {
        InitializeParticleSystem();
        InitializeParticleArray();
        StartDirectionUpdate();
    }

    private void InitializeParticleSystem()
    {
        windParticleSystem = GetComponent<ParticleSystem>();

        if (windParticleSystem == null)
        {
            Debug.LogError("ParticleWindDirection: No ParticleSystem found on this GameObject");
        }
    }

    private void InitializeParticleArray()
    {
        if (windParticleSystem != null)
        {
            particles = new ParticleSystem.Particle[windParticleSystem.main.maxParticles];
        }
    }

    private void StartDirectionUpdate()
    {
        if (windParticleSystem != null)
        {
            directionCoroutine = StartCoroutine(UpdateDirectionCoroutine());
        }
    }

    private void Update()
    {
        if (windParticleSystem == null) return;

        int numParticles = windParticleSystem.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            SteerParticle(ref particles[i]);
        }

        windParticleSystem.SetParticles(particles, numParticles);
    }

    private IEnumerator UpdateDirectionCoroutine()
    {
        while (true)
        {
            UpdateTargetDirection();
            yield return new WaitForSeconds(directionUpdateRate);
        }
    }

    private void UpdateTargetDirection()
    {
        targetDirection = RelicManager.Instance.GetDirectionToNearestRelic(transform.position);

        if (targetDirection == Vector3.zero)
        {
            targetDirection = Vector3.forward;
        }
    }

    private void SteerParticle(ref ParticleSystem.Particle particle)
    {
        if (targetDirection == Vector3.zero)
        {
            return;
        }

        float currentSpeed = particle.velocity.magnitude;
        Vector3 desiredVelocity = targetDirection * currentSpeed;

        Vector3 steer = (desiredVelocity - particle.velocity) * steerStrength;
        steer = Vector3.ClampMagnitude(steer, maxSteerForce);

        particle.velocity += steer * Time.deltaTime;
    }

    private void OnDisable()
    {
        if (directionCoroutine != null)
        {
            StopCoroutine(directionCoroutine);
        }
    }
}