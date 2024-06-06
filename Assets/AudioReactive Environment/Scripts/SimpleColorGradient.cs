using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleColorGradient : MonoBehaviour
{
    public Color startColor = Color.red;
    public Color endColor = Color.blue;
    public float duration = 2.0f;

    private Material cubeMaterial;
    private float lerpTime;

    void Start()
    {
        // Ensure the GameObject has a MeshRenderer component
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer component is missing.");
            return;
        }

        // Get the material of the MeshRenderer
        cubeMaterial = meshRenderer.material;

        // Initialize lerpTime
        lerpTime = 0.0f;
    }

    void Update()
    {
        if (cubeMaterial == null) return;

        // Update the lerp time based on duration
        lerpTime += Time.deltaTime / duration;
        if (lerpTime > 1.0f) lerpTime = 0.0f; // Reset lerp time to create a loop

        // Calculate the color based on lerpTime
        Color currentColor = Color.Lerp(startColor, endColor, lerpTime);

        // Apply the current color to the material
        cubeMaterial.color = currentColor;
    }
}

