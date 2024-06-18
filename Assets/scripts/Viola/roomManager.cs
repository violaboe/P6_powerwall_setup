using Meta.XR.MRUtilityKit;
using Oculus.Interaction.Samples;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private MRUKRoom room;
    private List<MRUKAnchor> walls = new List<MRUKAnchor>();
    // Start is called before the first frame update
  
    public void SceneLoaded()
    {
        room = FindObjectOfType<MRUKRoom>();

        foreach (MRUKAnchor anchor in room.Anchors)
        {
            if (anchor.HasAnyLabel(MRUKAnchor.SceneLabels.WALL_FACE))
            {
                walls.Add(anchor);
            }
        }

        Debug.Log(walls.Count);
    }

    public List<GameObject> GetAllWalls()
    {
        List<GameObject> wallGOs = new List<GameObject>();

        foreach (MRUKAnchor anchor in room.Anchors)
        {
            wallGOs.Add(anchor.transform.GetChild(0).gameObject); 
        }
        return wallGOs;
    }
}
