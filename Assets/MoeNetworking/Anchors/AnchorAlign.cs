using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorAlign : MonoBehaviour
{
    [SerializeField]
    private GameObject sceneObjects;

    [SerializeField]
    private GameObject anchorRotation;

    
    private MeshRenderer capsuleMeshRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        sceneObjects = GameObject.Find("AnchorSource");
        capsuleMeshRenderer = GameObject.Find("MyCapsule").GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AlignObjectsToScene();
        }
    }

    public void AlignObjectsToScene()
    {
        capsuleMeshRenderer.material.color = Random.ColorHSV();
        sceneObjects.transform.position = anchorRotation.transform.position;
        sceneObjects.transform.rotation = anchorRotation.transform.rotation;        
    }
}
