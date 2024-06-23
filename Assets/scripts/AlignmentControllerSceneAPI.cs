using Meta.XR.MRUtilityKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class AlignmentControllerSceneAPI : MonoBehaviour
{
    public Vector3 distanceToScreen; 


    [SerializeField] private Transform headPos;
    [SerializeField] private GameObject go;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    private MRUKAnchor screenAnchor;
    private MRUKRoom room;
    Vector3 screenNormalQuest;

    private void Start()
    {
        
    }
    public void SceneLoaded()
    {
        room = FindObjectOfType<MRUKRoom>();

        foreach (MRUKAnchor anchor in room.Anchors)
        {
            if (anchor.HasAnyLabel(MRUKAnchor.SceneLabels.SCREEN))
            {
                screenAnchor = anchor;
            }
        }

        if (screenAnchor == null)
        {
            Debug.Log("No Screen Anchor found in the room");
        }
        else if(go != null)
        {
                // Instantiate the object at the calculated position and orientation
            Instantiate(go, screenAnchor.transform.position, Quaternion.identity);

            if (GameObject.Find("SCREEN"))
            {
                Debug.Log("found screen");

                GameObject cuber = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cuber.name = "Cuberito";
                cuber.transform.position = screenAnchor.transform.position - FindNormalToPlayer(headPos, GameObject.Find("SCREEN"));
            }
        }
            else
            {
                Debug.Log("No Screen Found");
            }
         
    }


    Vector3 FindNormalToPlayer(Transform cameraTransform, GameObject cube)
    {
        // Calculate the direction from the camera to the cube
        Vector3 directionToCube = (cube.transform.position - cameraTransform.position).normalized;

        // Define the normals for the faces of the cube in the cube's local space
        Vector3[] cubeNormals = new Vector3[]
        {
            Vector3.up,    // Top
            Vector3.down,  // Bottom
            Vector3.left,  // Left
            Vector3.right, // Right
            Vector3.forward, // Front
            Vector3.back   // Back
        };

        Vector3 bestNormal = Vector3.zero;
        float bestDot = -1.0f;

        // Iterate through each normal and find the one most aligned with the direction to the camera
        foreach (Vector3 localNormal in cubeNormals)
        {
            // Transform the local normal to world space
            Vector3 worldNormal = cube.transform.TransformDirection(localNormal);

            // Calculate the dot product
            float dot = Vector3.Dot(worldNormal, directionToCube);

            // Check if this is the best dot product we've found
            if (dot > bestDot)
            {
                bestDot = dot;
                bestNormal = worldNormal;
            }
        }

        return bestNormal;
    }



    private void Update()
    {
        if(screenAnchor != null)
        {
            //go.transform.position = screenAnchor.transform.position;

            Debug.DrawRay(go.transform.position, go.transform.position - headPos.position, Color.red);

            distanceToScreen =  SendHeadToScreenPosition();
        }
    }

    private Vector3 SendHeadToScreenPosition()
    {
        Vector3 distance = go.transform.position - headPos.position;

        if(textMeshProUGUI != null)
        {
            textMeshProUGUI.text = distance.ToString(); 
        }

        return distance;
    }
    
}
