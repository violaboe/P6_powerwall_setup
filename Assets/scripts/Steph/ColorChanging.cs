using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Samples;
using Unity.VisualScripting;
using UnityEngine;

public class ColorChanging : MonoBehaviour
{
    public PoseRecognitionManager poseRecognitionManager;

    [Header("Materials")]
    [SerializeField]
    private Material startColor;
    [SerializeField]
    private Material stopColor;
    [SerializeField]
    private Material goColor;

    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();

        if (startColor == null || stopColor == null || goColor == null)
        {
            Debug.LogError("Please assign materials in the inspector");
            enabled = false; // disables the script if Materials are not properly assigned
            return;
        }

        rend.material = startColor; // Set the initial color to startColor
    }

    void Update()
    {
        if (poseRecognitionManager.IsPoseActive("StopPoseRight"))
        {
            Debug.Log("STOPPOSERIGHT detected!!");
            rend.material = stopColor;
        }
        else if (poseRecognitionManager.IsPoseActive("ThumbsUpRight"))
        {
            Debug.Log("THUMBSUPRIGHT detected!!");
            rend.material = goColor;
        }
        else
        {
            rend.material = startColor;
        }
    }
}

