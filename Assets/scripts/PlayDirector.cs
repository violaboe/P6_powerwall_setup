using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR;

public class PlayDirector : MonoBehaviour
{
    private PlayableDirector director;
    public GameObject controlPanel;
    public GameObject plants;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;
    }

    private void Update()
    {
        // Check for the B button press on the Quest 3 controller
        if (IsBButtonPressed())
        {
            StartTimeline();
            plants.SetActive(true);
        }
    }

    private void Director_Stopped(PlayableDirector obj)
    {
        controlPanel.SetActive(true);
    }

    private void Director_Played(PlayableDirector obj)
    {
        controlPanel.SetActive(false);
    }

    public void StartTimeline()
    {
        director.Play();
    }

    private bool IsBButtonPressed()
    {
        // Check if the B button is pressed on the right hand controller
        bool bButtonPressed = false;
        InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (rightHandDevice.isValid)
        {
            rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bButtonPressed);
        }

        return bButtonPressed;
    }
}
