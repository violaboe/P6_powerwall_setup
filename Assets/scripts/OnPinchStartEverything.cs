using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPinchStartEverything : MonoBehaviour
{
    public GameObject stephStuff;

    void Update()
    {
        if (stephStuff != null)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                stephStuff.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Steph's Stuff not assigned");
        }
    }
}
