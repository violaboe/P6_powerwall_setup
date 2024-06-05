using Meta.XR.MRUtilityKit;
using Oculus.Interaction.Samples;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class roomManager : MonoBehaviour
{
    private MRUKRoom room;
    private List<MRUKAnchor> walls = new List<MRUKAnchor>();
    // Start is called before the first frame update
    void Awake()
    {
    }
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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(walls.Count);
    }
}
