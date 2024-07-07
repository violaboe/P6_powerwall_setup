using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodrayGrower : MonoBehaviour
{
    public float targetYSize = 2.0f;
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
            float newYSize = mainModule.startSizeY.constant + growthSpeed * Time.deltaTime;

            if (newYSize > targetYSize)
            {
                newYSize = targetYSize;
            }

            mainModule.startSizeY = new ParticleSystem.MinMaxCurve(newYSize);
        }
    }
}

