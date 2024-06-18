using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMeshEmitter : MonoBehaviour
{
    public ParticleSystem fireflyParticles1;
    public ParticleSystem fireflyParticles2;
    public Mesh customMesh;
    public float meshEmissionDuration = 0.8f;

    private ParticleGravityCenter particleGravityCenter1;
    private ParticleGravityCenter particleGravityCenter2;
    private float elapsedTime = 0f;
    private bool isScattering = false;

    void Start()
    {
        if (fireflyParticles1 == null || fireflyParticles2 == null)
        {
            Debug.LogError("Particle system not assigned!");
            return;
        }

        if (customMesh == null)
        {
            Debug.LogError("Custom mesh not assigned!");
            return;
        }

        particleGravityCenter1 = fireflyParticles1.GetComponentInChildren<ParticleGravityCenter>();
        particleGravityCenter2 = fireflyParticles2.GetComponentInChildren<ParticleGravityCenter>();

        if (particleGravityCenter1 != null)
            particleGravityCenter1.enabled = false;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles1 children!");

        if (particleGravityCenter2 != null)
            particleGravityCenter2.enabled = false;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles2 children!");

        SetupParticleSystem(fireflyParticles1);
        SetupParticleSystem(fireflyParticles2);

        fireflyParticles1.Play();
        fireflyParticles2.Play();
    }

    void SetupParticleSystem(ParticleSystem ps)
    {
        var shapeModule = ps.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Mesh;
        shapeModule.mesh = customMesh;

        var velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;

        var forceModule = ps.forceOverLifetime;
        forceModule.enabled = true;
        forceModule.x = new ParticleSystem.MinMaxCurve(2.0f, -1.0f);
        forceModule.y = new ParticleSystem.MinMaxCurve(2.0f, -1.0f);
        forceModule.z = new ParticleSystem.MinMaxCurve(2.0f, -2.0f);

        var mainModule = ps.main;
        mainModule.gravityModifier = new ParticleSystem.MinMaxCurve(0.0f, 0.0f);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= meshEmissionDuration && !isScattering)
        {
            ScatterParticles();
            isScattering = true;
        }
    }

    void ScatterParticles()
    {
        if (particleGravityCenter1 != null)
            particleGravityCenter1.enabled = true;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles1 children!");

        if (particleGravityCenter2 != null)
            particleGravityCenter2.enabled = true;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles2 children!");

        // Modify properties for scattering particles
        ModifyParticleSystemForScatter(fireflyParticles1);
        ModifyParticleSystemForScatter(fireflyParticles2);
    }

    void ModifyParticleSystemForScatter(ParticleSystem ps)
    {
        var velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;
        velocityModule.x = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);
        velocityModule.y = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);
        velocityModule.z = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);

        var emission = ps.emission;
        emission.rateOverTime = 0;

        var shapeModule = ps.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Sphere;
        shapeModule.radius = 0.5f;
    }
}
