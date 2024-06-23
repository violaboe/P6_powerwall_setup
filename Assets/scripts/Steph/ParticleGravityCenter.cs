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

    public void ApplyPullingForce()
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
            particles[i].position = gravityCenterPosition;
            particles[i].velocity = Vector3.zero;
        }

        fireflyParticles.SetParticles(particles, particleCount);
    }
}
