using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentDebug : MonoBehaviour
{
    public GameObject blockOut;
    public GameObject passthroughWall;
    public GameObject debugEnv;

    bool debugOn;


    // Start is called before the first frame update
    void Start()
    {
        //blockOut = GameObject.Find("Blockout");
        debugOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            DebugOn();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            DebugOff();
        }
    }


    public void DebugOn()
    {

        
        
            //TurnOff TurnOff BlockOut Env
            if (blockOut != null) blockOut.SetActive(false);


            //TurnOff Passthrough
            passthroughWall.SetActive(false);

            //TurnOn Debug Enviromet
            debugEnv.SetActive(true);

            
        
        

        
    }


    public void DebugOff()
    {

        
        
            //TurnOff TurnOff BlockOut Env
            if (blockOut != null) blockOut.SetActive(true);


            //TurnOn Passthrough
            //passthroughWall.SetActive(true);

            //TurnOn Debug Enviromet
            debugEnv.SetActive(false);

            
        
       
    }
}
