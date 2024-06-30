using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyFlickering : MonoBehaviour
{
    public Material fireflyMaterial;
    public Material yellowMaterial;
    public float flickerSpeed = 0.1f; // Speed at which the material changes
    public float flickerDuration = 2.0f; // Number of flickers

    public IEnumerator FlickerAndChangeMaterial(List<ParticleSystem> particleSystems)
    {
        Debug.Log("FlickerAndChangeMaterial started"); 

        float totalFlickerTime = flickerDuration; // Total duration for one complete flicker cycle
        int flickerCount = Mathf.RoundToInt(totalFlickerTime / flickerSpeed); // Calculate flicker count based on total time

        Debug.Log($"FlickerAndChangeMaterial started with {flickerCount} flickers");
        for (int i = 0; i < flickerCount; i++)
        {
            // Iterate through each ParticleSystem and update the material in the renderer module
            Debug.Log($"Flicker cycle {i + 1}");
            foreach (ParticleSystem ps in particleSystems)
            {
                var renderer = ps.GetComponent<Renderer>();
                renderer.material = (i % 2 == 0) ? fireflyMaterial : yellowMaterial;
            }

            Debug.Log("Flickering now"); 
            yield return new WaitForSeconds(flickerSpeed);
        }

        // After flickering, set the material to yellowMaterial for each ParticleSystem
        foreach (ParticleSystem ps in particleSystems)
        {
            var renderer = ps.GetComponent<Renderer>();
            renderer.material = fireflyMaterial;
        }

        Debug.Log("FlickerAndChangeMaterial completed"); 
    }
}
