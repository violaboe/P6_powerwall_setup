using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlecontroller : MonoBehaviour
{
    public ParticleSystem[] particleSystems; // Array to hold the particle systems
    public AudioSource audioSource; // Reference to the audio source
    public int bandIndex = 0; // Index of the frequency band to monitor (0-7)
    public float threshold = 0.1f; // Threshold for activating/deactivating particle systems

    private float[] spectrumData = new float[64]; // Array to hold spectrum data
    private bool isActive = false; // Boolean to track the active state

    void Update()
    {
        // Get spectrum data from the audio source
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        // Check if the band value exceeds the threshold
        if (spectrumData[bandIndex] > threshold)
        {
            if (!isActive)
            {
                ToggleParticles(true);
                isActive = true;
            }
        }
        else
        {
            if (isActive)
            {
                ToggleParticles(false);
                isActive = false;
            }
        }
    }

    void ToggleParticles(bool state)
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps != null)
            {
                if (state)
                {
                    ps.Play();
                }
                else
                {
                    ps.Stop();
                }
            }
        }
    }
}
