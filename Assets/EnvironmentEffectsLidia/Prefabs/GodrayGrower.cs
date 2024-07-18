using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodrayGrower : MonoBehaviour
{
    public float targetYSize = 2.0f;
    public float targetXSize = 2.0f;
    public float growthSpeed = 0.1f;

    private ParticleSystem particleSystem;
    private ParticleSystem.MainModule mainModule;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            mainModule = particleSystem.main;
        }
        else
        {
            Debug.LogError("Particle system not found!");
        }
    }

    void Update()
    {
        if (mainModule.startSizeY.constant < targetYSize)
        {
            float newYSize = mainModule.startSizeY.constant + growthSpeed * Time.deltaTime;  //growth speed

            if (newYSize > targetYSize)     
            {
                newYSize = targetYSize;              //targetsize Y
            }

            mainModule.startSizeY = new ParticleSystem.MinMaxCurve(newYSize);
        }

        if (mainModule.startSizeX.constant < targetXSize)
        {
            float newXSize = mainModule.startSizeX.constant + growthSpeed * Time.deltaTime;

            if (newXSize > targetXSize)
            {
                newXSize = targetXSize;             //targetsize X
            }

            mainModule.startSizeX = new ParticleSystem.MinMaxCurve(newXSize);
        }
    }
}

