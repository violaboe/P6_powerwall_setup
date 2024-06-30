using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownByBeat : MonoBehaviour
{
   
    public float sensitivity = 10.0f; // Sensitivity to audio spectrum
    public float lerpSpeed = 2.0f;    // Speed of movement interpolation
    public AudioSource audioSource;
    private float[] spectrumData = new float[64];
    private Vector3 originalPosition;
    private bool isBeatDetected = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.position;
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            DetectBeat();
            MoveObject();
        }
    }

    void DetectBeat()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);
        float sum = 0f;

        for (int i = 0; i < spectrumData.Length; i++)
        {
            sum += spectrumData[i];
        }

        if (sum > sensitivity)
        {
            isBeatDetected = true;
        }
        else
        {
            isBeatDetected = false;
        }
    }

    void MoveObject()
    {
        if (isBeatDetected)
        {
            // Move up
            Vector3 targetPosition = originalPosition + Vector3.up * 0.5f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        }
        else
        {
            // Move back to original position
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * lerpSpeed);
        }
    }

}
