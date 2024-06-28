using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMeshEmitter : MonoBehaviour
{
    public ParticleSystem fireflyParticles1;
    
    public ParticleSystem fireflyParticles2;
    public ParticleSystem fireflyParticles3;
    //public ParticleSystem fireflyParticles4;
    private ParticleSystem.Particle[] singularParticle;
    public Mesh customMesh;
    public float meshEmissionDuration = 0.8f;

    private ParticleGravityCenter particleGravityCenter1;
    private ParticleGravityCenter particleGravityCenter2;
    private ParticleGravityCenter particleGravityCenter3;
    //private ParticleGravityCenter particleGravityCenter4;
    private float elapsedTime = 0f;
    private bool isScattering = false;

    void Start()
    {
        if (fireflyParticles1 == null || fireflyParticles2 == null || fireflyParticles3 == null)
        {
            Debug.LogError("Particle system not assigned!");
            return;
        }

        if (customMesh == null)
        {
            Debug.LogError("Custom mesh not assigned!");
            return;
        }

        ParticleGravityCenter[] gravityCenters = GetComponentsInChildren<ParticleGravityCenter>();
        foreach (var gravityCenter in gravityCenters)
        {
            if (gravityCenter != null)
            {
                gravityCenter.enabled = false;

            }
        }
        
        /*particleGravityCenter1 = fireflyParticles1.GetComponentInChildren<ParticleGravityCenter>();
        particleGravityCenter2 = fireflyParticles2.GetComponentInChildren<ParticleGravityCenter>();
        particleGravityCenter3 = fireflyParticles3.GetComponentInChildren<ParticleGravityCenter>();
        //particleGravityCenter4 = fireflyParticles4.GetComponentInChildren<ParticleGravityCenter>();

        if (particleGravityCenter1 != null)
            particleGravityCenter1.enabled = false;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles1 children!");

        if (particleGravityCenter2 != null)
            particleGravityCenter2.enabled = false;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles2 children!");

        if (particleGravityCenter3 != null)
            particleGravityCenter3.enabled = false;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles3 children!");

        /*if (particleGravityCenter4 != null)
            particleGravityCenter4.enabled = false;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles4 children!");*/

        StartParticleSystem(fireflyParticles1);
        StartParticleSystem(fireflyParticles2);
        StartParticleSystem(fireflyParticles3);
        //StartParticleSystem(fireflyParticles4);

        fireflyParticles1.Play();
        fireflyParticles2.Play();
        fireflyParticles3.Play();
        //fireflyParticles4.Play();
    }

    void StartParticleSystem(ParticleSystem ps)
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
        forceModule.z = new ParticleSystem.MinMaxCurve(1.0f, -1.0f);

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
        /*if (particleGravityCenter1 != null)
            particleGravityCenter1.enabled = true;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles1 children!");

        if (particleGravityCenter2 != null)
            particleGravityCenter2.enabled = true;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles2 children!");

        if (particleGravityCenter3 != null)
            particleGravityCenter3.enabled = true;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles3 children!");

        if (particleGravityCenter4 != null)
            particleGravityCenter4.enabled = true;
        else
            Debug.LogError("ParticleGravityCenter component not found in fireflyParticles4 children!");*/


        // Modify properties for scattering particles
        SetPSMiddlePath(fireflyParticles1);
        SetPSMiddlePath(fireflyParticles2);
        SetPSMiddlePath(fireflyParticles3);
        //SetPSMiddlePath(fireflyParticles4);
    }

    void InBetweenScatter(ParticleSystem ps)
    {

        var forceModule = ps.forceOverLifetime;
        forceModule.enabled = true;
        forceModule.x = new ParticleSystem.MinMaxCurve(2.0f, -1.0f);
        forceModule.y = new ParticleSystem.MinMaxCurve(2.0f, -1.0f);
        forceModule.z = new ParticleSystem.MinMaxCurve(1.0f, -1.0f);

        var velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;
        velocityModule.x = new ParticleSystem.MinMaxCurve(-1.0f, 3.0f);
        velocityModule.y = new ParticleSystem.MinMaxCurve(-1.0f, 3.0f);
        velocityModule.z = new ParticleSystem.MinMaxCurve(-1.0f, 5.0f);


    }

    void SetPSMiddlePath(ParticleSystem ps)
    {
        var velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;
        velocityModule.x = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);
        velocityModule.y = new ParticleSystem.MinMaxCurve(0.0f, 3.0f);
        velocityModule.z = new ParticleSystem.MinMaxCurve(0.0f, 1.0f);

        var emission = ps.emission;
        emission.rateOverTime = 0;

        var shapeModule = ps.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Sphere;
        shapeModule.radius = 0.5f;
    }

    public void endPathMesh(ParticleSystem ps)
    {
        var shapeModule = ps.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Box;
        //shapeModule.radius = 0.05f;
        shapeModule.scale = new Vector3(0.1f, 0.1f, 0.1f);
        Debug.Log("Changed shape to BoxEdge with radius 0.05 and rotation 90 degrees");
    }

    /*public void RevertToSphere(ParticleSystem ps)
    {
        var shapeModule = ps.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Sphere;
        shapeModule.radius = 0.5f;
        Debug.Log("Reverted shape to Sphere with radius 0.5");
    }*/
}
