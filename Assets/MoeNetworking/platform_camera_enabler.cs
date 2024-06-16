using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_camera_enabler : MonoBehaviour
{
    public GameObject oculus_camera_rig;
    public GameObject powerwall_camera_rig;

    private void Awake()
    {
        if(Application.platform == RuntimePlatform.Android && oculus_camera_rig != null)
        {
            oculus_camera_rig.SetActive(true);
        }
        if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) && powerwall_camera_rig != null)
        {
            powerwall_camera_rig.SetActive(true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
