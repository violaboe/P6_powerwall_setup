using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererbyBeat : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the audio source playing the music
    public GameObject targetObject; // The object to toggle visibility on and off
    public int band = 1; // Frequency band for beat detection (0-7)
    public float sensitivity = 1.0f; // Sensitivity threshold for beat detection
    public float minBeatInterval = 0.2f; // Minimum interval between beats to avoid rapid toggling

    private float[] spectrumData = new float[512]; // Array to store spectrum data
    private float lastBeatTime = 0f; // Time of the last detected beat

    void Start()
    {
        if (audioSource == null || targetObject == null)
        {
            Debug.LogError("AudioSource or targetObject not assigned.");
            enabled = false;
            return;
        }

        targetObject.SetActive(false); // Start with the target object off
    }

    void Update()
    {
        AnalyzeAudio();
    }

    void AnalyzeAudio()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris); // Get the spectrum data from the audio source

        float sum = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            if (i >= GetFrequencyRange(band).x && i <= GetFrequencyRange(band).y)
            {
                sum += spectrumData[i];
            }
        }

        float average = sum / (GetFrequencyRange(band).y - GetFrequencyRange(band).x); // Calculate the average amplitude for the specified band

        if (average > sensitivity && Time.time - lastBeatTime > minBeatInterval)
        {
            lastBeatTime = Time.time; // Update the time of the last detected beat
            ToggleTargetObject(); // Toggle the target object's visibility
        }
    }

    Vector2 GetFrequencyRange(int band)
    {
        // Define the frequency ranges for each band (this can be adjusted)
        switch (band)
        {
            case 0: return new Vector2(0, 4);
            case 1: return new Vector2(5, 15);
            case 2: return new Vector2(16, 36);
            case 3: return new Vector2(37, 80);
            case 4: return new Vector2(81, 150);
            case 5: return new Vector2(151, 300);
            case 6: return new Vector2(301, 500);
            case 7: return new Vector2(501, 511);
            default: return new Vector2(0, 511);
        }
    }

    void ToggleTargetObject()
    {
        targetObject.SetActive(!targetObject.activeSelf); // Toggle the target object's active state
    }
}
