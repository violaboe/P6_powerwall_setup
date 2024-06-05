using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeWalls : MonoBehaviour
{
    public Material neon;
    public roomManager roomManager;
    private List<GameObject> walls = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SceneLoaded()
    {
        walls = roomManager.GetAllWalls();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "WALL_FACE_EffectMesh")
        {
            other.gameObject.GetComponent<MeshRenderer>().material.color = ;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
