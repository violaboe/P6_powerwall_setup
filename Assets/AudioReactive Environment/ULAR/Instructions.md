# Welcome to the ULAR quick start guide

## Preparing the ULAR Controller

To begin using ULAR, you need to first set up its Controller. After that, other objects and scripts will use this Controller to make them react to audio.

1. Import ULAR into your project (since you are reading this, you're probably already done there)

2. In your Object Hierarchy, create a new object. It can be anything you desire. An empty object is preferable, however.
    > You may also rename it "ULAR Controller"

3. Once your object is in your scene, click the "Add Component" button in the Objects Inspector.

4. Based on what recording source you'd like to use, add one of these components:

    - For live audio, that is, audio coming from your speakers (system audio), add the component "LiveHighLevelAccess"
    - For an audio source within Unity, add the component "MP3HighLevelAccess"

## Accessing the Audio Values

Now that you've set up ULAR's Controller, you can connect it to the object you'd like to get to react to audio. The setup process will be almost identical for both live audio and Unity audio sources.

Right now, your Controller object now has a High Level Access script. This script connects to ULAR's Low Level Access and exposes simple functions for you to get useful data.

1. In the object(s) you want to connect to ULAR, add a component. Then, you can either create a new Script or use one of ULAR's example scripts, such as `CubeScaler`, `Mover`, or `Scaler`.

2. When coding, all you need to do is create variables for the Access Layer of your choice.
   - When using the Live Audio High Level, you need to add the following variable: `public LiveHighLevelAccess Script;`
   - For the Live Audio Low Level, you do: `public LiveLowLevelAccess Script;`
  
    The same applies to the Unity Audio Source. Just instead of "Live", you write "MP3"; e.g. `public MP3HighLevelAccess Script;` `public MP3LowLevelAccess Script;`.

    > The variable name "Script" is a placeholder and can be anything you like.

3. You now need to select your controller object from before in the Inspector under the `Script` input for the component we just added.

After that point, the Syntax is identical for both Live Audio and Audio Source. In the HighLevelAccess scripts, you will find a few functions, such as the `GetBandValue()`. You can read about their usage and parameters within the function definition.

The same applies to the LowLevelAccess Scripts. In these, you can access Arrays of multiple values and multiple functions. Be sure to read the comments and information provided within the script carefully.

Using the `GetBandValue()` function, we can now get the value of a certain frequency by doing `Script.GetBandValue(0, 1, 10, true)`. This function call will give us the value of the 0th Band, the Bass. From a minimum scale of 1 and a maximum scale of 10. The values will also the smoothed.

As the last example, if we were to get the overall amplitude of the audio, we simply call `Script.GetRawAmplitude()`, which will return a value of the current audio amplitude.

---

Everything beyond that point is up to you. You can use these values to control scale, location, rotation, speed, materials, and anything that you can come up with that is changeable with Unity over Script.

If you need some inspiration, check out our Example Scene.
