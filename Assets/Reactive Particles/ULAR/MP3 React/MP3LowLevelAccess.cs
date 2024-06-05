using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Welcome to the Low Level Access of the MP3 Reactor.
// While you can edit things here, I don't recommend it a lot. Unless you know what is going on here.
// I may change things here in the future, but rest asured, the function names, inputs and way of use will remain the same. 
// So you don't have to worry much about adapting your projects. :)

// This is the Low Level for the MP3 Reactor. So we need a audio source.

namespace ULAR {
        
    [RequireComponent (typeof (AudioSource))]
    public class MP3LowLevelAccess : MonoBehaviour
    {
        // Stuff. Ill be honest, its been a while since I coded most of this up. I forgot quiet a lot by now, but don't worry. It still works
        public AudioSource AUDIOSOURCE;
        public float[] SAMPLES = new float[512];
        float[] FreqBands = new float[8];
        float[] BandBuffer = new float[8];
        float[] BufferDecrease = new float[8];

        float[] FreqBandHighest = new float[8];
        public static float[] AudioBand = new float[8];
        public static float[] AudioBandBuffer = new float[8];

        public float BufferDecreaseValue = 0.005f;
        public float BufferDecreaseDiscount = 1.01f;

        public static float Amplitude, AmplitudeBuffer;
        private static float AmplitudeHighest;

        // You know, get the audio source
        void Start()
        {
            AUDIOSOURCE = GetComponent<AudioSource> ();
        }

        // The Live Reactor had a more optimized way to run the functions. 
        // And as of right now, I don't exactly remember why..Im sure I remember some day. And then I can make them run equally efficient
        void Update()
        {
            GetSpectrumAudioSource(); // REQUIRED! Launching the function to fill the SAMPLES Array with Spectrum Data
            MakeFrequencyBands(); // This is only required if you want to use 8 Bands, instead of..whatever else
            BandBufferFunc(); // OPTIONAL! Can only be used if you use 8 Bands. To smooth the movement of the Bands
            CreateNormalizedAudioBands(); // REQUIRED! If you run off the MP3HighLevelAccess Script, that is.
            // Actually, by now the Script has been optimized so much for the 8 frequency bands, that you pretty much need all of these functions.
            // Like I said, just use the HighLevelAccess. Unless you know what you are doing.
        }


        // Getting the Sample Data in the first place
        void GetSpectrumAudioSource() 
        {
            AUDIOSOURCE.GetSpectrumData(SAMPLES, 0, FFTWindow.Blackman);
        }

        // Converting 20.000 Samples into 8 Values
        public void MakeFrequencyBands() 
        {

            /* To compress the 20.000 Samples into 8 bands, we need to grab a certain range, and use their average as the band value
            * The way these ranges are calculated is...too complex to explain it in a few lines of comment. Just take them as they are, it works out.
            * 20-60
            * 60 - 250
            * 250 - 500
            * 500 - 2000
            * 2000 - 4000
            * 4000 - 6000
            * 6000 - 200000
            */

            int count = 0;

            // Start a loop for the eight bands
            for (int i = 0; i < 8; i++) {

                // We start with an averag value of 0. Because..why would we want to add on the average from the beginning?
                float average = 0;

                // This relates to the creation of the different ranges.
                // Like I already said, if you dont know how it works, just..let it do its thing
                int SampleCount = (int)Mathf.Pow(2, i) * 2;
                
                // The line above ends up with 510 Samples, out of 512.
                // To get the whole 512 (which is optional), we simply add 2 samples to the 7th run, which is the 8th band.
                if (i==7){
                    SampleCount += 2;
                }

                // This is the interesting part. We run this loop for the amount of Bands, in this code, eight times.
                // This specific loop then runs as many times as SampleCount is. Which we created with the "dont question it" line. 
                for (int j = 0; j < SampleCount; j++){

                    // We get the correct Sample with the count index, since the variable count is going to be the amount of samples in the end
                    // Then we multiply that with the count + 1, honestly, I dont know why :P
                    average += SAMPLES[count] * (count + 1);

                    // Then we increase the count, for obvious reason. (to go through the samples in correct sync)
                    count++;
                }

                // After each Calculation, for each Band, we divide the whole average by the count, which creates the average in the first place
                // If we wouldnt do that, the variable average would just be the sum of the previous loops additions
                average /= count;

                // In the end, we set the Frequency Band to be the Average (Mulitplied by 10 so its not super small)
                // If you want to amplify the values, you should to that later on when getting the values with the higher level functions
                // I mean, do what you want, change this value if you want, whatever lol. But still.
                FreqBands[i] = average * 10;
            }
        }

        

        // The Function to smooth movements of the 8 Bands
        public void BandBufferFunc()
        {
            // We have 8 Bands, so we run 8 times
            for (int g = 0; g < 8; ++g) {
                // If the Frequency Bands Value is Greater than the Band Buffer Value
                if (FreqBands[g] > BandBuffer[g]) {

                    // Make the Band Buffer the Frequency Band, so they are eqal at first
                    BandBuffer[g] = FreqBands[g];

                    // Each Band gets its own Decrease Value, they are set to the constant value BufferDecreaseValue
                    BufferDecrease[g] = BufferDecreaseValue;
                }

                // If the Frequency Bands Value is Lower than the Band Buffer Value
                // AKA the frequence value is less than the Buffer. We want to go down smoothly then
                if (FreqBands[g] < BandBuffer[g]) {

                    // Subtract the Decrease Value from the Buffer Amount.
                    // It will be a very tiny amount first, depending on what your initial Decrease Value is
                    BandBuffer[g] -= BufferDecrease[g]; 

                    // After we lowered the Buffer by a little bit, we multiply the Decrease Amount we just used
                    // To make it slightly bigger, increasing it each frame, so the value goes down faster and faster each time
                    // Which brings the wanted effect: Go down slowly first, but become faster over time
                    BufferDecrease[g] *= BufferDecreaseDiscount;
                }
            }
        }

        // This function scales every value to be between 0 and 1. This helps alot when wanting to use them for different Effects in Unity
        // Since you can then just scale it however you need it, without having to think about exceeding limits
        public void CreateNormalizedAudioBands() {

            // Once again, we have 8 Bands, so we run 8 times
            for (int i = 0; i < 8; i++) {

                // If the Frequency Band (which is the averaged value after running the MakeFrequencyBands function) is larger than the Largest Value of the Bands Highest FreqBand
                if (FreqBands[i] > FreqBandHighest[i]) {

                    // We Make that Frequency Band value be the highest value 
                    FreqBandHighest[i] = FreqBands[i];
                }

                AudioBand[i] = (FreqBands[i] / FreqBandHighest[i]);
                AudioBandBuffer[i] = (BandBuffer[i] / FreqBandHighest[i]);
            }
        }

        // A function to find the amplitude of all eight bands
        // AKA we get the average value of all eight bands
        public static float GetAmplitude(bool UseBuffer = false) {

            // Initial Variables to start with
            float CurrentAmplitude = 0;
            float CurrentAplitudeBuffer = 0;

            // Iterate through the eight bands
            for (int i = 0; i < 8; i++) {

                // For all eight values, we add them to the variables. One for the buffered values, one for the normal ones
                CurrentAmplitude += AudioBand[i];
                CurrentAplitudeBuffer += AudioBandBuffer[i];
            }

            // This finds the largest value
            if (CurrentAmplitude > AmplitudeHighest) {
                AmplitudeHighest = CurrentAmplitude;
            }

            // With the largest value, we can now average all the eight band values together to be 1
            Amplitude = CurrentAmplitude / AmplitudeHighest;
            AmplitudeBuffer = CurrentAplitudeBuffer / AmplitudeHighest;

            // And then return either the buffered values or the normal ones
            if (UseBuffer) {return AmplitudeBuffer;}
            else {return Amplitude;}
        }

    }
}