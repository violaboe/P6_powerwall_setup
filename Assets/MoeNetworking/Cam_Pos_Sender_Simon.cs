using FishNet.Object;
using FishNet.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;


public class Cam_Pos_Sender_Simon : NetworkBehaviour
{
    public GameObject oculus_camera_rig_S;
    public GameObject oculus_head_pos_obj_S;
    public GameObject powerwall_cam_S;

    public Vector3 head_pos_S = Vector3.zero;

    public Animator vert_anim_S;
    public Animator screen_anim_S;

    private Vector3 powerwall_cam_offset_S;
    private Vector3 powerwall_offset_S;

    public AlignmentDebug aligmentDebug_S;
    public TimeLineStarter timeLineStarter_S;




    private void Start()
    {
        aligmentDebug_S = GameObject.FindAnyObjectByType<AlignmentDebug>();

        timeLineStarter_S = GameObject.FindAnyObjectByType<TimeLineStarter>();


    }

    // Update is called once per frame
    void Update()
    {

        if (oculus_camera_rig_S != null && oculus_head_pos_obj_S != null && oculus_camera_rig_S.activeInHierarchy)
        {
            send_head_pos_to_server_S(oculus_head_pos_obj_S.transform.position);
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void send_head_pos_to_server_S(Vector3 _pos)
    {
        Debug.Log("ello");

        send_head_pos_powerwall_S(_pos);

        Debug.Log("on server: " + _pos);
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void send_head_pos_powerwall_S(Vector3 _pos)
    {
        if (powerwall_cam_S != null)
        {
            if (head_pos_S.y == 0.0f)
            {
                powerwall_cam_offset_S = _pos;
            }
            head_pos_S = _pos;
            powerwall_cam_S.transform.position = _pos - new Vector3(0.0f, 0, 0.5f);
        }
        Debug.Log("on client: " + _pos);



    }


    [ServerRpc(RequireOwnership = false)]
    public void start_vert_anim_server_S()
    {
        start_vert_anim_observer_S();
    }

    [ObserversRpc]
    private void start_vert_anim_observer_S()
    {
        vert_anim_S.SetTrigger("vert_trigger");
    }

    [ServerRpc(RequireOwnership = false)]
    public void start_screen_anim_server_S()
    {
        start_screen_anim_observer_S();
    }

    [ObserversRpc]
    private void start_screen_anim_observer_S()
    {
        screen_anim_S.SetTrigger("screen_trigger");
    }

    public void print_rig_status_S()
    {
        print_rig_status_on_server_S(base.LocalConnection, oculus_camera_rig_S != null, oculus_head_pos_obj_S != null, oculus_camera_rig_S.activeInHierarchy);
    }

    [ServerRpc]
    public void print_rig_status_on_server_S(NetworkConnection conn, bool rig_exists, bool cam_exists, bool rig_active)
    {
        Debug.Log("on client: " + conn + " with platform: " + Application.platform + " rig exists: " + rig_exists + " cam exists: " + cam_exists + " oculus rig active: " + rig_active);
    }

    public void print_pos_once_S()
    {
        print_pos_once_on_server_S(head_pos_S);
    }

    [ServerRpc]
    public void print_pos_once_on_server_S(Vector3 _pos)
    {
        Debug.Log("im at: " + _pos);
    }



    [ServerRpc(RequireOwnership = false)]
    public void DebugOffServer_S()
    {
        DebugOffObserver_S();
    }

    [ObserversRpc]
    private void DebugOffObserver_S()
    {
        aligmentDebug_S.DebugOff();
    }


    [ServerRpc(RequireOwnership = false)]
    public void DebugOnServer_S()
    {
        DebugOnObserver_S();
    }

    [ObserversRpc]
    private void DebugOnObserver_S()
    {
        aligmentDebug_S.DebugOn();
    }



    [ServerRpc(RequireOwnership = false)]
    public void StartTimeLineServer_S()
    {
        StartTimeLineObserver_S();
    }

    [ObserversRpc]
    private void StartTimeLineObserver_S()
    {
        timeLineStarter_S.StartTimeline();

    }



}


