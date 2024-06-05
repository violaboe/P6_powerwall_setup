using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Script needs the LowLevelAccess Script to function

namespace ULAR {

    [RequireComponent (typeof (LiveLowLevelAccess))]        
    public class LiveHighLevelAccess : MonoBehaviour
    {
        
        /*Access this function from another Script to read out the 8 different audio ranges
        * Band: 0-7 Sub Bass, Bass, Low Midrange, Midrange, Upper Midrange, Presence, Brilliance
        * MinScale: The Minnimum the Value can become
        * MaxScale: The Maximum the Value can become
        * SmoothMovement: Smoothing the Values to smoothly blend*/
        public float GetBandValue(int Band=0, float MinScale=1, float MaxScale=1, bool SmoothMovement=true) {
            float value;
            value = LiveLowLevelAccess.CreateNormalizedAudioBands(Band, SmoothMovement)*MaxScale + MinScale;
            return value;
        }

        // This function gives you the direct current Amplitude of the audio in general, with all frequencies combined
        public float GetRawAmplitude() {
            return LiveLowLevelAccess.GetAmplitude(false);
        }

        // This function gives you the current Amplitude of the audio in general, with all frequencies combined, but smoothed
        public float GetBufferAmplitude() {
            return LiveLowLevelAccess.GetAmplitude(true);
        }

    }
}