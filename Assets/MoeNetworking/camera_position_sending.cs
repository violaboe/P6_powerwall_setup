using FishNet.Object;
using FishNet.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;

public class camera_position_sending : NetworkBehaviour
{
    public GameObject oculus_camera_rig;
    public GameObject oculus_head_pos_obj;
    public GameObject powerwall_cam;

    public Vector3 head_pos = Vector3.zero;

    public Animator vert_anim;
    public Animator screen_anim;

    private Vector3 powerwall_cam_offset;
    private Vector3 powerwall_offset;

    public AlignmentDebug aligmentDebug;

    public PlayDirectorTobi timeLineStarter;
    public PlayDirector VPlayer;

    public Vector3 vectorToScreenMultiplayer;
    public Vector3 planNormalMultiplayer;

    public AlignmentControllerQuest alignmentControllerQuest;
    public AlignmentControllerCave alignmentControllerCave;

    [SerializeField]
    private GameObject stephParticles;

    public GameObject redSphere;




    private void Start()
    {
        aligmentDebug = GameObject.FindAnyObjectByType<AlignmentDebug>();

        timeLineStarter = GameObject.FindAnyObjectByType<PlayDirectorTobi>();

        VPlayer = GameObject.FindAnyObjectByType<PlayDirector>();

        alignmentControllerQuest = GameObject.FindAnyObjectByType<AlignmentControllerQuest>();
        alignmentControllerCave = GameObject.FindAnyObjectByType<AlignmentControllerCave>();
    }

    // Update is called once per frame
    void Update()
    {

        if (alignmentControllerQuest.isSceneAligned)
        {
            vectorToScreenMultiplayer = alignmentControllerQuest.VectorToScreen;
            planNormalMultiplayer = alignmentControllerQuest.planeNormal;
        }
        

        if (oculus_camera_rig != null && oculus_head_pos_obj != null && oculus_camera_rig.activeInHierarchy && alignmentControllerQuest.isSceneAligned)
        {
            send_head_pos_to_server(vectorToScreenMultiplayer, planNormalMultiplayer);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveRedSphereForward();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void send_head_pos_to_server(Vector3 _pos, Vector3 planeNormal)
    {
        Debug.Log("ello");

        send_head_pos_powerwall(_pos, planeNormal);

        Debug.Log("on server: " + _pos);
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void send_head_pos_powerwall(Vector3 _pos, Vector3 planeNormal)
    {
        //if(powerwall_cam != null)
        //{
        //    if(head_pos.y == 0.0f)
        //    {
        //        powerwall_cam_offset = _pos;
        //    }
        //    head_pos = _pos;
        //    powerwall_cam.transform.position = _pos - new Vector3(0.0f, 0, 0.5f);
        //}
        //Debug.Log("on client: " + _pos);

        Debug.Log("on client: " + _pos + "Normal: " + planeNormal);
        alignmentControllerCave.RepositionCaveCamera(_pos, planeNormal);
        
    }


    [ServerRpc(RequireOwnership = false)]
    public void start_vert_anim_server()
    {
        start_vert_anim_observer();
    }

    [ObserversRpc]
    private void start_vert_anim_observer()
    {
        vert_anim.SetTrigger("vert_trigger");
    }

    [ServerRpc(RequireOwnership = false)]
    public void start_screen_anim_server()
    {
        start_screen_anim_observer();
    }

    [ObserversRpc]
    private void start_screen_anim_observer()
    {
        //screen_anim.SetTrigger("screen_trigger");
        MoveRedSphereForward();
    }

    public void print_rig_status()
    {
        print_rig_status_on_server(base.LocalConnection, oculus_camera_rig != null, oculus_head_pos_obj != null, oculus_camera_rig.activeInHierarchy);
    }

    [ServerRpc]
    public void print_rig_status_on_server(NetworkConnection conn, bool rig_exists, bool cam_exists, bool rig_active)
    {
        Debug.Log("on client: " + conn + " with platform: " + Application.platform + " rig exists: " + rig_exists + " cam exists: " + cam_exists + " oculus rig active: " + rig_active);
    }

    public void print_pos_once()
    {
        print_pos_once_on_server(head_pos);
    }

    [ServerRpc]
    public void print_pos_once_on_server(Vector3 _pos)
    {
        Debug.Log("im at: " + _pos);
    }



    [ServerRpc(RequireOwnership = false)]
    public void DebugOffServer()
    {
        DebugOffObserver();
    }

    [ObserversRpc]
    private void DebugOffObserver()
    {
        aligmentDebug.DebugOff();
    }


    [ServerRpc(RequireOwnership = false)]
    public void DebugOnServer()
    {
        DebugOnObserver();
    }

    [ObserversRpc]
    private void DebugOnObserver()
    {
        aligmentDebug.DebugOn();
    }



    [ServerRpc(RequireOwnership = false)]
    public void StartTimeLineServer()
    {
        StartTimeLineObserver();
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void StartTimeLineObserver()
    {
        VPlayer.director.Play();
        //timeLineStarter.StartTimeline();
        StartCoroutine(StartTobiScene());
       // StartCoroutine(StartParticlesSteph());
    }



    IEnumerator StartParticlesSteph()
    {
        yield return new WaitForSeconds(72.63f);
        stephParticles.SetActive(true);
    }

   

    public void MoveRedSphereForward()
    {
        this.Animate(redSphere.transform, Easing.AnimationType.LocalPosition, Easing.Ease.EaseInQuad, redSphere.transform.localPosition, redSphere.transform.localPosition + new Vector3(0, 0, -10), 3f, MoveRedSphereBackward);
    }

    public void MoveRedSphereBackward()
    {
        this.Animate(redSphere.transform, Easing.AnimationType.LocalPosition, Easing.Ease.EaseInQuad, redSphere.transform.localPosition, redSphere.transform.localPosition + new Vector3(0, 0, 10), 3f, MoveRedSphereForward);
    }

    IEnumerator StartTobiScene()
    {
        yield return new WaitForSeconds(21.21667f);
        timeLineStarter.StartTimelineTobi();

    }


    public void StartEverything()
    {
        StartTimeLineServer();
        VPlayer.director.Play();
        StartCoroutine(StartParticlesSteph());
    }
}

