using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example script to show how you can create your own scripts for the audio feed using MP3 Files
// It scacles the 8 example cubes up and down, depending on the audio level

namespace ULAR {
public class CubeScaler : MonoBehaviour
{

    // You need this line to be able to link the GameObjct that has the HighLevelAccess Script on it
    // Note that you either call it "HighLevelAccess" or "MP3HighLevelAccess". Depending on if you want live audio or MP3 audio.
    public MP3HighLevelAccess Script; 

    // You don't need these settings as public variables. But it obviously helps to change stuff during runtime :P
    public int Band;
    public float MaxScale = 10;
    public bool Smooth;
    
    // Then just do whatever else your code is ment to do
    // From this point on, it is all up to you.

    void Update()
    {
        transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, Script.GetBandValue(Band, 1, MaxScale, Smooth));
    }
}
}