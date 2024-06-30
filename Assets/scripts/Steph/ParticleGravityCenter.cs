using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGravityCenter : MonoBehaviour
{
    public ParticleSystem fireflyParticles;
    public Transform gravityCenter;
    public float gravityStrength = 10.0f;
    public float gravityRadius = 5.0f;
    public float pushBackStrength = 20.0f;
    public float pushBackRadius = 1.0f;
    public float targetVelocity = 0.1f;

    private ParticleSystem.Particle[] particles;

    void LateUpdate()
    {
        if (fireflyParticles == null || gravityCenter == null)
            return;

        int maxParticles = fireflyParticles.main.maxParticles;
        if (particles == null || particles.Length < maxParticles)
            particles = new ParticleSystem.Particle[maxParticles];

        int particleCount = fireflyParticles.GetParticles(particles);
        
        Vector3 gravityCenterPosition = gravityCenter.position;
        
        for (int i = 0; i < particleCount; i++)
        {
            Vector3 directionToGravityCenter = gravityCenterPosition - particles[i].position;
            float distanceToGravityCenter = directionToGravityCenter.magnitude;
            
            // Apply gravitational pull within the gravity radius
            if (distanceToGravityCenter < gravityRadius)
            {
                particles[i].velocity += directionToGravityCenter.normalized * gravityStrength * Time.deltaTime;
            }
            
            // Apply push-back force if particles get too close to the gravity center
            if (distanceToGravityCenter < pushBackRadius)
            {
                particles[i].velocity += -directionToGravityCenter.normalized * pushBackStrength * Time.deltaTime;
            }
        }

        fireflyParticles.SetParticles(particles, particleCount);
    }

    public void ApplyPullingForce(float duration = 10.0f)
    {
        if (fireflyParticles == null || gravityCenter == null)
            return;

        StartCoroutine(PullParticles(duration));
    }

    private IEnumerator PullParticles(float duration)
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = gravityCenter.position;

        while (elapsedTime < duration)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].position = Vector3.Lerp(particles[i].position, originalPosition, elapsedTime / duration);
                particles[i].velocity = Vector3.Lerp(particles[i].velocity, Vector3.one * targetVelocity, elapsedTime / duration);
            }

            fireflyParticles.SetParticles(particles, particles.Length);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure particles are exactly at the gravity center's position at the end of the pull
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].position = originalPosition;
            particles[i].velocity = Vector3.one * targetVelocity;
        }

        fireflyParticles.SetParticles(particles, particles.Length);
        Debug.Log("FIREFLIES PULLED");
    }

    public void SetDampen(float dampenValue)
    {
        if (fireflyParticles == null)
            return;

        var mainModule = fireflyParticles.main;
        var limitVelocityModule = fireflyParticles.limitVelocityOverLifetime;

        // Set dampen value in Limit Velocity Over Lifetime module
        limitVelocityModule.dampen = dampenValue;
    }
}
