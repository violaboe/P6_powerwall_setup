using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Welcome to the Low Level Access of the Live Reactor.
// While you can edit things here, I don't recommend it a lot. Unless you know what is going on here.
// I may change things here in the future, but rest asured, the function names, inputs and way of use will remain the same. 
// So you don't have to worry much about adapting your projects. :)

// This is the Low Level for the Live Reactor. So we need the Loopback Capture Source
// It is what lets us grab the PC audio. For the MP3 Version, this would be Unitys Audio Source.

namespace ULAR {

    [RequireComponent (typeof (LoopbackCaptureSource))]
    public class LiveLowLevelAccess : MonoBehaviour
    {
        // Stuff. Ill be honest, its been a while since I coded most of this up. I forgot quiet a lot by now, but don't worry. It still works
        static public SpectrumSource AudioIn;
        public static float[] SAMPLES = new float[512];
        static float[] FreqBands = new float[8];
        static float[] BandBuffer = new float[8];
        static float[] BufferDecrease = new float[8];

        static float[] FreqBandHighest = new float[8];
        public static float[] AudioBand = new float[8];
        public static float[] AudioBandBuffer = new float[8];

        public float BufferDecreaseValue =  0.0005f;
        public float BufferDecreaseDiscount = 1.01f;

        public static float BufferDecreasePerFrame;
        public static float BufferDecreaseFalloff;
        
        public static float Amplitude, AmplitudeBuffer;
        private static float AmplitudeHighest;

        // You know, get the audio source
        void Awake() {
            AudioIn = GetComponent<LoopbackCaptureSource>(); 
        }
        
        // While the MP3 Version runs all functions in Update, here we simply use the Update to update 2 Variables.
        // In case they change during runtime. You can change these in the Controller object.
        void Update () {
            BufferDecreasePerFrame = BufferDecreaseValue;
            BufferDecreaseFalloff = BufferDecreaseDiscount; 
        }

        // Getting the Sample Data in the first place
        // This function could be put into the MakeFrequencyBands function, for having less functions
        // But this way it is seperated in a cleaner way
        static void GetSpectrumAudioSource()
        {
            // Listen to Audiosources Spectrum, and put the Data into the SAMPLES Array
            double[] RAWSAMPLES = AudioIn.GetSpectrumData();
            if (RAWSAMPLES.Length > 0) {
                for (int i = 0; i < 512; i++) {
                    SAMPLES[i] = (float) RAWSAMPLES[i];
                }  
            }
        }

        // Converting 20.000 Samples into 8 Values
        static void MakeFrequencyBands() 
        {
            // Like I said, this function could simply exist inside here, instead of being it's own function
            GetSpectrumAudioSource();

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
        static void BandBufferFunc()
        {
            MakeFrequencyBands();
            // We have 8 Bands, so we run 8 times
            for (int g = 0; g < 8; ++g) {
                // If the Frequency Bands Value is Greater than the Band Buffer Value
                if (FreqBands[g] > BandBuffer[g]) {

                    // Make the Band Buffer the Frequency Band, so they are eqal at first
                    BandBuffer[g] = FreqBands[g];

                    // Each Band gets its own Decrease Value, they are set to the constant value BufferDecreasePerFrame
                    BufferDecrease[g] = BufferDecreasePerFrame;
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
                    BufferDecrease[g] *= BufferDecreaseFalloff;
                }
            }
        }

        // This function scales every value to be between 0 and 1. This helps alot when wanting to use them for different Effects in Unity
        // Since you can then just scale it however you need it, without having to think about exceeding limits
        // This function also creates the Audio values for both normal and smoothed. 
        // Which you maybe want in 2 functions, that way you only calculate one of them, since you maybe only use the smoothed ones, and not the normal ones
        // Which is probably something I am going to do at some point...or you do, feel free :P
        static public float CreateNormalizedAudioBands(int Band, bool Smooth) {
            
            BandBufferFunc();
            // Once again, we have 8 Bands, so we run 8 times
            for (int i = 0; i < 8; i++) {

                // If the Frequency Band (which is the averaged value after running the MakeFrequencyBands function) is larger than the Largest Value of the Bands Highest FreqBand
                if (FreqBands[i] > FreqBandHighest[i]) {

                    // We Make that Frequency Band value be the highest value 
                    FreqBandHighest[i] = FreqBands[i];
                }

                // Update the Audio Values, as well as the smoothed ones
                AudioBand[i] = (FreqBands[i] / FreqBandHighest[i]);
                AudioBandBuffer[i] = (BandBuffer[i] / FreqBandHighest[i]);
            }
            // And finally, return either the smoothed audio or the normal one
            if (Smooth) {return AudioBandBuffer[Band];}
            else {return AudioBand[Band];}
            
        }

        // A function to find the amplitude/volume of all eight bands
        // AKA we get the average value of all eight bands
        // This function also calculates the normal and smoothed audio. Again, could be 2 functions for performance gains..maybe
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