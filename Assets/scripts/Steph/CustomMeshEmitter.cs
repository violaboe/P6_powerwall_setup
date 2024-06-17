using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMeshEmitter : MonoBehaviour
{
    public ParticleSystem fireflyParticles1; // Reference to your Particle System
    public ParticleSystem fireflyParticles2;
    //public ParticleSystem fireflyParticles3;
    public Mesh customMesh; // Reference to your custom mesh
    public float meshEmissionDuration = 0.8f;

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

        var shapeModule1 = fireflyParticles1.shape; // Get the ShapeModule of the particle system
        shapeModule1.shapeType = ParticleSystemShapeType.Mesh; // Set the shape type to Mesh
        shapeModule1.mesh = customMesh; // Assign the custom mesh

        var shapeModule2 = fireflyParticles2.shape; // Get the ShapeModule of the particle system
        shapeModule2.shapeType = ParticleSystemShapeType.Mesh; // Set the shape type to Mesh
        shapeModule2.mesh = customMesh; // Assign the custom mesh

        /*var shapeModule3 = fireflyParticles2.shape; // Get the ShapeModule of the particle system
        shapeModule3.shapeType = ParticleSystemShapeType.Mesh; // Set the shape type to Mesh
        shapeModule3.mesh = customMesh; // Assign the custom mesh*/

        /*
        var velocityModule = fireflyParticles1.velocityOverLifetime;
        velocityModule.enabled = true;

        var forceModule = fireflyParticles1.forceOverLifetime;
        velocityModule.enabled = true;

        forceModule.x = new ParticleSystem.MinMaxCurve(2.0f, -1.0f);
        forceModule.y = new ParticleSystem.MinMaxCurve(2.0f, -1.0f);
        forceModule.z = new ParticleSystem.MinMaxCurve(2.0f, -2.0f);

        var mainModule = fireflyParticles1.main;
        mainModule.gravityModifier = new ParticleSystem.MinMaxCurve(0.0f, 0.0f);*/

    }
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= meshEmissionDuration && !isScattering)
        {
            ScatterParticles();
            isScattering = true;

        }


        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            fireflyParticles1.Play();
        }

    }

    void ScatterParticles()
    {
        var velocityModule1 = fireflyParticles1.velocityOverLifetime;
        velocityModule1.enabled = true;

        velocityModule1.x = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);
        velocityModule1.y = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);
        velocityModule1.z = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);

        var emission1 = fireflyParticles1.emission;
        emission1.rateOverTime = 0;

        var shapeModule1 = fireflyParticles1.shape;
        shapeModule1.shapeType = ParticleSystemShapeType.Sphere;
        shapeModule1.radius = 0.5f;

        var velocityModule2 = fireflyParticles2.velocityOverLifetime;
        velocityModule2.enabled = true;

        velocityModule2.x = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);
        velocityModule2.y = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);
        velocityModule2.z = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);

        var emission2 = fireflyParticles2.emission;
        emission2.rateOverTime = 0;

        var shapeModule2 = fireflyParticles2.shape;
        shapeModule2.shapeType = ParticleSystemShapeType.Sphere;
        shapeModule2.radius = 0.5f;

    }
}
