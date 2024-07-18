using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using Unity.Mathematics;

public class PathChangerManager : MonoBehaviour
{
    public CustomMeshEmitter customMeshEmitter;
    public FireflyFlickering fireflyFlickering;
    public ParticleTriggerHandler particleTriggerHandlerL; //ParticleTriggerHandler on OculusHands
    public ParticleTriggerHandler particleTriggerHandlerR;

    public GameObject startingPath;
    public GameObject path1;
    public GameObject path2;
    public GameObject path3;
    //public GameObject path4;
    public GameObject endPath;

    public Material yellowMaterial;

    private List<PathFollower> startingPathFollowers = new List<PathFollower>(); 
    private HashSet<PathFollower> pushedFollowers = new HashSet<PathFollower>(); //HashSet for pushedParticles
    private HashSet<PathFollower> switchedToEndPathFollowers = new HashSet<PathFollower>(); //HashSet for particles that are going to the jar
    private HashSet<PathFollower> reachedEndPathFollowers = new HashSet<PathFollower>(); //HashSet for particles that reached end of endPath


    [SerializeField]
    private Texture2D _2dColorLUT;


    private OVRPassthroughColorLut lutTexturePulse;

    [SerializeField]
    private OVRPassthroughLayer ovrPassPulse;

    [SerializeField]
    private GameObject SiblingsInJar;

    [SerializeField]
    private GameObject EndTutorial;

    [SerializeField]
    private AudioSource playDirectV;


    void Start()
    {
        // Get all PathFollower components in the children
        PathFollower[] followers = GetComponentsInChildren<PathFollower>();

        foreach (PathFollower follower in followers)
        {
            if (follower.pathCreator.gameObject == startingPath)
            {
                startingPathFollowers.Add(follower);
                follower.enabled = true;
                follower.endOfPathInstruction = EndOfPathInstruction.Stop;
                Debug.Log("Initialized follower with Stop instruction");
            }
        }

        // Disable Hand interactions on start
        particleTriggerHandlerL.enabled = false;
        particleTriggerHandlerR.enabled = false;

        particleTriggerHandlerL.OnParticlesPushed += OnParticlesPushed;
        particleTriggerHandlerR.OnParticlesPushed += OnParticlesPushed;

        StartCoroutine(CheckPathCompletion());

        playDirectV = FindAnyObjectByType<PlayDirector>().GetComponentInParent<AudioSource>();
    }



    //This is to check which particles have been pushed by which hand specifically
    private void OnParticlesPushed(ParticleSystem particleSystem)
    {
        if (particleTriggerHandlerR.enabled && particleTriggerHandlerL.enabled)
        {
            foreach (var follower in startingPathFollowers)
            {
                if ((follower.gameObject.name == "Fireflies_1" && particleSystem == particleTriggerHandlerL.fireflyParticles1) ||
                    (follower.gameObject.name == "Fireflies_2" && particleSystem == particleTriggerHandlerL.fireflyParticles2) ||
                    (follower.gameObject.name == "Fireflies_3" && particleSystem == particleTriggerHandlerL.fireflyParticles3) ||
                    //(follower.gameObject.name == "Fireflies_4" && particleSystem == particleTriggerHandlerL.fireflyParticles4) ||
                    (follower.gameObject.name == "Fireflies_1" && particleSystem == particleTriggerHandlerR.fireflyParticles1) ||
                    (follower.gameObject.name == "Fireflies_2" && particleSystem == particleTriggerHandlerR.fireflyParticles2) ||
                    (follower.gameObject.name == "Fireflies_3" && particleSystem == particleTriggerHandlerR.fireflyParticles3))
                    //(follower.gameObject.name == "Fireflies_4" && particleSystem == particleTriggerHandlerR.fireflyParticles4))
                {
                pushedFollowers.Add(follower);
                Debug.Log($"Particle system pushed for {follower.gameObject.name}");
                
                }
            }
        }
    }

    // Coroutine to check on which path each particle system is currently at (Start-Path1/2-End)
    IEnumerator CheckPathCompletion()
    {
        while (true)
        {
            foreach (PathFollower follower in startingPathFollowers)
            {
                // If the follower has been pushed and is on path1 or path2, switch to the end path
                if (pushedFollowers.Contains(follower) && IsOnPath(follower, path1, path2, path3))
                {
                    if (!switchedToEndPathFollowers.Contains(follower))
                    {
                        SwitchToEndPath(follower);
                        switchedToEndPathFollowers.Add(follower);
                        Debug.Log($"Switching to end path for {follower.gameObject.name}");
                        
                    }
                }
                else if (switchedToEndPathFollowers.Contains(follower) && HasReachedEnd(follower))
                {
                    if (!reachedEndPathFollowers.Contains(follower))
                    {
                        //HandleReachedEndPath(follower);
                        reachedEndPathFollowers.Add(follower);
                        Debug.Log($"Reached end of end path for {follower.gameObject.name}");
                    }
                }

                else if (follower.enabled && HasReachedEnd(follower))
                {
                    Debug.Log($"{follower.gameObject.name} reached end of starting path.");

                    if (switchedToEndPathFollowers.Contains(follower))
                    {
                        Debug.Log($"{follower.gameObject.name} already switched to end path.");
                        continue;
                    }

                    // Regular path switching logic
                    SwitchPath(follower);
                    Debug.Log($"Switching path for {follower.gameObject.name}");
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    //Bool to check whether particles have reached the end of the Starting Path so it'll automatically switch to Path1 or Path2
    bool HasReachedEnd(PathFollower follower)
    {
        float pathLength = follower.pathCreator.path.length;
        return follower.endOfPathInstruction == EndOfPathInstruction.Stop && follower.distanceTravelled >= pathLength;
    }

    IEnumerator TriggerHandlerActivationDelay(PathFollower follower)
    {
        yield return new WaitForSeconds(3f);  // Wait for x seconds before switching to Middle Path
        // Activate user hand interactions
        particleTriggerHandlerL.enabled = true;
        particleTriggerHandlerR.enabled = true;
    }

    //Function to call for switching particle paths (Start-either Path1/Path2)
    void SwitchPath(PathFollower follower)
    {
        
        // Disable the follower to change its path
        follower.enabled = false;
        Debug.Log($"Switching path for {follower.gameObject.name}");

        // Assign the new path based on the GameObject name
        if (follower.gameObject.name == "Fireflies_1" && path1 != null)
        {
            follower.pathCreator = path1.GetComponent<PathCreator>();
            Debug.Log("Assigned Path1 to follower");
        }
        else if (follower.gameObject.name == "Fireflies_2" && path2 != null)
        {
            follower.pathCreator = path2.GetComponent<PathCreator>();
            Debug.Log("Assigned Path2 to follower");
        }
        else if (follower.gameObject.name == "Fireflies_3" && path3 != null)
        {
            follower.pathCreator = path3.GetComponent<PathCreator>();
            Debug.Log("Assigned Path3 to follower");
        }
        /*else if (follower.gameObject.name == "Fireflies_4" && path4 != null)
        {
            follower.pathCreator = path4.GetComponent<PathCreator>();
            Debug.Log("Assigned Path4 to follower");
        }*/
        else
        {
            Debug.LogWarning("Path not assigned, invalid GameObject name or path is null");
        }

        StartCoroutine(TriggerHandlerActivationDelay(follower));

        // Change EndOfPathInstruction to Loop
        follower.endOfPathInstruction = EndOfPathInstruction.Loop;
        Debug.Log("Changed EndOfPathInstruction to Loop");

        follower.speed = 30f;
        Debug.Log("Changed follower speed to 30");

        // Reset distance and re-enable the follower
        follower.distanceTravelled = 0f;
        follower.enabled = true;

        /*ParticleSystem[] particleSystems = follower.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            if (ps != null)
            {
                var noiseModule = ps.noise;
                noiseModule.enabled = true;
            }
        }*/
    }


        //Function to call for switching particle paths (Path1/Path2-End)
    void SwitchToEndPath(PathFollower follower)
    {
        // Disable the follower to change its path
        follower.enabled = false;
        Debug.Log($"Switching to end path for {follower.gameObject.name}");

        // Assign the end path
        follower.pathCreator = endPath.GetComponent<PathCreator>();
        Debug.Log("Assigned endPath to follower");

        // Change EndOfPathInstruction to Stop
        follower.endOfPathInstruction = EndOfPathInstruction.Stop;
        Debug.Log("Changed EndOfPathInstruction to Stop");

        follower.speed = 5f;
        Debug.Log("Changed follower speed to 5");

        // Reset distance and re-enable the follower
        follower.distanceTravelled = 0f;
        follower.enabled = true;

        // Find the pushed particle systems associated with this follower
        List<ParticleSystem> pushedParticleSystems = new List<ParticleSystem>();
        ParticleSystem[] allParticleSystems = follower.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in allParticleSystems)
        {
            if (pushedFollowers.Contains(follower))
            {
                pushedParticleSystems.Add(ps);
            }
        }

        StartCoroutine(fireflyFlickering.FlickerAndChangeMaterial(pushedParticleSystems));
        // Access the particle systems associated with the follower
        ParticleSystem[] particleSystems = follower.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            if (ps != null)
            {
                var velocityModule = ps.velocityOverLifetime;
                velocityModule.radial = new ParticleSystem.MinMaxCurve(0.0f);
                Debug.Log("Set radial value to 0 in velocity over lifetime module");

                var noiseModule = ps.noise;
                noiseModule.enabled = false;

                customMeshEmitter.endPathMesh(ps);
            }
        }
        // Access the Particle Gravity Center scripts with the follower
        ParticleGravityCenter[] gravityCenters = follower.GetComponentsInChildren<ParticleGravityCenter>();
        foreach (var gravityCenter in gravityCenters)
        {
            if (gravityCenter != null)
            {
                gravityCenter.pushBackStrength = 0;
                gravityCenter.pushBackRadius = 0;

                gravityCenter.SetDampen(0.2f);
            }
        }
        Debug.Log("$Distance: {follower.distanceTravelled}");
        if (follower.distanceTravelled >= 10.0f)
        {
            Debug.Log("Follower has travelled 20 distance");
            foreach (var gravityCenter in gravityCenters)
            {
                if (gravityCenter != null)
                {
                    gravityCenter.SetDampen(0.5f);
                    Debug.Log("Set dampen value to 0.5");

                    particleTriggerHandlerL.enabled = false;
                    particleTriggerHandlerR.enabled = false;
                    Debug.Log("Hands have been disabled");
                    //gravityCenter.ApplyPullingForce();
                    follower.speed = 20;
                }
            }
        }
    }

    //Bool to check if the Particles are on Path1 or Path2
    bool IsOnPath(PathFollower follower, GameObject path1, GameObject path2, GameObject path3)
    {
        return follower.pathCreator.gameObject == path1 || follower.pathCreator.gameObject == path2 || follower.pathCreator.gameObject == path3;
    }


    private void Update()
    {

        Debug.Log(switchedToEndPathFollowers.Count);
        if(switchedToEndPathFollowers.Count >= 3)
        {
            
            
                //lutTexturePulse = new OVRPassthroughColorLut(_2dColorLUT, true);
                //ovrPassPulse.SetColorLut(lutTexturePulse, 1);
                StartCoroutine(StartTheEnd());

            

        }
    }


    IEnumerator StartTheEnd()
    {
        yield return new WaitForSeconds(10f);
        SiblingsInJar.SetActive(true);

        yield return new WaitForSeconds(10f);
        EndTutorial.SetActive(true);
        playDirectV.Stop();

        yield return new WaitForSeconds(10f);
        Destroy(EndTutorial);



    }
}
