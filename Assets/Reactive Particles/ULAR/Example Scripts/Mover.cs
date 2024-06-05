using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example script to show how you can create your own scripts for the audio feed using MP3 Files
// It moves the camera forward and backward, depending on the audio level
namespace ULAR {
    public class Mover : MonoBehaviour
    {
        // You need this line to be able to link the GameObjct that has the HighLevelAccess Script on it
        // Note that you either call it "HighLevelAccess" or "MP3HighLevelAccess". Depending on if you want live audio or MP3 audio.
        public LiveHighLevelAccess Script;

        // Then just do whatever else your code is ment to do
        // In this example, we want to import the camera to make it audio reactive
        public GameObject Cam;

        // You don't need these settings as public variables. But it obviously helps to change stuff during runtime :P
        public int Band;
        public float MaxScale;
        public float MinScale;

        void Update()
        {
            // From this point on, it is all up to you. In this example, I am changing the cameras transform
            // X and Y remain the original camera numbers. While Z gets changed by the scripts function. 
            // For the function "documentation", feel free to look into the "LowerLevelAccess" Script. It states the actual GetBandValue function + information
            transform.position = new Vector3 (transform.position.x, transform.position.y, Script.GetBandValue(Band, MinScale, MaxScale, true));
        }
    }
}