using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class PathChangerManager : MonoBehaviour
{
    public ParticleTriggerHandler particleTriggerHandlerL;
    public ParticleTriggerHandler particleTriggerHandlerR;
    public GameObject startingPath;
    public GameObject path1;
    public GameObject path2;
    public GameObject endPath;

    private List<PathFollower> startingPathFollowers = new List<PathFollower>();
    private HashSet<PathFollower> pushedFollowers = new HashSet<PathFollower>();
    private HashSet<PathFollower> switchedToEndPathFollowers = new HashSet<PathFollower>();

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

        particleTriggerHandlerL.OnParticlesPushed += OnParticlesPushed;
        particleTriggerHandlerR.OnParticlesPushed += OnParticlesPushed;

        StartCoroutine(CheckPathCompletion());
    }

    private void OnParticlesPushed(ParticleSystem particleSystem)
    {
        foreach (var follower in startingPathFollowers)
        {
            if ((follower.gameObject.name == "Fireflies_1" && particleSystem == particleTriggerHandlerL.fireflyParticles1) ||
                (follower.gameObject.name == "Fireflies_2" && particleSystem == particleTriggerHandlerL.fireflyParticles2) ||
                (follower.gameObject.name == "Fireflies_1" && particleSystem == particleTriggerHandlerR.fireflyParticles1) ||
                (follower.gameObject.name == "Fireflies_2" && particleSystem == particleTriggerHandlerR.fireflyParticles2))
            {
                pushedFollowers.Add(follower);
                Debug.Log($"Particle system pushed for {follower.gameObject.name}");
            }
        }
    }

    IEnumerator CheckPathCompletion()
    {
        while (true)
        {
            foreach (PathFollower follower in startingPathFollowers)
            {
                // If the follower has been pushed and is on path1 or path2, switch to the end path
                if (pushedFollowers.Contains(follower) && IsOnPath(follower, path1, path2))
                {
                    if (!switchedToEndPathFollowers.Contains(follower))
                    {
                        SwitchToEndPath(follower);
                        switchedToEndPathFollowers.Add(follower);
                        Debug.Log($"Switching to end path for {follower.gameObject.name}");
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

    bool HasReachedEnd(PathFollower follower)
    {
        float pathLength = follower.pathCreator.path.length;
        return follower.endOfPathInstruction == EndOfPathInstruction.Stop && follower.distanceTravelled >= pathLength;
    }

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
        else
        {
            Debug.LogWarning("Path not assigned, invalid GameObject name or path is null");
        }

        // Change EndOfPathInstruction to Loop
        follower.endOfPathInstruction = EndOfPathInstruction.Loop;
        Debug.Log("Changed EndOfPathInstruction to Loop");

        follower.speed = 30f;
        Debug.Log("Changed follower speed to 30");

        // Reset distance and re-enable the follower
        follower.distanceTravelled = 0f;
        follower.enabled = true;
    }

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

        follower.speed = 10f;
        Debug.Log("Changed follower speed to 10");

        // Reset distance and re-enable the follower
        follower.distanceTravelled = 0f;
        follower.enabled = true;
    }

    bool IsOnPath(PathFollower follower, GameObject path1, GameObject path2)
    {
        return follower.pathCreator.gameObject == path1 || follower.pathCreator.gameObject == path2;
    }
}
