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
    private float elapsedTime = 0f;
    private bool isScattering = false;

    void Start()
    {
        ParticleSystem[] fireflyParticles = GetComponentsInChildren<ParticleSystem>();
        /*if (fireflyParticles1 == null || fireflyParticles2 == null || fireflyParticles3 == null)
        {
            Debug.LogError("Particle system not assigned!");
            return;
        }*/

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
        velocityModule.x = new ParticleSystem.MinMaxCurve(-1.0f, 2.0f);
        velocityModule.y = new ParticleSystem.MinMaxCurve(1.0f, 3.0f);
        velocityModule.z = new ParticleSystem.MinMaxCurve(-1.0f, 1.0f);

        var forceModule = ps.forceOverLifetime;
        forceModule.enabled = true;
        forceModule.x = new ParticleSystem.MinMaxCurve(-1.0f, 1.0f);
        forceModule.y = new ParticleSystem.MinMaxCurve(-1.0f, 1.0f);
        forceModule.z = new ParticleSystem.MinMaxCurve(-1.0f, 1.0f);

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
        // Modify properties for scattering particles
        InBetweenScatter(fireflyParticles1);
        InBetweenScatter(fireflyParticles2);
        InBetweenScatter(fireflyParticles3);
        //InBetweenScatter(fireflyParticles4);
    }

    // The scatter that happens in between Starting Path & Paths around the user
    void InBetweenScatter(ParticleSystem ps)
    {

        var forceModule = ps.forceOverLifetime;
        forceModule.enabled = true;
        forceModule.x = new ParticleSystem.MinMaxCurve(1.0f, 2.0f);
        forceModule.y = new ParticleSystem.MinMaxCurve(1.0f, 2.0f);
        forceModule.z = new ParticleSystem.MinMaxCurve(1.0f, 2.0f);

        var velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;
        velocityModule.x = new ParticleSystem.MinMaxCurve(1.0f, 7.0f);
        velocityModule.y = new ParticleSystem.MinMaxCurve(1.0f, 3.0f);
        velocityModule.z = new ParticleSystem.MinMaxCurve(1.0f, 7.0f);

        /*ParticleGravityCenter[] gravityCenters = GetComponentsInChildren<ParticleGravityCenter>();
        foreach (var gravityCenter in gravityCenters)
        {
            if (gravityCenter != null)
            {
                gravityCenter.SetDampen(0f);
                Debug.Log("Set dampen value to 0");
            }
        }*/

        /*StartCoroutine(SetPSMiddlePathWithDelay(fireflyParticles1));
        StartCoroutine(SetPSMiddlePathWithDelay(fireflyParticles2));
        StartCoroutine(SetPSMiddlePathWithDelay(fireflyParticles3));*/

        SetPSMiddlePath(fireflyParticles1);
        SetPSMiddlePath(fireflyParticles2);
        SetPSMiddlePath(fireflyParticles3);
        
    }

    /*IEnumerator SetPSMiddlePathWithDelay(ParticleSystem ps)
    {
        yield return new WaitForSeconds(0.5f);  // Wait for x seconds before switching to Middle Path
        SetPSMiddlePath(ps);
    }*/

    void SetPSMiddlePath(ParticleSystem ps)
    {

        ParticleGravityCenter[] gravityCenters = GetComponentsInChildren<ParticleGravityCenter>();
        foreach (var gravityCenter in gravityCenters)
        {
            if (gravityCenter != null)
            {
                gravityCenter.enabled = true;

            }
        }

        var velocityModule = ps.velocityOverLifetime;
        velocityModule.enabled = true;
        velocityModule.x = new ParticleSystem.MinMaxCurve(0.0f, 3.0f);
        velocityModule.y = new ParticleSystem.MinMaxCurve(0.0f, 3.0f);
        velocityModule.z = new ParticleSystem.MinMaxCurve(0.0f, 3.0f);

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
}
