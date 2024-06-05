using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColour : MonoBehaviour
{
    void Start()
    {
        // Get the Renderer component of the cube
        Renderer cubeRenderer = GetComponent<Renderer>();

        // Assign a random color to the cube
        cubeRenderer.material.color = new Color(Random.value, Random.value, Random.value);
    }
}


