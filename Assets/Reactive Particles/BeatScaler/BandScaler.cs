using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BandScaler : MonoBehaviour
{
    public AudioSource audioSource; // The AudioSource component
    public GameObject[] objectsToScale; // Array of objects to scale
    public float scaleMultiplier = 10.0f; // How much to scale the objects
    public float beatSensitivity = 1.5f; // Sensitivity for detecting beats
    public float scaleDuration = 0.1f; // Duration of the scale effect

    private float[] spectrumData;
    private float[] previousSpectrumData;
    private bool[] isScaling;
    private float[] scaleTimers;
    private int numSamples = 512; // Number of samples to analyze
    private int[] frequencyBands; // Indices of frequency bands for each object

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        spectrumData = new float[numSamples];
        previousSpectrumData = new float[numSamples];
        isScaling = new bool[objectsToScale.Length];
        scaleTimers = new float[objectsToScale.Length];

        // Initialize frequency bands
        frequencyBands = new int[objectsToScale.Length];
        for (int i = 0; i < objectsToScale.Length; i++)
        {
            frequencyBands[i] = Random.Range(0, 8); // Assign a random band index (0-7)
        }
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        for (int i = 0; i < objectsToScale.Length; i++)
        {
            if (IsBeatDetected(frequencyBands[i]))
            {
                isScaling[i] = true;
                scaleTimers[i] = 0;
            }

            if (isScaling[i])
            {
                ScaleObject(i);
            }
        }

        // Store current spectrum data for next frame comparison
        for (int i = 0; i < numSamples; i++)
        {
            previousSpectrumData[i] = spectrumData[i];
        }
    }

    bool IsBeatDetected(int bandIndex)
    {
        int bandStart = GetBandStartIndex(bandIndex);
        int bandEnd = GetBandEndIndex(bandIndex);
        float currentEnergy = 0;
        float previousEnergy = 0;

        for (int i = bandStart; i <= bandEnd; i++)
        {
            currentEnergy += spectrumData[i] * spectrumData[i];
            previousEnergy += previousSpectrumData[i] * previousSpectrumData[i];
        }

        // Detect beat based on energy difference in the frequency band
        return (currentEnergy > previousEnergy * beatSensitivity);
    }

    void ScaleObject(int index)
    {
        scaleTimers[index] += Time.deltaTime;

        if (scaleTimers[index] < scaleDuration)
        {
            objectsToScale[index].transform.localScale = Vector3.one * scaleMultiplier;
        }
        else
        {
            objectsToScale[index].transform.localScale = Vector3.one;
            isScaling[index] = false;
        }
    }

    int GetBandStartIndex(int bandIndex)
    {
        switch (bandIndex)
        {
            case 0: return 0;
            case 1: return 1;
            case 2: return 2;
            case 3: return 4;
            case 4: return 8;
            case 5: return 16;
            case 6: return 32;
            case 7: return 64;
            default: return 0;
        }
    }

    int GetBandEndIndex(int bandIndex)
    {
        switch (bandIndex)
        {
            case 0: return 1;
            case 1: return 2;
            case 2: return 4;
            case 3: return 8;
            case 4: return 16;
            case 5: return 32;
            case 6: return 64;
            case 7: return 128;
            default: return 1;
        }
    }
}

