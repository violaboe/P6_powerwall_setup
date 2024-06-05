using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadertest : MonoBehaviour
{
    public GameObject noShaderObjects;
    public GameObject shaderObjects;
    private bool shaderOn;
    public List<GameObject> objects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)|| OVRInput.GetDown(OVRInput.Button.One)) 
        { 
            shaderOn = !shaderOn;
        }
        if(shaderOn)
        {
            shaderObjects.SetActive(true);
            noShaderObjects.SetActive(false);
        }
        else
        {
            shaderObjects.SetActive(false);
            noShaderObjects.SetActive(true);
        }
    }
}
