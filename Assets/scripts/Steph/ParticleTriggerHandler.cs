using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class ParticleTriggerHandler : MonoBehaviour
{
    public Transform jarLocation;
    public ParticleSystem fireflyParticles1;
    public ParticleSystem fireflyParticles2;
    public float pushBackStrength = 20.0f;
    public float pushBackRadius = 1.0f;

    private ParticleSystem.Particle[] particles1;
    private ParticleSystem.Particle[] particles2;
    private SphereCollider pushCollider;

    public event System.Action<ParticleSystem> OnParticlesPushed;

    void Start()
    {
        // Add a sphere collider to the XRHand
        pushCollider = gameObject.AddComponent<SphereCollider>();
        pushCollider.isTrigger = true;
        pushCollider.radius = pushBackRadius;
    }

    void LateUpdate()
    {
        if (fireflyParticles1 == null && fireflyParticles2 == null)
            return;

        int maxParticles1 = fireflyParticles1.main.maxParticles;
        int maxParticles2 = fireflyParticles2.main.maxParticles;

        if (particles1 == null || particles1.Length < maxParticles1)
            particles1 = new ParticleSystem.Particle[maxParticles1];

        if (particles2 == null || particles2.Length < maxParticles2)
            particles2 = new ParticleSystem.Particle[maxParticles2];

        int particleCount1 = fireflyParticles1.GetParticles(particles1);
        int particleCount2 = fireflyParticles2.GetParticles(particles2);

        Vector3 handPosition = transform.position;

        for (int i = 0; i < particleCount1; i++)
        {
            Vector3 directionFromHand = particles1[i].position - handPosition;
            float distanceFromHand = directionFromHand.magnitude;

            // Apply push-back force if particles are within the push radius of the hand
            if (distanceFromHand < pushBackRadius)
            {
                particles1[i].velocity += directionFromHand.normalized * pushBackStrength * Time.deltaTime;
                OnParticlesPushed?.Invoke(fireflyParticles1);
            }
        }

        fireflyParticles1.SetParticles(particles1, particleCount1);

        for (int i = 0; i < particleCount2; i++)
        {
            Vector3 directionFromHand = particles2[i].position - handPosition;
            float distanceFromHand = directionFromHand.magnitude;

            // Apply push-back force if particles are within the push radius of the hand
            if (distanceFromHand < pushBackRadius)
            {
                particles2[i].velocity += directionFromHand.normalized * pushBackStrength * Time.deltaTime;
                OnParticlesPushed?.Invoke(fireflyParticles2);
            }
        }

        fireflyParticles2.SetParticles(particles2, particleCount2);
    }
}
