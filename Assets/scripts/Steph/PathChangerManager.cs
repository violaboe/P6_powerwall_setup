using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class PathChangerManager : MonoBehaviour
{
    public GameObject startingPath;

    private List<PathFollower> startingPathFollowers = new List<PathFollower>();
    private List<PathFollower> nextPathFollowers = new List<PathFollower>();

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
            }
            else
            {
                nextPathFollowers.Add(follower);
                follower.enabled = false;
            }
        }

        StartCoroutine(CheckPathCompletion());
    }

    IEnumerator CheckPathCompletion()
    {
        while (true)
        {
            for (int i = 0; i < startingPathFollowers.Count; i++)
            {
                if (startingPathFollowers[i].enabled && HasReachedEnd(startingPathFollowers[i]))
                {
                    SwitchPath(i);
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

    void SwitchPath(int index)
    {
        startingPathFollowers[index].enabled = false;
        nextPathFollowers[index].enabled = true;
        nextPathFollowers[index].distanceTravelled = 0f; // Reset distance for the new path
    }
}
