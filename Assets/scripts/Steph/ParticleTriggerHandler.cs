using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class ParticleTriggerHandler : MonoBehaviour
{
    public Transform jarLocation;
    public ParticleSystem fireflyParticles;
    public float pushBackStrength = 20.0f;
    public float pushBackRadius = 1.0f;

    private ParticleSystem.Particle[] particles;
    private SphereCollider pushCollider;

    void Start()
    {
        // Add a sphere collider to the XRHand
        pushCollider = gameObject.AddComponent<SphereCollider>();
        pushCollider.isTrigger = true;
        pushCollider.radius = pushBackRadius;
    }

    void LateUpdate()
    {
        if (fireflyParticles == null)
            return;

        int maxParticles = fireflyParticles.main.maxParticles;
        if (particles == null || particles.Length < maxParticles)
            particles = new ParticleSystem.Particle[maxParticles];

        int particleCount = fireflyParticles.GetParticles(particles);

        Vector3 handPosition = transform.position;

        for (int i = 0; i < particleCount; i++)
        {
            Vector3 directionFromHand = particles[i].position - handPosition;
            float distanceFromHand = directionFromHand.magnitude;

            // Apply push-back force if particles are within the push radius of the hand
            if (distanceFromHand < pushBackRadius)
            {
                particles[i].velocity += directionFromHand.normalized * pushBackStrength * Time.deltaTime;
            }
        }

        fireflyParticles.SetParticles(particles, particleCount);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the particle system's particle
        if (other.gameObject == fireflyParticles.gameObject)
        {
            // Here, you can add any specific logic when a particle enters the collider, if needed
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the collider is the particle system's particle
        if (other.gameObject == fireflyParticles.gameObject)
        {
            // Here, you can add any specific logic when a particle exits the collider, if needed
        }
    }
}
