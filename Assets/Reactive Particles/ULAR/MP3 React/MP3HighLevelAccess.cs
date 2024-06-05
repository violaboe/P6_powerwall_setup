using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Script needs the LowLevelAccess Script to function

namespace ULAR {
    
    [RequireComponent (typeof (MP3LowLevelAccess))]
    public class MP3HighLevelAccess : MonoBehaviour
    {
        
        /*Access this function from another Script to read out the 8 different audio ranges
        * Band: 0-7 Sub Bass, Bass, Low Midrange, Midrange, Upper Midrange, Presence, Brilliance
        * MinScale: The Minnimum the Value can become
        * MaxScale: The Maximum the Value can become
        * SmoothMovement: Smoothing the Values to smoothly blend*/
        public float GetBandValue(int Band=0, float MinScale=1, float MaxScale=1, bool SmoothMovement=true) {
            float value;
            if (SmoothMovement) { value = MP3LowLevelAccess.AudioBandBuffer[Band]*MaxScale + MinScale; }
            else { value = MP3LowLevelAccess.AudioBand[Band]*MaxScale + MinScale; }
            return value;
        }

        // This function gives you the direct current Amplitude of the audio in general, with all frequencies combined
        public float GetRawAmplitude() {
            return MP3LowLevelAccess.GetAmplitude(false);
        }

        // This function gives you the current Amplitude of the audio in general, with all frequencies combined, but smoothed
        public float GetBufferAmplitude() {
            return MP3LowLevelAccess.GetAmplitude(true);
        }

    }
}