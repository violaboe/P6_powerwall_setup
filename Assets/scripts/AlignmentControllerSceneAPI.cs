using Meta.XR.MRUtilityKit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlignmentControllerSceneAPI : MonoBehaviour
{
    public Vector3 distanceToScreen; 


    [SerializeField] private Transform headPos;
    [SerializeField] private GameObject go;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    private MRUKAnchor screenAnchor;
    private MRUKRoom room;

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
            go = Instantiate(go, screenAnchor.transform.position, Quaternion.identity);
        }

    }
   
    private void Update()
    {
        if(screenAnchor != null)
        {
            go.transform.position = screenAnchor.transform.position;

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
