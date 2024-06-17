using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using Oculus.Interaction.Samples;
using UnityEngine;
using UnityEngine.Events;

public class FloatingAnimation : MonoBehaviour
{

    public PoseRecognitionManager poseRecognitionManager;

    [Header("Animation")]
    [SerializeField]
    public Transform centerObject;
    public float minHeight = 0.0f;
    public float maxHeight = 1.0f;
    public float floatSpeed = 0.5f;
    public float radius = 0.5f;
    public float angularSpeed = 50.0f;
    private float initialY;
    //private bool hasDonePose = false;

    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;    
    }

    // Update is called once per frame
    void Update()
    {
        floatAnim();

        if (poseRecognitionManager.IsPoseActive("PaperRight"))
        {
            // stops the floating motion
            Debug.Log("PAPERRIGHT detected!!");
            enabled = false;
            //hasDonePose = true;
            this.gameObject.SetActive(false);
        }
        else
        {
            enabled = true;
            this.gameObject.SetActive(true);
        }
    }

    private void floatAnim()
    {
        float newY = Mathf.PingPong(Time.time * floatSpeed, maxHeight - minHeight) + minHeight;

        float angle = Time.time * angularSpeed * Mathf.Deg2Rad;
        float newX = Mathf.Cos(angle) * radius;
        float newZ = Mathf.Sin(angle) * radius;

        transform.position = new Vector3(centerObject.position.x + newX, newY, centerObject.position.z + newZ);
    }
}